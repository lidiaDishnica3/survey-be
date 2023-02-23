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
    [Authorize]
    public class ResponseController : ControllerBase
    {
        private readonly IResponseService _responseService;
        private readonly IMapper _mapper;
        private readonly ILogger<Response> _logger;

        public ResponseController(IResponseService responseService, IMapper mapper, ILogger<Response> logger)
        {
            _responseService = responseService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Response>>> GetResponses()
        {
            try
            {
                var response = await _responseService.GetResponses();
                if (!response.Any())
                {
                    _logger.LogError(Messages.NO_DATA);
                    return BadRequest(new { message = string.Format(Messages.NO_DATA) });
                }

                var model = _mapper.Map<List<ResponseDto>>(response);
                return Ok(model.OrderByDescending(x => x.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }


        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ResponseDto>> GetResponseById(int id)
        {
            try
            {
                var response = await _responseService.GetResponseById(id);
                if (response == null)
                {
                    _logger.LogError(string.Format(Messages.NOT_FOUND, $"Response id: {id}"));
                    return BadRequest(new { message = string.Format(Messages.NOT_FOUND, $"Response with id: {id}") });
                }

                var model = _mapper.Map<ResponseDto>(response);
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }


        [HttpPost("Add")]
        public async Task<IActionResult> AddResponse([FromBody] ResponseDto model)
        {

            if (!ModelState.IsValid)
            {
                _logger.LogError(string.Format("Non valid"));
                return BadRequest(model);
            }
            try
            {
                var entity = _mapper.Map<Response>(model);
                await _responseService.AddResponse(entity);

                return Ok(new { message = Messages.CREATED_SUCCESSFULLY });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateResponse([FromBody] ResponseDto model)
        {
            try
            {
                var response = await _responseService.GetResponseById(model.Id);
                if (response == null)
                {
                    _logger.LogError(string.Format(Messages.NOT_FOUND, $"Response id: {model.Id}"));
                    return BadRequest(new { message = string.Format(Messages.NOT_FOUND, $"Response with id: {model.Id}") });
                }

                response.Id = model.Id;
                response.RespondentId = model.RespondentId;
                response.SurveyQuestionOptionsId = model.SurveyQuestionOptionsId;
                response.Other = model.Other;
                response.DeletedOn = model.DeletedOn;

                await _responseService.UpdateResponse(response);

                return Ok(new { message = Messages.UPDATED_SUCCESSFULLY });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }


        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteResponse(int id)
        {
            try
            {
                var response = await _responseService.GetResponseById(id);
                if (response == null)
                {
                    _logger.LogError(string.Format(Messages.NOT_FOUND, $"Response id: {id}"));
                    return BadRequest(new { message = string.Format(Messages.NOT_FOUND, $"Response with id: {id}") });
                }
                //if physic delete
                //await _responseService.DeleteResponse(response);
                //if logic delete
                response.DeletedOn = DateTime.Now;
                await _responseService.UpdateResponse(response);

                return Ok(new { message = string.Format(Messages.DELETED_SUCCESSFULLY, "response") });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }
    }
}
