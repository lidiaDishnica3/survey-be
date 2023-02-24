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
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Logging;

namespace InternalSurvey.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RespondentController : BaseController<RespondentController>
    {
        private readonly IRespondentService _respondentService;
        private readonly IResponseService _responseService;
        private readonly IMapper _mapper;
        private readonly ILogger<SurveyController> _logger;
        private readonly IGenerateUniqueLinkService _generateUniqueLinkService;

        public RespondentController(IRespondentService respondentService, IResponseService responseService, IMapper mapper, ILogger<SurveyController> logger,
            IGenerateUniqueLinkService generateUniqueLinkService) : base(logger)
        {
            _respondentService = respondentService;
            _responseService = responseService;
            _mapper = mapper;
            _logger = logger;
            _generateUniqueLinkService = generateUniqueLinkService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<RespondentDto>>> GetRespondents(ODataQueryOptions<Respondent> queryOptions)
        {
            try
            {
                var respondents = await _respondentService.GetRespondents();
                var respondentsList = respondents.AsQueryable();
                // is not has respondets bad request

                //var respondents = await _respondentService.GetRespondents();
                //var respondentsList = respondents.AsQueryable();

                if (!respondents.Any())
                {
                    _logger.LogError(Messages.NO_DATA);
                    return BadRequest(new { message = string.Format(Messages.NO_DATA) });
                }

                var generator = new ODataResponseGenerator<Respondent, RespondentDto>();
                var response = generator.GenerateResponseDto(queryOptions, respondentsList, _mapper);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }


        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<RespondentDto>> GetRespondentById(int id)
        {
            try
            {
                var respondent = await _respondentService.GetRespondentById(id);
                var responsesList = await _responseService.GetResponses();
                var responses = responsesList.AsQueryable().Where(res => res.RespondentId == id).ToList();

                if (respondent == null)
                {
                    _logger.LogError(string.Format(Messages.NOT_FOUND, $"Respondent id: {id}"));
                    return BadRequest(new { message = string.Format(Messages.NOT_FOUND, $"Respondent with id: {id}") });
                }

                respondent.Responses = responses;
                var model = _mapper.Map<RespondentDto>(respondent);

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }


        [HttpPost("Add")]
        public async Task<IActionResult> AddRespondent([FromBody] RespondentDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(string.Format("Non valid"));
                return BadRequest(model);
            }
            try
            {
                var entity = _mapper.Map<Respondent>(model);
                if (model.Id == 0)
                {
                    entity.CreatedOn = DateTime.Now;
                    entity.CreatedBy = GetEmailUsername();
                }
                await _respondentService.AddRespondent(entity);
                await _respondentService.SaveChanges();

                return Ok(new { message = Messages.CREATED_SUCCESSFULLY });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateRespondent([FromBody] RespondentDto model)
        {
            try
            {
                var respondent = await _respondentService.GetRespondentById(model.Id);
                if (respondent == null)
                {
                    _logger.LogError(string.Format(Messages.NOT_FOUND, $"Respondent id: {model.Id}"));
                    return BadRequest(new { message = string.Format(Messages.NOT_FOUND, $"Respondent with id: {model.Id}") });
                }

                if (model.Id == 0)
                {
                    respondent.CreatedOn = DateTime.Now;
                    respondent.CreatedBy = GetEmailUsername();
                }
                else
                {
                    respondent.Id = model.Id;
                    respondent.Email = model.Email;

                    respondent.ModifiedOn = DateTime.Now;
                    respondent.ModifiedBy = GetEmailUsername();
                }

                await _respondentService.UpdateRespondent(respondent);

                return Ok(new { message = Messages.UPDATED_SUCCESSFULLY });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }


        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteRespondent(int id)
        {
            try
            {
                var respondent = await _respondentService.GetRespondentById(id);

                if (respondent == null)
                {
                    _logger.LogError(string.Format(Messages.NOT_FOUND, $"Respondent id: {id}"));
                    return BadRequest(new { message = string.Format(Messages.NOT_FOUND, $"Respondent with id: {id}") });
                }

                //if physic delete
                //await _respondentService.DeleteRespondent(respondent);
                //if logic delete
                respondent.DeletedBy = GetEmailUsername();
                respondent.DeletedOn = DateTime.Now;
                await _respondentService.UpdateRespondent(respondent);

                return Ok(new { message = string.Format(Messages.DELETED_SUCCESSFULLY, "respondent") });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }

    }
}
