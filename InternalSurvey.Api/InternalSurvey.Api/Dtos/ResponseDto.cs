using InternalSurvey.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Dtos
{
    public class ResponseDto
    {
        public int Id { get; set; }
        public int RespondentId { get; set; }
        public int SurveyQuestionOptionsId { get; set; }
        public string Other { get; set; }
        public DateTime? DeletedOn { get; set; }

    }
}
