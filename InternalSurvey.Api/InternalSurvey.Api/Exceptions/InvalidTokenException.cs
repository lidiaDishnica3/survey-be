using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Exceptions
{
    public class InvalidTokenException : Exception, IInvalidProperty
    {
        public string Email { get; set; }
        public bool InvalidToken { get; set; }
        public bool InvalidSurvey { get; set; }
        public bool SurveyHasExpired { get; set; }
        public bool InvalidEmail { get; set; }
        public bool HasVoted { get; set; }

        public object GetInvalidProperty()
        {
            if (InvalidToken)
            {
                return new { InvalidToken = true };
            }
            else if (InvalidSurvey)
            {
                return new { InvalidSurvey = true };
            }
            else if (SurveyHasExpired)
            {
                return new { SurveyHasExpired = true };
            }
            else if (InvalidEmail)
            {
                return new { InvalidEmail = true };
            }
            else if (HasVoted)
            {
                return new { HasVoted = true, Email };
            }
            else
            {
                //default option
                return new { InvalidToken = true };
            }

        }
    }

}
