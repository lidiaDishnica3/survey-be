using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using InternalSurvey.Api.Data;
using InternalSurvey.Api.Dtos;
using InternalSurvey.Api.Entities;
using InternalSurvey.Api.Helpers;
using InternalSurvey.Api.Helpers.Email.Interfaces;
using InternalSurvey.Api.Interfaces;
using InternalSurvey.Api.Repository;
using InternalSurvey.Api.Services;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InternalSurvey.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class SurveyController : BaseController<SurveyController>
  {
    #region properties
    //services
    private readonly ISurveyService _surveysService;
    private readonly IEmailService _emailService;
    private readonly IRespondentService _respondentService;
    private readonly IQuestionsService _questionService;
    private readonly ICommentService _commentService;
    private readonly ISurveyQuestionOptionsService _surveyQuestionOptionsService;
    private readonly IGenerateUniqueLinkService _generateUniqueLinkService;
    private readonly IImageService _imageService;
    //automapper
    private readonly IMapper _mapper;
    //logger
    private readonly ILogger<SurveyController> _logger;
    //context for transaction
    private readonly SurveyAppContext _context;
    IConfiguration _configuration;

    #endregion

    #region constructor
    public SurveyController(ISurveyService surveysService, IEmailService emailService
        , IRespondentService respondentService, IQuestionsService questionService, ISurveyQuestionOptionsService surveyQuestionOptionsService
        , IGenerateUniqueLinkService generateUniqueLinkService, IImageService imageService, SurveyAppContext context, IConfiguration configuration, ILogger<SurveyController> logger, IMapper mapper,
     ICommentService commentService) : base(logger)
    {
      _logger = logger;
      _mapper = mapper;
      _surveysService = surveysService;
      _emailService = emailService;
      _respondentService = respondentService;
      _questionService = questionService;
      _commentService = commentService;
      _surveyQuestionOptionsService = surveyQuestionOptionsService;
      _generateUniqueLinkService = generateUniqueLinkService;
      _context = context;
      _imageService = imageService;
      _configuration = configuration;
    }

    #endregion

    #region public methods
    // GET: /Survey/GetAll
    [HttpGet("GetAll")]
    public async Task<ActionResult<List<SurveyDto>>> GetSurveys(ODataQueryOptions<Survey> queryOptions)
    {
      try
      {
        var surveys = await _surveysService.GetSurveys();
        var surveysList = surveys.AsQueryable();

        var generator = new ODataResponseGenerator<Survey, SurveyDto>();
        var response = generator.GenerateResponseDto(queryOptions, surveysList, _mapper);

        return Ok(response);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }
    // GET: /Survey/GetAll
    [HttpGet("GetAllPublished")]
    public async Task<ActionResult<List<SurveyDto>>> GetSurveysPublished(ODataQueryOptions<Survey> queryOptions)
    {
      try
      {
        var surveys = await _surveysService.GetSurveysPublished();
        var surveysList = surveys.AsQueryable();

        var generator = new ODataResponseGenerator<Survey, SurveyDto>();
        var response = generator.GenerateResponseDto(queryOptions, surveysList, _mapper);

        return Ok(response);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }

    //add survey, add survey's questions and options
    [HttpPost("AddSurveyTotal")]
    public async Task<IActionResult> AddSurveyTotal([FromBody] SurveyDto model)
    {
      if (!ModelState.IsValid)
      {
        _logger.LogError(string.Format("Non valid"));
        return BadRequest(model);
      }
      //add survey, add survey's questions and options
      await using var transaction = await _context.Database.BeginTransactionAsync();
      try
      {
        var survey = _mapper.Map<Survey>(model);
        survey.CreatedOn = DateTime.Now;
        survey.CreatedBy = GetEmailUsername();
        survey.StartDate = null;

        //survey added
        var result = await _surveysService.AddSurvey(survey);

        if (result != null && result.Id != 0)
        {
          if (model.QuestionDtos.Any())
          {
            foreach (var questionDto in model.QuestionDtos)
            {
              questionDto.SurveyId = result.Id;
              var questionModel = _mapper.Map<Question>(questionDto);
              questionModel.CreatedBy = GetEmailUsername();
              questionModel.CreatedOn = DateTime.Now;

              //question added
              var questionServ = await _questionService.AddQuestion(questionModel);

              if (questionServ != null && questionServ.Id != 0)
              {
                await AddOptions(questionServ, questionDto);
              }
            }
          }
          if (model.CommentDtos.Any())
          {
            foreach (var commentDto in model.CommentDtos)
            {
              commentDto.SurveyId = result.Id;
              var commentModel = _mapper.Map<Comment>(commentDto);
              commentModel.CreatedBy = GetEmailUsername();
              commentModel.CreatedOn = DateTime.Now;
              if (commentModel.CommnetText == null || commentModel.CommnetText == "")
              {
                _logger.LogError(string.Format(Messages.INCOMPLETE_DATA, $"Comment text is missing"));
                return BadRequest(new { message = string.Format(Messages.INCOMPLETE_DATA, $"Comment text is missing") });
              }
              else
              {
                //question added
                var commentServ = await _commentService.AddComment(commentModel);
              }

            }
          }
        }
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return Ok(new { survey, message = Messages.CREATED_SUCCESSFULLY });
      }
      catch (Exception ex)
      {
        await transaction.RollbackAsync();
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }

    [HttpPut("UpdateSurveyTotal")]
    public async Task<IActionResult> UpdateSurveyTotal([FromBody] SurveyDto dto)
    {
      if (!ModelState.IsValid)
      {
        _logger.LogError(string.Format("Non valid"));
        return BadRequest(dto);
      }
      await using var transaction = await _context.Database.BeginTransactionAsync();
      try
      {
        //get survey
        var survey = await _surveysService.GetSurveyById(dto.Id);
        if (survey == null)
        {
          _logger.LogError(string.Format("Survey not found"));
          return BadRequest(new { message = "Survey not found" });
        }
        #region survey
        survey.ModifiedOn = DateTime.Now;
        survey.ModifiedBy = GetEmailUsername();
        survey.Id = dto.Id;
        survey.Description = dto.Description;
        survey.SwitchOffRespondents = dto.SwitchOffRespondents;
        survey.VotingRespondents = dto.VotingRespondents;
        survey.EndDate = dto.EndDate;
        survey.Title = dto.Title;
        survey.UserId = dto.UserId;

        //update survey
        var result = await _surveysService.UpdateSurvey(survey);
        #endregion

        if (result != null && result.Id != 0)
        {
          if (dto.QuestionDtos.Any())
          {
            foreach (var questionDto in dto.QuestionDtos)
            {
              Question questionServ = new Question();
              questionDto.SurveyId = result.Id;

              if (questionDto.Id != 0)
              {
                //get question
                var questionData = await _questionService.GetQuestionById(questionDto.Id);

                //add or remove others
                if (questionData.HasOthers != questionDto.HasOthers)
                {
                  if (questionDto.HasOthers)
                  {
                    await AddOther(questionDto.Id);
                  }
                  else
                  {
                    var option = await _surveyQuestionOptionsService.GetOtherSurveyQuestionOptionsByQuestionId(questionDto.Id);
                    if (option != null)
                    {
                      await _surveyQuestionOptionsService.DeleteSurveyQuestionOptions(option);
                    }
                  }
                }
                #region question data
                questionData.ModifiedOn = DateTime.Now;
                questionData.ModifiedBy = GetEmailUsername();
                questionData.DeletedOn = questionData.DeletedOn;
                questionData.QuestionType = questionDto.QuestionType;
                questionData.Description = questionDto.Description;
                questionData.HasOthers = questionDto.HasOthers;
                questionData.IsRequired = questionDto.IsRequired;
                questionData.Order = questionDto.Order;
                questionData.Title = questionDto.Title;
                questionData.SurveyId = questionDto.SurveyId;
                questionData.Id = questionDto.Id;

                //update question
                questionServ = await _questionService.UpdateQuestion(questionData);
                #endregion

                if (questionServ != null && questionServ.Id != 0)
                {
                  foreach (var optionDto in questionDto.SurveyQuestionOptions)
                  {
                    optionDto.QuestionId = questionServ.Id;
                    if (optionDto.Id != 0)
                    {
                      //get options
                      var optionData = await _surveyQuestionOptionsService.GetSurveyQuestionOptionsById(optionDto.Id);
                      #region option data
                      if (questionDto.QuestionType == Enums.QuestionTypeEnum.TextChoice)
                      {
                        optionData.Option = "textarea";
                      }
                      else if (questionDto.QuestionType == Enums.QuestionTypeEnum.ImageChoice)
                      {
                        optionData.Option = !String.IsNullOrEmpty(optionDto.Option) ? _imageService.UploadImage(optionDto.Option) : optionData.Option;
                      }
                      else { optionData.Option = optionDto.Option; }

                      optionData.QuestionId = optionDto.QuestionId;
                      optionData.DeletedOn = optionDto.DeletedOn;
                      optionData.Id = optionDto.Id;
                      await _surveyQuestionOptionsService.UpdateSurveyQuestionOptions(optionData);
                      #endregion
                    }
                    else
                    {
                      if (questionDto.QuestionType == Enums.QuestionTypeEnum.TextChoice)
                      {
                        optionDto.Option = "textarea";
                      }
                      else if (questionDto.QuestionType == Enums.QuestionTypeEnum.ImageChoice)
                      {
                        optionDto.Option = !String.IsNullOrEmpty(optionDto.Option) ? _imageService.UploadImage(optionDto.Option) : optionDto.Option;
                      }
                      var optionData = _mapper.Map<SurveyQuestionOptions>(optionDto);
                      await _surveyQuestionOptionsService.AddSurveyQuestionOptions(optionData);
                    }
                  }

                }
              }
              else
              {
                #region add question
                var questionModel = _mapper.Map<Question>(questionDto);
                questionModel.CreatedBy = GetEmailUsername();
                questionModel.CreatedOn = DateTime.Now;
                questionServ = await _questionService.AddQuestion(questionModel);
                #endregion

                await AddOptions(questionServ, questionDto);
              }
            }
          }
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return Ok(new { message = Messages.UPDATED_SUCCESSFULLY });
      }
      catch (Exception ex)
      {
        await transaction.RollbackAsync();
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }

    // GET: /Survey/GetById/id
    [HttpGet("GetById/{id}")]
    public async Task<ActionResult<SurveyDto>> GetSurveyById(int id)
    {
      try
      {
        var survey = await _surveysService.GetSurveyById(id);
        if (survey == null)
        {
          _logger.LogError(string.Format("Survey not found"));
          return BadRequest(new { message = "Survey not found" });
        }

        var model = _mapper.Map<SurveyDto>(survey);
        return Ok(model);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }

    //includes survey questions and options data
    // GET: /Survey/GetByIdExtended/id
    [AllowAnonymous]
    [HttpGet("GetByExtendedId/{id}")]
    public async Task<ActionResult<SurveyDto>> GetByExtendedId(int id)
    {
      try
      {
        var survey = await _surveysService.GetSurveyByIdExtended(id);
        if (survey == null)
        {
          _logger.LogError(string.Format("Survey not found"));
          return BadRequest(new { message = "Survey not found" });
        }

        var model = _mapper.Map<SurveyDto>(survey);
        return Ok(model);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }

    //GET: /Survey/Add
    [HttpPost("Add")]
    public async Task<IActionResult> AddSurvey([FromBody] SurveyDto model)
    {
      if (!ModelState.IsValid)
      {
        _logger.LogError(string.Format("Non valid"));
        return BadRequest(model);
      }
      try
      {
        var entity = _mapper.Map<Survey>(model);

        entity.CreatedOn = DateTime.Now;
        entity.CreatedBy = GetEmailUsername();
        entity.StartDate = null;
        var result = await _surveysService.AddSurvey(entity);
        if (result != null && result.Id != 0)
        {
          if (model.QuestionDtos.Any())
          {
            foreach (var question in model.QuestionDtos)
            {
              question.SurveyId = entity.Id;
            }
          }
        }
        return Ok(new { entity, message = Messages.CREATED_SUCCESSFULLY });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }

    // PUT: /Survey/Update
    [HttpPut("Update")]
    public async Task<IActionResult> UpdateSurvey([FromBody] SurveyDto dto)
    {
      try
      {
        var survey = await _surveysService.GetSurveyById(dto.Id);
        if (survey == null)
        {
          _logger.LogError(string.Format("Survey not found"));
          return BadRequest(new { message = "Survey not found" });
        }
        if (dto.Id == 0)
        {
          survey.CreatedOn = DateTime.Now;
          survey.CreatedBy = GetEmailUsername();
        }
        else
        {
          survey.ModifiedOn = DateTime.Now;
          survey.ModifiedBy = GetEmailUsername();
          survey.Id = dto.Id;
          survey.Description = dto.Description;
          survey.SwitchOffRespondents = dto.SwitchOffRespondents;
          survey.VotingRespondents = dto.VotingRespondents;
          survey.EndDate = dto.EndDate;
          survey.Title = dto.Title;
          survey.UserId = dto.UserId;
        }
        await _surveysService.UpdateSurvey(survey);

        return Ok(new { message = Messages.UPDATED_SUCCESSFULLY });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }

    // DELETE: /Survey/Delete/id
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteSurvey(int id)
    {
      try
      {
        var survey = await _surveysService.GetSurveyByIdExtended(id);
        if (survey == null)
        {
          _logger.LogError(string.Format("Survey not found"));
          return BadRequest(new { message = "Survey not found" });
        }
        // check if logic delete or physic delete
        if (survey.StartDate.HasValue)
        {
          if (Utils.LogicDelete(survey.StartDate.Value))
          {
            survey.DeletedBy = GetEmailUsername();
            survey.DeletedOn = DateTime.Now;
            await _surveysService.UpdateSurvey(survey);

            foreach (var qu in survey.Questions.Where(i => i.DeletedOn == null))
            {
              qu.DeletedOn = DateTime.Now;
              await _questionService.UpdateQuestion(qu);

              foreach (var option in qu.SurveyQuestionOptions.Where(i => i.DeletedOn == null))
              {
                option.DeletedOn = DateTime.Now;
                await _surveyQuestionOptionsService.UpdateSurveyQuestionOptions(option);
              }
            }
          }
          else
            await _surveysService.DeleteSurvey(survey);
        }
        else
          await _surveysService.DeleteSurvey(survey);

        return Ok(new { message = Messages.DELETED_SUCCESSFULLY });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }

    //publish and send email to all respondents
    // PUT: /Survey/Update
    [HttpPut("Publish")]
    public async Task<IActionResult> PublishSurvey([FromBody] SurveyDto dto)
    {
      try
      {
        var survey = await _surveysService.GetSurveyById(dto.Id);
        if (survey == null)
        {
          _logger.LogError(string.Format("Survey not found"));
          return BadRequest(new { message = "Survey not found" });
        }

        //check if end date is valid and greater than publish date
        if (survey.EndDate > DateTime.Now)
        {
          survey.ModifiedOn = DateTime.Now;
          survey.ModifiedBy = GetEmailUsername();
          survey.StartDate = DateTime.Now;

          // send mail to all
          await SendEmailOrReminder(survey.Title, survey.Id, true);

          //send reminder to all 
          for (int i = -3; i < 0; i++)
          {
            if ((dto.EndDate - survey.StartDate).Value.Days < -i)
            {
              continue;
            }
            BackgroundJob.Schedule<SurveyController>(m => m.SendEmailOrReminder(dto.Title, dto.Id, false), dto.EndDate.AddDays(i));
          }
          await _surveysService.UpdateSurvey(survey);

          return Ok(new { message = Messages.PUBLISHED_SUCCESSFULLY });
        }
        else
        {
          return BadRequest(new { message = "EndDateNotValid" });
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }

    [AllowAnonymous]
    [HttpGet("SwitchOffRespondents")]
    public async Task<IActionResult> SwitchOffRespondents([FromQuery] string token)
    {
      try
      {
        var claims = _generateUniqueLinkService.GetClaims(token);
        string respondentEmail;
        int surveyId;
        if (claims.Any())
        {
          respondentEmail = claims.First(claim => claim.Type == "Email").Value;
          Int32.TryParse(claims.First(claim => claim.Type == "SurveyId").Value, out surveyId);
        }
        else
        {
          return BadRequest(new { message = Messages.INVALID_PAGE });
        }
        var survey = await _surveysService.GetSurveyById(surveyId);

        if (survey == null)
        {
          _logger.LogError(string.Format(Messages.NOT_FOUND));
          return BadRequest(new { message = string.Format(Messages.NOT_FOUND) });
        }

        var respondent = await _respondentService.GetRespondentByEmail(respondentEmail);
        if (respondent == null)
        {
          _logger.LogError(string.Format(Messages.NOT_FOUND));
          return BadRequest(new { message = string.Format(Messages.NOT_FOUND) });
        }
        var switchOffRespondents = survey.SwitchOffRespondents;
        if (!String.IsNullOrEmpty(switchOffRespondents))
        {
          var lista = survey.SwitchOffRespondents.Split(';');
          var alreadyExists = lista?.Any(x => x.ToString() == Convert.ToString(respondent.Id));
          if (alreadyExists.HasValue && !alreadyExists.Value)
            survey.SwitchOffRespondents += ";" + respondent.Id;
        }
        else
        {
          survey.SwitchOffRespondents = string.Empty;
          survey.SwitchOffRespondents += respondent.Id;
        }
        await _surveysService.UpdateSurvey(survey);
        return Ok(new { respondentEmail, survey.Title });

      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }

    [HttpGet("GetRespondentsForASuvey/{id}")]
    public async Task<IActionResult> GetRespondentsForASuvey(int id)
    {
      try
      {
        var listaSurvey = await _surveysService.GetSurveyById(id);

        List<Respondent> respondents = new List<Respondent>();

        var listaRespondents = await _respondentService.GetRespondents();

        var lista = listaSurvey.VotingRespondents?.Split(";");

        respondents = listaRespondents.Where(item => lista.Contains(item.Id.ToString())).ToList();

        var votingRespondents = respondents.Count();
        var allRespondents = listaRespondents.Count();

        return Ok(new { votingRespondents = votingRespondents, allRespondents = allRespondents, surveyTitle = listaSurvey.Title });

      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }

    [HttpGet("GetResponsesFromQuestions/{surveyId}")]
    public async Task<IActionResult> GetResponsesFromQuestions(int surveyId)
    {
      try
      {
        var model = new SurveyAnalitsicsDto
        {
          Questions = new List<QuestionAnalytics>()
        };
        var survey = await _surveysService.GetSurveyByIdExtendedTotal(surveyId);
        if (survey == null)
        {
          return BadRequest("SurveyNull");
        }
        var respondents = await _respondentService.GetRespondents();
        if (respondents == null)
        {
          return BadRequest("RespondentsNull");

        }
        model.SurveyTitle = survey.Title;
        model.SurveyRespondents = survey.VotingRespondents;
        model.Respondents = respondents.Count();

        foreach (var question in survey.Questions)
        {
          var questionDto = new QuestionAnalytics
          {
            QuestionId = question.Id,
            QuestionTitle = question.Title,
            QuestionType = question.QuestionType,
            HasOthers = question.HasOthers,
            Options = new List<OptionsAnalytics>(),
          };
          var totalVotersForOption = 0;
          foreach (var option in question.SurveyQuestionOptions)
          {
            var tempCounter = option.Responses?.Count();

            var responses = question.QuestionType == Enums.QuestionTypeEnum.NumberChoice ?
                option.Responses.GroupBy(r => r.Other).ToDictionary(g => g.Key, g => g.Count()) :
                null;

            var optionDto = new OptionsAnalytics
            {
              OptionId = option.Id,
              OptionVoters = tempCounter.HasValue ? tempCounter.Value : 0,
              Option = option.Option,
              Responses = responses
            };
            totalVotersForOption += tempCounter.HasValue ? tempCounter.Value : 0;
            questionDto.Options.Add(optionDto);
          }
          questionDto.TotalVoters = totalVotersForOption;
          model.Questions.Add(questionDto);
        }
        return Ok(model);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }

    [HttpGet("SendEmailOrReminder")]
    public async Task SendEmailOrReminder(string title, int id, bool publishNotReminder = false)
    {
      try
      {
        var surveyEl = await _surveysService.GetSurveyById(id);
        var surveyDto = _mapper.Map<SurveyDto>(surveyEl);
        var listaRespondents = await _respondentService.GetRespondents();
        if (surveyDto != null)
        {
          //is not a publish 
          if (!publishNotReminder)
          {
            List<Respondent> notToSendEmailUsers = new List<Respondent>();
            var listOfSwitchOff = surveyDto.SwitchOffRespondents?.Split(";").ToList();
            var listOfHasVoted = surveyDto.VotingRespondents?.Split(";").ToList();
            foreach (var respondent in listaRespondents)
            {
              if (!(listOfSwitchOff?.Contains(respondent.Id.ToString()) == true) && !(listOfHasVoted?.Contains(respondent.Id.ToString()) == true))
              {
                var reminderLink = _generateUniqueLinkService.GenerateUniqueLink(respondent.Email, id, _configuration.GetValue<string>("ApplicationsUrl:UrlQuiz"), surveyEl.EndDate);
                var switchOffLink = _generateUniqueLinkService.GenerateUniqueLink(respondent.Email, id, _configuration.GetValue<string>("ApplicationsUrl:UrlThankYou"), surveyEl.EndDate);
                await _emailService.SendEmail(respondent.Email, string.Format(Messages.OBJECT_EMAIL_REMINDER_SURVEY, title),
                    string.Format(Messages.BODY_EMAIL_REMINDER_SURVEY, reminderLink, switchOffLink));
              }
            }
          }
          //is publish
          else
          {
            foreach (var respondent in listaRespondents)
            {
              var generatedLink = _generateUniqueLinkService.GenerateUniqueLink(respondent.Email, id, _configuration.GetValue<string>("ApplicationsUrl:UrlQuiz"), surveyEl.EndDate);
              await _emailService.SendEmail(respondent.Email, string.Format(Messages.OBJECT_EMAIL_NEW_SURVEY, title), string.Format(Messages.BODY_EMAIL_NEW_SURVEY, generatedLink));
            }
          }
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }
    #endregion

    #region private methods
    private async Task AddOrUpdateOption(SurveyQuestionOptions option)
    {
      if (option.Id != 0)
      {
        await _surveyQuestionOptionsService.UpdateSurveyQuestionOptions(option);
      }
      else
      {
        await _surveyQuestionOptionsService.AddSurveyQuestionOptions(option);
      }

    }
    private async Task AddOther(int questionId)
    {
      var optionOther = new SurveyQuestionOptions();
      optionOther.QuestionId = questionId;
      optionOther.Option = "hasOtherOption2929";
      await _surveyQuestionOptionsService.AddSurveyQuestionOptions(optionOther);
    }
    private async Task AddOptions(Question question, QuestionDto questionDto)
    {
      foreach (var option in question.SurveyQuestionOptions)
      {
        option.QuestionId = question.Id;
        //if it's textarea

        if (questionDto.QuestionType == Enums.QuestionTypeEnum.TextChoice)
        {
          option.Option = "textarea";
        }
        //if it's image

        else if (questionDto.QuestionType == Enums.QuestionTypeEnum.ImageChoice)
        {
          option.Option = !String.IsNullOrEmpty(option.Option) ? _imageService.UploadImage(option.Option)
                      : "";
        }
        await AddOrUpdateOption(option);

      }
      if (questionDto.HasOthers)
      {
        await AddOther(question.Id);
      }
    }

    #endregion
  }
}

