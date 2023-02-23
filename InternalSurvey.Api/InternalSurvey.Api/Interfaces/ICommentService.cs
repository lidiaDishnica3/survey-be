using InternalSurvey.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace InternalSurvey.Api.Interfaces
{
  public interface ICommentService
  {
    Task<Comment> AddComment(Comment comment);
    Task DeleteComment(Comment comment);
    Task<Comment> GetCommentById(int id);

    Task<IEnumerable<Comment>> GetCommentsBySurveyId(int SurveyId);
  }
}
