using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Pavliks.WAM.ManagementConsole.ManagementAPI.Startup))]

namespace Pavliks.WAM.ManagementConsole.ManagementAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
