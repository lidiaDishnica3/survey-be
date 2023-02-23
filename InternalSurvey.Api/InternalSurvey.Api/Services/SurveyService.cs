using InternalSurvey.Api.Entities;
using InternalSurvey.Api.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.Tracing;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using InternalSurvey.Api.Helpers;

namespace InternalSurvey.Api.Services
{
  public class SurveyService : ISurveyService
  {
    private readonly IGenericRepository<Survey> _genericRepository;
    private ILogger<Survey> _logger;
    public SurveyService(IGenericRepository<Survey> genericRepository, ILogger<Survey> logger)
    {
      _genericRepository = genericRepository;
      _logger = logger;
    }

    public async Task<Survey> AddSurvey(Survey survey)
    {
      try
      {
        await _genericRepository.Add(survey);
        await _genericRepository.SaveChangesAsync();
        return survey;
      }
      catch (Exception ex)
      {

        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        throw ex;
      }
    }

    public async Task DeleteSurvey(Survey survey)
    {

      try
      {
        _genericRepository.Remove(survey);
        await _genericRepository.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        throw ex;
      }
    }

    public async Task<Survey> GetSurveyById(int id)
    {
      try
      {
        var survey = await _genericRepository.GetById(id);
        if (survey != null && survey.DeletedOn != null)
        {
          return null;
        }
        return survey;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        throw ex;
      }
    }
    public async Task<Survey> GetSurveyByIdExtended(int id)
    {
      try
      {
        var survey = await _genericRepository
            .GetAllAsQueryable()
            .Where(x => x.DeletedOn == null)
            .Include(i => i.Questions)
            .ThenInclude(o => o.SurveyQuestionOptions)
            .Include(w => w.Comments)
            .ThenInclude(q => q.Respondent)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (survey != null && survey.DeletedOn != null)
        {
          return null;
        }
        return survey;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        throw ex;
      }
    }
    public async Task<Survey> GetSurveyByIdExtendedTotal(int id)
    {
      try
      {
        var survey = await _genericRepository
            .GetAllAsQueryable()
            .Where(x => x.DeletedOn == null && x.StartDate != null)
            .Include(i => i.Questions)
            .ThenInclude(o => o.SurveyQuestionOptions)
            .ThenInclude(r => r.Responses)
            .Include(w => w.Comments)
            .ThenInclude(q => q.Respondent)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (survey != null && survey.DeletedOn != null)
        {
          return null;
        }
        return survey;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        throw ex;
      }
    }

    public async Task<IEnumerable<Survey>> GetSurveys()
    {
      try
      {
        return await _genericRepository.GetAllAsQueryable().Where(x => x.DeletedOn == null).Include(i => i.Questions)
            .Include(w => w.Comments)
            .ThenInclude(q => q.Respondent).ToListAsync();

      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        throw ex;
      }
    }
    public async Task<IEnumerable<Survey>> GetSurveysPublished()
    {
      try
      {
        return await _genericRepository.GetAllAsQueryable().Where(x => x.DeletedOn == null && x.StartDate != null).Include(i => i.Questions)
            .Include(w => w.Comments)
            .ThenInclude(q => q.Respondent).ToListAsync();

      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        throw ex;
      }
    }

    public async Task<Survey> UpdateSurvey(Survey survey)
    {
      try
      {
        _genericRepository.Update(survey);
        await _genericRepository.SaveChangesAsync();
        return survey;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
        throw ex;
      }
    }
  }
}
