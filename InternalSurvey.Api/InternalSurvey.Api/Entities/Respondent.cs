using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Entities
{
    public class Respondent : BaseEntity
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        public ICollection<Response> Responses  { get; set; }
    }
}
