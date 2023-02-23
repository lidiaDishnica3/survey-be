using InternalSurvey.Api.Entities;
using InternalSurvey.Api.Helpers;
using InternalSurvey.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Services
{
  public class CommentService : ICommentService
  {
    private readonly IGenericRepository<Comment> _genericRepository;
    private readonly ILogger<Comment> _logger;

    public CommentService(IGenericRepository<Comment> genericRepository, ILogger<Comment> logger)
    {
      _genericRepository = genericRepository;
      _logger = logger;
    }

    public async Task<Comment> AddComment(Comment comment)
    {
      try
      {

        await _genericRepository.Add(comment);
        await _genericRepository.SaveChangesAsync();
        return comment;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        throw ex;
      }
    }

    public async Task DeleteComment(Comment comment)
    {
      try
      {
        _genericRepository.Remove(comment);
        await _genericRepository.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        throw ex;
      }
    }

    public async Task<Comment> GetCommentById(int id)
    {
      try
      {
        return await _genericRepository.GetById(id);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        throw ex;
      }
    }

    public async Task<IEnumerable<Comment>> GetCommentsBySurveyId(int SurveyId)
    {
      try
      {
        var comments = await _genericRepository.GetAllAsQueryable().Where(x => x.SurveyId == SurveyId).ToListAsync();
        return comments;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        throw ex;
      }
    }
  }
}
