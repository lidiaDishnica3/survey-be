using InternalSurvey.Api.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Dtos
{
    public class SurveyQuestionOptionsDto
    {
        public int Id { get; set; }
    
        public string Option { get; set; }
        public int QuestionId { get; set; }
        public DateTime? DeletedOn { get; set; }
        public ICollection<ResponseDto> Responses { get; set; }

    }
}
