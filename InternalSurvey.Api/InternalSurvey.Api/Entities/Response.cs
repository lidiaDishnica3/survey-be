using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Entities
{
    public class Response
    {
        public int Id { get; set; }
        public int RespondentId { get; set; }
        public int SurveyQuestionOptionsId { get; set; }
        public string Other { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Respondent Respondent { get; set; }
        public SurveyQuestionOptions  SurveyQuestionOption { get; set; }
    }
}
