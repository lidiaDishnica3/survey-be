using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InternalSurvey.Api.Dtos;
using InternalSurvey.Api.Entities;
using InternalSurvey.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using InternalSurvey.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using InternalSurvey.Api.Controllers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InternalQuestion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionController : BaseController<QuestionController>
    {
        private readonly IQuestionsService _questionsService;
        private readonly ISurveyQuestionOptionsService _surveyQuestionOptionsService;
        private readonly ILogger<QuestionController> _logger;
        private readonly IMapper _mapper;
        public QuestionController(ILogger<QuestionController> logger, IMapper mapper, IQuestionsService QuestionsService,
            ISurveyQuestionOptionsService surveyQuestionOptionsService) : base(logger)
        {
            _logger = logger;
            _mapper = mapper;
            _questionsService = QuestionsService;
            _surveyQuestionOptionsService = surveyQuestionOptionsService;
        }

        // GET: /Question/GetAll
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<QuestionDto>>> GetQuestions()
        {
            try
            {
                var questionList = await _questionsService.GetQuestions();
                var model = _mapper.Map<List<QuestionDto>>(questionList);

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }

        // GET: /Question/GetById/id
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<QuestionDto>> GetQuestionById(int id)
        {
            try
            {
                var question = await _questionsService.GetQuestionById(id);
                if (question == null)
                {
                    _logger.LogError(string.Format(Messages.NOT_FOUND, $"Question id: {id}"));
                    return BadRequest(new { message = string.Format(Messages.NOT_FOUND, $"Question with id: {id}") });
                }

                var model = _mapper.Map<QuestionDto>(question);
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }

        // GET: /Question/Add
        [HttpPost("Add")]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(string.Format("Non valid"));
                return BadRequest(model);
            }
            try
            {
                var entity = _mapper.Map<Question>(model);
                entity.CreatedOn = DateTime.Now;
                entity.CreatedBy = GetEmailUsername();

                var result = await _questionsService.AddQuestion(entity);
                if (result != null && result.Id != 0)
                {
                    if (model.SurveyQuestionOptions.Any())
                    {
                        foreach (var option in model.SurveyQuestionOptions)
                        {
                            option.QuestionId = entity.Id;
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

        // PUT: /Question/Update
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateQuestion([FromBody] QuestionDto dto)
        {
            try
            {
                var question = await _questionsService.GetQuestionById(dto.Id);
                if (question == null)
                {
                    _logger.LogError(string.Format(Messages.NOT_FOUND, $"Question id: {dto.Id}"));
                    return BadRequest(new { message = string.Format(Messages.NOT_FOUND, $"Question with id: {dto.Id}") });
                }

                question.Id = dto.Id;
                question.Title = dto.Title;
                question.IsRequired = dto.IsRequired;
                if (question.HasOthers != dto.HasOthers)
                {
                    if (dto.HasOthers)
                    {
                        SurveyQuestionOptions optionsDto = new SurveyQuestionOptions();
                        optionsDto.QuestionId = dto.Id;
                        optionsDto.Option = "hasOtherOption2929";
                        await _surveyQuestionOptionsService.AddSurveyQuestionOptions(optionsDto);
                    }
                    else
                    {
                        var option = await _surveyQuestionOptionsService.GetOtherSurveyQuestionOptionsByQuestionId(dto.Id);
                        if (option != null)
                        {
                            await _surveyQuestionOptionsService.DeleteSurveyQuestionOptions(option);
                        }

                    }
                }
                question.HasOthers = dto.HasOthers;
                question.Description = dto.Description;
                question.SurveyId = dto.SurveyId;
                question.Order = dto.Order;
                question.ModifiedOn = DateTime.Now;
                question.ModifiedBy = GetEmailUsername();
                question.DeletedOn = dto.DeletedOn;
                question.QuestionType = dto.QuestionType;
                await _questionsService.UpdateQuestion(question);

                return Ok(new { question, message = Messages.UPDATED_SUCCESSFULLY });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }

        // DELETE: /Question/Delete/id
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            try
            {
                var question = await _questionsService.GetQuestionById(id);
                if (question == null)
                {
                    _logger.LogError(string.Format(Messages.NOT_FOUND, $"Question id: {id}"));
                    return BadRequest(new { message = string.Format(Messages.NOT_FOUND, $"Question with id: {id}") });
                }
                await _questionsService.DeleteQuestion(question);

                return Ok(new { message = string.Format(Messages.DELETED_SUCCESSFULLY, "question") });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }
    }
}
