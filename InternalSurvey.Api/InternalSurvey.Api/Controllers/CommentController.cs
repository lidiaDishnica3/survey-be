using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InternalSurvey.Api.Dtos;
using InternalSurvey.Api.Entities;
using InternalSurvey.Api.Helpers;
using InternalSurvey.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace InternalSurvey.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CommentController : BaseController<CommentController>
  {
    private readonly ICommentService _commentService;
    private readonly IMapper _mapper;
    private readonly ILogger<CommentController> _logger;
    private readonly ISurveySubmissionService _surveySubmissionService;
    private readonly ISurveyService _surveyService;
    private readonly IRespondentService _respondentService;

    public CommentController(ICommentService commentService, IMapper mapper, 
      ILogger<CommentController> logger, ISurveySubmissionService surveySubmissionService,
      IRespondentService respondentService, ISurveyService surveyService) : base(logger)
    {
      _commentService = commentService;
      _mapper = mapper;
      _logger = logger;
      _surveySubmissionService = surveySubmissionService;
      _respondentService = respondentService;
      _surveyService = surveyService;
    }

    [HttpGet("GetCommentBySurveyId/{SurveyId}")]
    public async Task<ActionResult<List<CommentDto>>> GetCommentsBySurveyId(int SurveyId)
    {
      try
      {
        var commentList = await _commentService.GetCommentsBySurveyId(SurveyId);
        var model = _mapper.Map<List<CommentDto>>(commentList);

        return Ok(model);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }


    [HttpGet("GetById/{id}")]
    public async Task<ActionResult<CommentDto>> GetCommentById(int id)
    {
      try
      {
        var comment = await _commentService.GetCommentById(id);
        if (comment == null)
        {
          _logger.LogError(string.Format(Messages.NOT_FOUND, $"Comment id: {id}"));
          return BadRequest(new { message = string.Format(Messages.NOT_FOUND, $"Comment with id: {id}") });
        }

        var model = _mapper.Map<CommentDto>(comment);
        return Ok(model);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }


    [HttpPost("Add")]
    public async Task<IActionResult> AddComment([FromQuery] string token, [FromBody] CommentDto model)
    {
      if (!ModelState.IsValid)
      {
        _logger.LogError(string.Format("Non valid"));
        return BadRequest(model);
      }
      try
      {
        var respondentTokenData = await _surveySubmissionService.GetRespondentTokenData(token);
        var respondent = await _respondentService.GetRespondentByEmail(respondentTokenData.Email);
        
        var entity = _mapper.Map<Comment>(model);
        entity.CreatedOn = DateTime.Now;
        entity.CreatedBy = GetEmailUsername();
        entity.RespondentId = respondent.Id;
        entity.SurveyId = respondentTokenData.Survey.Id;

        if (entity.CommnetText == null || entity.CommnetText == "")
        {
          _logger.LogError(string.Format(Messages.INCOMPLETE_DATA, $"Comment text is missing"));
          return BadRequest(new { message = string.Format(Messages.INCOMPLETE_DATA, $"Comment text is missing") });
        }

        var result = await _commentService.AddComment(entity);

        return Ok(new { entity, message = Messages.CREATED_SUCCESSFULLY });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }



    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
      try
      {
        var comment = await _commentService.GetCommentById(id);
        if (comment == null)
        {
          _logger.LogError(string.Format(Messages.NOT_FOUND, $"Comment id: {id}"));
          return BadRequest(new { message = string.Format(Messages.NOT_FOUND, $"Comment with id: {id}") });
        }
        var survey = await _surveyService.GetSurveyById(comment.SurveyId);
        if (survey.CreatedBy == GetEmailUsername())
        {
          comment.DeletedOn = DateTime.Now;
          await _commentService.DeleteComment(comment);
          return Ok(new { message = string.Format(Messages.DELETED_SUCCESSFULLY, "comment") });
        }
        else
        {
          return BadRequest(new { message = string.Format(Messages.NOT_ALLOWED_OPERATION)});
        }

      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
      }
    }
  }
}