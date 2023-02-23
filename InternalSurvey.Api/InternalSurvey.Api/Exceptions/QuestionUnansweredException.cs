using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Exceptions
{
    public class UnansweredQuestionException : Exception, IInvalidProperty
    {
        public int UnansweredQuestionOrder { get; set; }

        public object GetInvalidProperty()
        {
            return new { UnansweredQuestionOrder };
        }
    }
}
