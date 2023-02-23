using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Exceptions
{
    public interface IInvalidProperty
    {
        public object GetInvalidProperty();
    }
}
