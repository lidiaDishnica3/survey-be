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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InternalSurvey.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SurveyQuestionOptionsController : ControllerBase
    {

        private readonly ISurveyQuestionOptionsService _surveyQuestionOptionsService;
        private readonly IQuestionsService _questionsService;
        private readonly IImageService _imageService;
        private readonly ILogger<SurveyQuestionOptionsController> _logger;
        private readonly IMapper _mapper;

        public SurveyQuestionOptionsController(ILogger<SurveyQuestionOptionsController> logger, IMapper mapper,
            ISurveyQuestionOptionsService surveyQuestionOptionsService, IQuestionsService questionsService, IImageService imageService)
        {
            _logger = logger;
            _mapper = mapper;
            _surveyQuestionOptionsService = surveyQuestionOptionsService;
            _questionsService = questionsService;
            _imageService = imageService;
        }

        // GET: /SurveyQuestionOptions/GetAll
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<SurveyQuestionOptionsDto>>> GetSurveyQuestionOptions()
        {
            try
            {
                var surveyquestionOptionsList = await _surveyQuestionOptionsService.GetSurveyQuestionOptions();
                var model = _mapper.Map<List<SurveyQuestionOptionsDto>>(surveyquestionOptionsList);

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }

        // GET: /SurveyQuestionOptions/GetById/id
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<SurveyQuestionOptionsDto>> GetSurveyQuestionOptionsById(int id)
        {
            try
            {
                var surveyQuestionOptions = await _surveyQuestionOptionsService.GetSurveyQuestionOptionsById(id);
                if (surveyQuestionOptions == null)
                {
                    _logger.LogError(string.Format("SurveyQuestionOptions not found"));
                    return BadRequest(new { message = "SurveyQuestionOptions not found" });
                }

                var model = _mapper.Map<SurveyQuestionOptionsDto>(surveyQuestionOptions);
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }

        // GET: /SurveyQuestionOptions/Add
        [HttpPost("Add")]
        public async Task<IActionResult> AddSurveyQuestionOptions([FromBody] SurveyQuestionOptionsDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(string.Format("Non valid"));
                return BadRequest(model);
            }
            try
            {
                var entity = _mapper.Map<SurveyQuestionOptions>(model);
                var question = await _questionsService.GetQuestionById(model.QuestionId);
                if (question.QuestionType == Enums.QuestionTypeEnum.ImageChoice)
                    entity.Option = !String.IsNullOrEmpty(model.Option) ? _imageService.UploadImage(model.Option)
                            : "";

                await _surveyQuestionOptionsService.AddSurveyQuestionOptions(entity);

                return Ok(new { message = Messages.CREATED_SUCCESSFULLY });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }


        // PUT: /SurveyQuestionOptions/Update
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateSurveyQuestionOptions([FromBody] SurveyQuestionOptionsDto dto)
        {
            try
            {
                var surveyQuestionOptions = await _surveyQuestionOptionsService.GetSurveyQuestionOptionsById(dto.Id);
                if (surveyQuestionOptions == null)
                {
                    _logger.LogError(string.Format("SurveyQuestionOptions not found"));
                    return BadRequest(new { message = "SurveyQuestionOptions not found" });
                }
                var question = await _questionsService.GetQuestionById(dto.QuestionId);
                surveyQuestionOptions.Id = dto.Id;
                if (question.QuestionType == Enums.QuestionTypeEnum.ImageChoice)
                    surveyQuestionOptions.Option = !String.IsNullOrEmpty(dto.Option) ? _imageService.UploadImage(dto.Option) : surveyQuestionOptions.Option;
                else
                    surveyQuestionOptions.Option = dto.Option;

                surveyQuestionOptions.DeletedOn = dto.DeletedOn;
                surveyQuestionOptions.QuestionId = dto.QuestionId;



                await _surveyQuestionOptionsService.UpdateSurveyQuestionOptions(surveyQuestionOptions);

                return Ok(new { message = Messages.UPDATED_SUCCESSFULLY });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }

        // DELETE: /SurveyQuestionOptions/Delete/id
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteSurveyQuestionOptions(int id)
        {
            try
            {
                var surveyQuestionOptions = await _surveyQuestionOptionsService.GetSurveyQuestionOptionsById(id);
                if (surveyQuestionOptions == null)
                {
                    _logger.LogError(string.Format("SurveyQuestionOptions not found"));
                    return BadRequest(new { message = "SurveyQuestionOptions not found" });
                }              
                var question = await _questionsService.GetQuestionById(surveyQuestionOptions.QuestionId);
                if (question.QuestionType == Enums.QuestionTypeEnum.ImageChoice)
                {
                    if (System.IO.File.Exists(_imageService.GetImageLocation(surveyQuestionOptions.Option)))
                    {
                        System.IO.File.Delete(_imageService.GetImageLocation(surveyQuestionOptions.Option));
                    }
                }
                await _surveyQuestionOptionsService.DeleteSurveyQuestionOptions(surveyQuestionOptions);
                return Ok(new { message = Messages.DELETED_SUCCESSFULLY });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }

        [HttpGet("GetImageById/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<SurveyQuestionOptionsDto>> GetImage(int id)
        {
            if (id != 0)
            {
                var option = await _surveyQuestionOptionsService.GetSurveyQuestionOptionsById(id);
                option.Option = _imageService.GetImage(option.Option);
                var model = _mapper.Map<SurveyQuestionOptionsDto>(option);
                return Ok(model);
            }
            return null;
        }
    }
}
