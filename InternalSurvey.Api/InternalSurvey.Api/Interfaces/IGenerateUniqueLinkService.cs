using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Interfaces
{
    public interface IGenerateUniqueLinkService
    {
        string GenerateUniqueLink(string email, int surveyId, string appUrl,DateTime endDate);

        public List<Claim> GetClaims(string token);
    }
}
