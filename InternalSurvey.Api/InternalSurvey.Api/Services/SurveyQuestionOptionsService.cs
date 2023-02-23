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
    public class SurveyQuestionOptionsService : ISurveyQuestionOptionsService
    {
        private readonly IGenericRepository<SurveyQuestionOptions> _genericRepository;
        private ILogger<SurveyQuestionOptions> _logger;
        public SurveyQuestionOptionsService(IGenericRepository<SurveyQuestionOptions> genericRepository, ILogger<SurveyQuestionOptions> logger)
        {
            _genericRepository = genericRepository;
            _logger = logger;
        }

        public async Task<SurveyQuestionOptions> AddSurveyQuestionOptions(SurveyQuestionOptions surveyQuestionOptions)
        {
            try
            {
                await _genericRepository.Add(surveyQuestionOptions);
                await _genericRepository.SaveChangesAsync();
                return surveyQuestionOptions;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An unexpected error has occured");
                throw ex;
            }
        }

        public async Task DeleteSurveyQuestionOptions(SurveyQuestionOptions surveyQuestionOptions)
        {

            try
            {
                _genericRepository.Remove(surveyQuestionOptions);
                await _genericRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error has occured");
                throw ex;
            }
        }

        public async Task<SurveyQuestionOptions> GetSurveyQuestionOptionsById(int id)
        {
            try
            {

                var surveyQuestionOptions = await _genericRepository.GetById(id);
                if (surveyQuestionOptions != null && surveyQuestionOptions.DeletedOn != null)
                {
                    return null;
                }
                return surveyQuestionOptions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error has occured");
                throw ex;
            }
        }
 
        public async Task<IEnumerable<SurveyQuestionOptions>>  GetSurveyQuestionOptions()
        {
            try
            {
                return await _genericRepository.GetAllAsQueryable().Where(x => x.DeletedOn == null).Include(i => i.Responses).ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error has occured");
                throw ex;
            }
        }

        public async Task UpdateSurveyQuestionOptions(SurveyQuestionOptions surveyQuestionOptions)
        {
            try
            {
                _genericRepository.Update(surveyQuestionOptions);
                await _genericRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error has occured");
                throw ex;
            }
        }

        public async Task<SurveyQuestionOptions> GetOtherSurveyQuestionOptionsByQuestionId(int questionId)
        {
            try
            {
                return await _genericRepository.GetAllAsQueryable().Where(x => x.DeletedOn == null && x.QuestionId==questionId && x.Option.Equals("hasOtherOption2929")).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error has occured");
                throw ex;
            }
        }

        public List<SurveyQuestionOptions> GetQuestionInSurveyQuestion(int id)
        {
            try
            {
                return _genericRepository.Find(x => x.QuestionId == id).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }
    }
}
