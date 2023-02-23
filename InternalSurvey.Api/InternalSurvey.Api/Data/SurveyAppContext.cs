using InternalSurvey.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Data
{
  public class SurveyAppContext : DbContext
  {
    public SurveyAppContext(DbContextOptions<SurveyAppContext> options) : base(options)
    {
    }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Respondent> Respondents { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<SurveyQuestionOptions> SurveyQuestionOptions { get; set; }
    public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
    public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
    public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
    public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      //base.OnModelCreating(modelBuilder);
      modelBuilder.Seed();

    }
  }
}
