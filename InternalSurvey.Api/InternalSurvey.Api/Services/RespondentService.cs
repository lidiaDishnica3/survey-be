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
    public class RespondentService : IRespondentService
    {
        private readonly IGenericRepository<Respondent> _genericRepository;
        private readonly ILogger _logger;

        public RespondentService(IGenericRepository<Respondent> genericRepository, ILogger<Respondent> logger)
        {
            _genericRepository = genericRepository;
            _logger = logger;
        }

        public async Task AddRespondent(Respondent respondent)
        {
            try
            {
                await _genericRepository.Add(respondent);
                await _genericRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public async Task DeleteRespondent(Respondent respondent)
        {
            try
            {
                _genericRepository.Remove(respondent);
                await _genericRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }


        public async Task<Respondent> GetRespondentById(int id)
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

        public async Task<Respondent> GetRespondentByEmail(string email)
        {
            try
            {
                var respondent = await _genericRepository.FindOne(x => x.Email.Equals(email));
                return respondent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public async Task<List<Respondent>> GetRespondents()
        {
            try
            {
                var respondents = await _genericRepository.GetAllAsQueryable().Where(x => x.DeletedOn == null).ToListAsync();
                return respondents;
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

        public async Task UpdateRespondent(Respondent respondent)
        {
            try
            {
                _genericRepository.Update(respondent);
                await _genericRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }
        public async Task<bool?> HasVoted(string respondentEmail, string listOfRespondentIds)
        {
            try
            {
                var respondent = await _genericRepository.GetAllAsQueryable().FirstOrDefaultAsync(x => x.Email == respondentEmail);
                if (respondent == null)
                {
                    return null;
                }
                var emailIds = Utils.ConvertStringToList(listOfRespondentIds);

                return emailIds.Contains(respondent.Id.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }
    }
}
