using System;
using System.Threading.Tasks;
using AutoMapper;
using InternalSurvey.Api.Dtos;
using InternalSurvey.Api.Exceptions;
using InternalSurvey.Api.Helpers;
using InternalSurvey.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InternalSurvey.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SurveySubmissionController : Controller
    {
        private readonly ILogger<SurveySubmissionController> _logger;
        private readonly IMapper _mapper;
        private readonly ISurveySubmissionService _surveySubmissionService;

        public SurveySubmissionController(ILogger<SurveySubmissionController> logger, IMapper mapper, ISurveySubmissionService surveySubmissionService)
        {
            _logger = logger;
            _mapper = mapper;
            _surveySubmissionService = surveySubmissionService;
        }

        // POST: /SurveySubmission/Submit
        [AllowAnonymous]
        [HttpGet("GetSurveyDataForRespondent")]
        public async Task<IActionResult> GetSurveyDataForRespondent([FromQuery] string token)
        {
            try
            {
                var respondentTokenData = await _surveySubmissionService.GetRespondentTokenData(token);
                var respondentTokenDto = _mapper.Map<RespondentTokenDto>(respondentTokenData);
                return Ok(respondentTokenDto);
            }
            catch (InvalidTokenException ite)
            {
                return BadRequest(ite.GetInvalidProperty());
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Messages.UNEXPECTED_ERROR));
                throw ex;
            }
        }

        // POST: /SurveySubmission/Submit
        [AllowAnonymous]
        [HttpPost("Submit")]
        public async Task<IActionResult> SubmitSurvey([FromQuery] string token, [FromBody] SurveySubmissionDto submission)
        {
            try
            {
                var respondentTokenData = await _surveySubmissionService.GetRespondentTokenData(token);
                await _surveySubmissionService.SubmitSurvey(submission);
                return Ok(new { message = "Survey submitted successfully!" });
            }
            catch (InvalidTokenException ite)
            {
                return BadRequest(ite.GetInvalidProperty());
            }
            catch (UnansweredQuestionException uqe)
            {
                return BadRequest(uqe.GetInvalidProperty());
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Messages.UNEXPECTED_ERROR));
                throw ex;
            }
        }
    }
}
