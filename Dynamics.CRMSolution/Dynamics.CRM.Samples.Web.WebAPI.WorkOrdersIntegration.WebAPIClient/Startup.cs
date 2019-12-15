using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Arcos.CUC.WorkOrdersIntegration.WebAPIClient.Startup))]

namespace Arcos.CUC.WorkOrdersIntegration.WebAPIClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
