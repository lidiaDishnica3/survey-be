using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Entities
{
    public class SurveyQuestionOptions
    {
        public int Id { get; set; }
        public string Option { get; set; }
        public int QuestionId { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Question Question { get; set; }
        public ICollection<Response> Responses { get; set; }


    }
}
