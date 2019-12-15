using AutoMapper;
using Pavliks.WAM.ManagementConsole.BL;
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Implementation;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using Pavliks.WAM.ManagementConsole.ManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Controllers
{
    public class ConfigurationController : ApiController
    {
        private ConfigurationBL _ConfigurationBL;

        public ConfigurationController(IConfigurationRepository _IConfigurationRepository)
        {
            _ConfigurationBL = new ConfigurationBL(_IConfigurationRepository);
        }

        [System.Web.Http.HttpGet()]
        public HttpResponseMessage GetConfiguration()
        {
            Configuration configuration = _ConfigurationBL.GetConfiguration();
            return Request.CreateResponse(HttpStatusCode.OK, configuration);
        }

    }
}