using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using InternalSurvey.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InternalSurvey.Api.Controllers
{
    public class BaseController<T> : ControllerBase
    {
        private readonly ILogger<T> _logger;
        private readonly IConfiguration _configuration;
        private ILogger<SurveyController> logger;

        internal string BasePath => _configuration.GetValue<string>("APIUrl") ?? string.Empty;

        public BaseController(ILogger<T> logger)
        {
            _logger = logger;
        }

        public BaseController(ILogger<T> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public BaseController(ILogger<SurveyController> logger)
        {
            this.logger = logger;
        }

        internal string GetEmailUsername()
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                    return claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        internal string GetUserId()
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                    return claimsIdentity.FindFirst("userId")?.Value;
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }
    }
}