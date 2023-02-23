using InternalSurvey.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Interfaces
{
    public interface IQuestionsService
    {
        Task<Question> AddQuestion(Question Question);
        Task DeleteQuestion(Question Question);
        Task<Question> GetQuestionById(int id);
        Task<IEnumerable<Question>> GetQuestions();
        Task SaveChanges();
        Task<Question> UpdateQuestion(Question Question);

        List<Question> GetSurveyIdInQuestion(int id);
    }
}
