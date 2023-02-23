using InternalSurvey.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Interfaces
{
    public interface IResponseService
    {
        Task<IEnumerable<Response>> GetResponses();

        Task<Response> GetResponseById(int id);

        Task AddResponse(Response response);
        Task AddResponses(IEnumerable<Response> response);

        Task UpdateResponse(Response response);

        Task DeleteResponse(Response response);

        Task SaveChanges();
        List<Response> GetSurveyQuestionOpstionsInResponse(int id);
    }
}
