using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Entities
{
    public class SurveyResult
    {
        [Key]
        public int Id { get; set; }
    }
}
