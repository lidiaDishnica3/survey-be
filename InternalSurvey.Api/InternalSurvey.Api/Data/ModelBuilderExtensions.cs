using InternalSurvey.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher<AspNetUsers>();
            modelBuilder.Entity<AspNetUsers>().HasData(
                new AspNetUsers
                {
                    Id = "a4c95d33-b702-499c-b436-621e786e7518",
                    Email = "atisadmin@atis.al",
                    CreatedBy = "atisadmin@atis.al",
                    CreatedOn = DateTime.Now,
                    FirstName = "Atis",
                    LastName = "Admin",
                    PasswordHash = hasher.HashPassword(null, "Admin123*"),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "atisadmin@atis.al",
                    NormalizedUserName = "atisadmin@atis.al".ToUpper(),
                    NormalizedEmail = "atisadmin@atis.al".ToUpper(),
                }

            );
            modelBuilder.Entity<Respondent>().HasData(
              new Respondent
              {
                  Id = 1,
                  Email = "rei.xhiani@atis.al",

              },
                new Respondent
                {
                    Id = 2,
                    Email = "nexhip.alimadhi@atis.al",

                },
                  new Respondent
                  {
                      Id = 3,
                      Email = "lidia.dishnica@atis.al",

                  }

          );
        }
    }
}
