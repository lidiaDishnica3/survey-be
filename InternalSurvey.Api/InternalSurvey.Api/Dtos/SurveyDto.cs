using InternalSurvey.Api.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Dtos
{
    public class SurveyDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
        public string VotingRespondents { get; set; }
        public string SwitchOffRespondents { get; set; }
        public ICollection<QuestionDto> QuestionDtos { get; set; }
    public ICollection<CommentDto> CommentDtos { get; set; }
  }
}
