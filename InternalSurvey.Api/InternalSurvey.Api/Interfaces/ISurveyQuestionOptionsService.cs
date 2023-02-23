using InternalSurvey.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Interfaces
{
    public interface ISurveyQuestionOptionsService
    {
        Task<IEnumerable<SurveyQuestionOptions>> GetSurveyQuestionOptions();
        Task<SurveyQuestionOptions> GetSurveyQuestionOptionsById(int id);
        Task<SurveyQuestionOptions> AddSurveyQuestionOptions(SurveyQuestionOptions surveyQuestionOptions);
        Task UpdateSurveyQuestionOptions(SurveyQuestionOptions surveyQuestionOptions);
        Task DeleteSurveyQuestionOptions(SurveyQuestionOptions surveyQuestionOptions);
        Task<SurveyQuestionOptions> GetOtherSurveyQuestionOptionsByQuestionId(int questionId);
        List<SurveyQuestionOptions> GetQuestionInSurveyQuestion(int id);

    }
}
