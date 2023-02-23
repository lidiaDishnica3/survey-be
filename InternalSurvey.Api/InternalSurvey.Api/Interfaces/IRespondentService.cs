using InternalSurvey.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Interfaces
{
    public interface IRespondentService
    {
        Task AddRespondent(Respondent respondent);
        Task DeleteRespondent(Respondent respondent);
        Task<Respondent> GetRespondentById(int id);
        Task<Respondent> GetRespondentByEmail(string email);
        Task<List<Respondent>> GetRespondents();
        Task SaveChanges();
        Task UpdateRespondent(Respondent respondent);
        Task<bool?> HasVoted(string email, string respondentIds);
    }
}
