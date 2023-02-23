using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Dtos
{
    public class RespondentTokenDto
    {
        public string Email { get; set; }
        public SurveyDto Survey { get; set; }
    }
}
