using InternalSurvey.Api.Entities;
using InternalSurvey.Api.Helpers;
using InternalSurvey.Api.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InternalSurvey.Api.Services
{
    public class QuestionsService : IQuestionsService
    {
        private readonly IGenericRepository<Question> _genericRepository;
        private ILogger<Question> _logger;
        public QuestionsService(IGenericRepository<Question> genericRepository, ILogger<Question> logger)
        {
            _genericRepository = genericRepository;
            _logger = logger;
        }
        public async Task<Question> AddQuestion(Question question)
        {
            try
            {
                await _genericRepository.Add(question);
                await _genericRepository.SaveChangesAsync();
                return question;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public async Task DeleteQuestion(Question Question)
        {
            try
            {
                _genericRepository.Remove(Question);
                await _genericRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public async Task<Question> GetQuestionById(int id)
        {
            try
            {
                var question = await _genericRepository.GetAllAsQueryable().Include(x => x.SurveyQuestionOptions).Where(x => x.DeletedOn == null).FirstOrDefaultAsync(x => x.Id == id);
                if (question != null && question.DeletedOn != null)
                {
                    return null;
                }
                return question;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public async Task<IEnumerable<Question>> GetQuestions()
        {
            try
            {
                return await _genericRepository.GetAllAsQueryable().Include(x => x.SurveyQuestionOptions).Where(x => x.DeletedOn == null).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public List<Question> GetSurveyIdInQuestion(int id)
        {
            try
            {
                return _genericRepository.Find(x => x.SurveyId == id).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public async Task SaveChanges()
        {
            try
            {
                await _genericRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public async Task<Question> UpdateQuestion(Question Question)
        {
            try
            {
                _genericRepository.Update(Question);
                await _genericRepository.SaveChangesAsync();
                return Question;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }
    }
}
