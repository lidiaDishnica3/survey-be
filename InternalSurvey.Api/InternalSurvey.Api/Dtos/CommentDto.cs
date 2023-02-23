using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Dtos
{
  public class CommentDto
  {
    public int CommentId { get; set; }
    public int RespondentId { get; set; }
    public int SurveyId { get; set; }
    public string CommnetText { get; set; }
    
    public RespondentDto Respondent { get; set; }
  }
}
