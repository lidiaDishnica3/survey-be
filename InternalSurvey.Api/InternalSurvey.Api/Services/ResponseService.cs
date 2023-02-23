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
    public class ResponseService : IResponseService
    {
        private readonly IGenericRepository<Response> _genericRepository;
        private readonly ILogger<Response> _logger;

        public ResponseService(IGenericRepository<Response> genericRepository, ILogger<Response> logger)
        {
            _genericRepository = genericRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Response>> GetResponses()
        {
            try
            {
                var responses = await _genericRepository.GetAllAsQueryable().Where(x => x.DeletedOn == null).ToListAsync();
                return responses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public async Task<Response> GetResponseById(int id)
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

        public async Task AddResponse(Response response)
        {
            try
            {
                await _genericRepository.Add(response);
                await _genericRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public async Task DeleteResponse(Response response)
        {
            try
            {
                _genericRepository.Remove(response);
                await _genericRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public async Task UpdateResponse(Response response)
        {
            try
            {
                _genericRepository.Update(response);
                await _genericRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public List<Response> GetSurveyQuestionOpstionsInResponse(int id)
        {
            try
            {
                return _genericRepository.Find(x => x.SurveyQuestionOptionsId == id).ToList();
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

        public async Task AddResponses(IEnumerable<Response> responses)
        {
            try
            {
                await _genericRepository.AddRange(responses);
                await _genericRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }
    }
}
