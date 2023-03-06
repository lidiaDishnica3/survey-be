using System;
using System.Linq;
using InternalSurvey.Api.Data;
using InternalSurvey.Api.Interfaces;
using InternalSurvey.Api.Repository;
using InternalSurvey.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using AutoMapper;
using InternalSurvey.Api.Automapper;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using InternalSurvey.Api.Helpers.Email.Interfaces;
using InternalSurvey.Api.Helpers.Email.Services;
using InternalSurvey.Api.Helpers.Email;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Microsoft.OData.Edm;
using InternalSurvey.Api.Dtos;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

namespace InternalSurvey.Api
{
    public class Startup
  {
    public IConfiguration Configuration { get; }
    public Startup(IWebHostEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();

      Configuration = builder.Build();
      //Initialize Logger 
      Log.Logger = new LoggerConfiguration()
          .ReadFrom.Configuration(Configuration)
          .CreateLogger();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      //db connections
      services.AddDbContext<SurveyAppContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:SurveyCS"]));
      services.AddDbContext<ApplicationDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:SurveyCS"]));

      //Hangfire Configuration

      services.AddHangfire(configuration => configuration
          .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(Configuration["ConnectionString:SurveyCS"], new SqlServerStorageOptions
          {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.FromSeconds(30F),
            UseRecommendedIsolationLevel = true,
            UsePageLocksOnDequeue = true,
            DisableGlobalLocks = true
          }));
      services.AddHangfireServer();



      var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("AppSettings:Secret"));

      services.AddDefaultIdentity<ApplicationUser>()
          .AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();

      // jwt
      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
          .AddJwtBearer(jwtOptions =>
          {
            jwtOptions.RequireHttpsMetadata = false;
            jwtOptions.SaveToken = true;
            jwtOptions.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = new SymmetricSecurityKey(key),
              ValidateIssuer = false,
              ValidateAudience = false
            };
          });


      //cors
      services.AddCors(options =>
      {
        options.AddPolicy("CorsPolicy",
            builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
      });
      // json ignore
      services.AddControllers()
        .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
        .AddDataAnnotationsLocalization();

      //di for services
      services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
      services.AddScoped<IUsersService, UsersService>();
      services.AddScoped<IQuestionsService, QuestionsService>();
      services.AddScoped<ICommentService, CommentService>();
      services.AddScoped<ISurveyService, SurveyService>();
      services.AddScoped<ISurveySubmissionService, SurveySubmissionService>();
      services.AddScoped<IRespondentService, RespondentService>();
      services.AddScoped<IResponseService, ResponseService>();
      services.AddScoped<ISurveyQuestionOptionsService, SurveyQuestionOptionsService>();
      services.AddScoped<IEmailService, EmailService>();
      services.AddScoped<IImageService, ImageService>();
      services.AddScoped<IGenerateUniqueLinkService, GenerateUniqueLinkService>();

      //email config
      services.Configure<Email>(Configuration.GetSection("EmailConfiguration"));

      //swagger
      services.AddSwaggerDocumentation();

      //automapper
      var mappingConfig = new MapperConfiguration(mc =>
      {
        mc.AddProfile(new Mapping());
      });

      IMapper mapper = mappingConfig.CreateMapper();
      services.AddSingleton(mapper);
         services.AddControllers()
             .AddOData(opt => opt.AddRouteComponents("api", GetEdmModel()));

            //services.AddOData();

      services.AddMvcCore(options =>
      {
        foreach (var outputFormatter in options.OutputFormatters.OfType<OutputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
        {
          outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
        }

        foreach (var inputFormatter in options.InputFormatters.OfType<InputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
        {
          inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
        }
      });
    }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      //cors
      app.UseCors("CorsPolicy");
      app.UseHttpsRedirection();
      //serilog
      app.UseSerilogRequestLogging();
      //swagger
      app.UseSwaggerDocumentation();

      // HangfireUI
      app.UseHangfireDashboard();
      app.UseHangfireServer(new BackgroundJobServerOptions
      {
        WorkerCount = 1
      });
      GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 1 });

      app.UseAuthentication();
      app.UseFileServer();

      app.UseRouting();
      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        //endpoints.MapODataRoute("api", "api", GetEdmModel());
        //endpoints.EnableDependencyInjection();
      });
    }

    private IEdmModel GetEdmModel()
    {
      var odataBuilder = new ODataConventionModelBuilder();
      odataBuilder.EntitySet<RespondentDto>("Respondent");
      odataBuilder.EntitySet<SurveyDto>("Survey");
      odataBuilder.EntitySet<UserDto>("AspNetUsers");

      return odataBuilder.GetEdmModel();
    }

  }
}
