using InternalSurvey.Api.Entities;
using InternalSurvey.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Services
{
    public class UsersService : IUsersService
    {
        private readonly IGenericRepository<AspNetUsers> _genericRepository;
        public UsersService(IGenericRepository<AspNetUsers> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task AddUser(AspNetUsers user)
        {
            try
            {
                await _genericRepository.Add(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteUser(AspNetUsers user)
        {
            try
            {
                _genericRepository.Remove(user);
                await _genericRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AspNetUsers> GetUserById(int id)
        {
            try
            {
                return await _genericRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AspNetUsers> GetUserByStringId(string id)
        {
            try
            {
                return await _genericRepository.GetByStringId(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<AspNetUsers>> GetUsers()
        {
            try
            {
                return await _genericRepository.GetAll();
            }
            catch (Exception ex)
            {
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
                throw ex;
            }
        }

        public void UpdateUser(AspNetUsers user)
        {
            try
            {
                _genericRepository.Update(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
