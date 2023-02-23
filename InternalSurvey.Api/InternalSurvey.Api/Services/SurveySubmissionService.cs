using InternalSurvey.Api.Data;
using InternalSurvey.Api.Dtos;
using InternalSurvey.Api.Entities;
using InternalSurvey.Api.Enums;
using InternalSurvey.Api.Exceptions;
using InternalSurvey.Api.Helpers;
using InternalSurvey.Api.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Services
{
    public class SurveySubmissionService : ISurveySubmissionService
    {
        private readonly ILogger<SurveySubmissionService> _logger;
        private readonly SurveyAppContext _context;
        private readonly ISurveyService _surveyService;
        private readonly IResponseService _responseService;
        private readonly IRespondentService _respondentService;
        private readonly IGenerateUniqueLinkService _generateUniqueLinkService;

        public SurveySubmissionService(ILogger<SurveySubmissionService> logger, SurveyAppContext context, ISurveyService surveyService, IResponseService responseService, IRespondentService respondentService, IGenerateUniqueLinkService uniqueLinkService)
        {
            _logger = logger;
            _context = context;
            _surveyService = surveyService;
            _responseService = responseService;
            _respondentService = respondentService;
            _generateUniqueLinkService = uniqueLinkService;
        }
        public async Task SubmitSurvey(SurveySubmissionDto surveyResponses)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var survey = await _surveyService.GetSurveyByIdExtended(surveyResponses.SurveyId);
                var respondent = await _respondentService.GetRespondentByEmail(surveyResponses.RespondentEmail);
                int respondentId = respondent.Id;
                int questionOrder = 1;
                foreach (var answer in surveyResponses.Answers)
                {
                    await SaveResponseBasedOnQuestionType(questionOrder, survey, respondentId, answer);
                    questionOrder++;
                }
                survey.VotingRespondents += !string.IsNullOrWhiteSpace(survey.VotingRespondents) ? ";" + respondentId.ToString() : respondentId.ToString();
                await _surveyService.UpdateSurvey(survey);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (UnansweredQuestionException uqe)
            {
                await transaction.RollbackAsync();
                throw uqe;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.TRANSACTION_FAILED);
                await transaction.RollbackAsync();
                throw ex;
            }
        }

        public async Task<RespondentTokenData> GetRespondentTokenData(string token)
        {
            var claims = _generateUniqueLinkService.GetClaims(token);
            string respondentEmail;
            int surveyId;
            bool? respondentHasVoted;
            if (claims.Any())
            {
                respondentEmail = claims.First(claim => claim.Type == "Email").Value;
                Int32.TryParse(claims.First(claim => claim.Type == "SurveyId").Value, out surveyId);
            }
            else
            {
                throw new InvalidTokenException() { InvalidToken = true };
            }
            var survey = await _surveyService.GetSurveyByIdExtended(surveyId);
            if (survey == null)
            {
                throw new InvalidTokenException() { InvalidSurvey = true };
            }
            bool surveyHasExpired = Utils.HasExpired(survey.EndDate);
            if (surveyHasExpired)
            {
                throw new InvalidTokenException() { SurveyHasExpired = true };
            }
            respondentHasVoted = await _respondentService.HasVoted(respondentEmail, survey.VotingRespondents);
            if (respondentHasVoted == null)
            {
                throw new InvalidTokenException() { InvalidEmail = true };
            }
            if (respondentHasVoted.Value == true)
            {
                throw new InvalidTokenException() { HasVoted = true, Email = respondentEmail };
            }
            return new RespondentTokenData()
            {
                Email = respondentEmail,
                Survey = survey
            };
        }

        private async Task SaveResponseBasedOnQuestionType(int questionOrder, Survey survey, int respondentId, SubmittedAnswerDto submittedAnswers)
        {
            try
            {
                var question = survey.Questions.FirstOrDefault(q => q.Id == submittedAnswers.QuestionId);
                var questionType = question.QuestionType;
                bool answerIsEmpty = submittedAnswers.IsEmpty(question);
                if (answerIsEmpty)
                {
                    if (question.IsRequired)
                    {
                        //In textbox you can not force a user to insert it.
                        if (question.QuestionType == QuestionTypeEnum.TextChoice)
                        {
                            return;
                        }
                        throw new UnansweredQuestionException() { UnansweredQuestionOrder = questionOrder };
                    }
                    else
                    {
                        return;
                    }
                }
                if (questionType == QuestionTypeEnum.RadioButtons || questionType == QuestionTypeEnum.TextChoice
                    || questionType == QuestionTypeEnum.ImageChoice || questionType == QuestionTypeEnum.NumberChoice)
                {
                    await SaveSingleReponse(respondentId, submittedAnswers);
                }
                else if (questionType == QuestionTypeEnum.CheckBox)
                {
                    //Respondent has checked at least one checkbox
                    if (submittedAnswers.QuestionOptionId == 0)
                    {
                        await SaveMultipleReponses(respondentId, submittedAnswers);
                    }
                    else
                    {
                        //Respondent chose other option
                        await SaveSingleReponse(respondentId, submittedAnswers);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        private async Task SaveSingleReponse(int respondentId, SubmittedAnswerDto submittedAnswers)
        {
            var response = new Response()
            {
                RespondentId = respondentId,
                SurveyQuestionOptionsId = submittedAnswers.QuestionOptionId,
                Other = submittedAnswers.Other,
            };
            await _responseService.AddResponse(response);
        }

        private async Task SaveMultipleReponses(int respondentId, SubmittedAnswerDto submittedAnswers)
        {
            var responses = new List<Response>();
            var validOptionIds = submittedAnswers.QuestionOptionIds.Where(id => id != 0);
            foreach (int optionId in validOptionIds)
            {
                responses.Add(
                    new Response()
                    {
                        RespondentId = respondentId,
                        SurveyQuestionOptionsId = optionId,
                        Other = submittedAnswers.Other,
                    }
                 );
            }
            await _responseService.AddResponses(responses);
        }
    }
}