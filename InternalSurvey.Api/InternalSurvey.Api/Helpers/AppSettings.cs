﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}
