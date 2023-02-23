using InternalSurvey.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace InternalSurvey.Api.Interfaces
{
    public interface ISurveyService
    {
         Task<IEnumerable<Survey>> GetSurveys();
         Task<IEnumerable<Survey>> GetSurveysPublished();
        Task<Survey> GetSurveyById(int id);
        Task<Survey> GetSurveyByIdExtended(int id);
        Task<Survey> AddSurvey(Survey survey);
        Task<Survey> UpdateSurvey(Survey survey);
        Task DeleteSurvey(Survey survey);
        Task<Survey> GetSurveyByIdExtendedTotal(int id);
    }
}
