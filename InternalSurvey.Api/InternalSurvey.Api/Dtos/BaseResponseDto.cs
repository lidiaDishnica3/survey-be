using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Dtos
{
    public class BaseResponeDto<T> where T : class
    {
        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int PageNumber { get; set; }

        public List<T> Body { get; set; }
    }
}
