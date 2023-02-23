using InternalSurvey.Api.Dtos;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Interfaces
{
    public interface ISurveySubmissionService
    {
        Task SubmitSurvey(SurveySubmissionDto surveyResponses);
        Task<RespondentTokenData> GetRespondentTokenData(string token);
    }
}
