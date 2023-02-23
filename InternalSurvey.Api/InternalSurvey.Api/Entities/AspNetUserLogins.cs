using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Entities
{
    public class AspNetUserLogins
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(128)]
        public string LoginProvider { get; set; }

        [MaxLength(128)]
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }

        [MaxLength(450)]
        public string UserId { get; set; }

        public AspNetUsers User { get; set; }
    }
}
