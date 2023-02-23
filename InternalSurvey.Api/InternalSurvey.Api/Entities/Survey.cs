using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Entities
{
    public class Survey : BaseEntity
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
        public ICollection<Question> Questions { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public AspNetUsers AspNetUsers { get; set; }


    }
}
