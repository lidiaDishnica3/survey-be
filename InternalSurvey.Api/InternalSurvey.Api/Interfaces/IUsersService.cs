using InternalSurvey.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Interfaces
{
    public interface IUsersService
    {
        Task AddUser(AspNetUsers user);
        Task DeleteUser(AspNetUsers user);
        Task<AspNetUsers> GetUserById(int id);
        Task<AspNetUsers> GetUserByStringId(string id);
        Task<IEnumerable<AspNetUsers>> GetUsers();
        Task SaveChanges();
        void UpdateUser(AspNetUsers user);
    }
}
