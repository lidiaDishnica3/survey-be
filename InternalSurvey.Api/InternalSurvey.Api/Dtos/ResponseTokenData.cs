using InternalSurvey.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Dtos
{
    public class RespondentTokenData
    {
        public string Email { get; set; }
        public Survey Survey { get; set; }
    }
}
