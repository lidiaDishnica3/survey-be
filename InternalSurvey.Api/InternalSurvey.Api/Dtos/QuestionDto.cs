using InternalSurvey.Api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Dtos
{
    public class QuestionDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public bool HasOthers { get; set; }
        public bool IsRequired { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int SurveyId { get; set; }
        public QuestionTypeEnum QuestionType { get; set; }
        public ICollection<SurveyQuestionOptionsDto> SurveyQuestionOptions { get; set; }
    }
}
