using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Entities
{
  public class Comment : BaseEntity
  {
    public int CommentId { get; set; }
    public int SurveyId { get; set; }
    public int RespondentId { get; set; }
    public string CommnetText { get; set; }
    public Respondent Respondent { get; set; }
  }
}
