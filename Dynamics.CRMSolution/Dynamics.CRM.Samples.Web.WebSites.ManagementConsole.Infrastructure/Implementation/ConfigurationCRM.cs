using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pavliks.WAM.ManagementConsole.Domain;
using System.Web.Configuration;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using CrmToolkit;
using Pavliks.WAM.ManagementConsole.Helpers;
using Microsoft.Xrm.Sdk.Client;
using System.ServiceModel.Description;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Implementation
{
    public class ConfigurationCRM : IConfigurationRepository
    {
        DataManager DataManager = new DataManager();

        public ConfigurationCRM()
        {

        }

        public Configuration GetConfiguration()
        {
            Configuration configuration = null;
            QueryExpression configurationQuery = new QueryExpression("dm_configuration")
            {
                ColumnSet = new ColumnSet(new string[] { "dm_emailsender", "dm_eventcalendarurl", "dm_managementconsoleurl", "dm_paymenturl", "dm_reportserverurl", "dm_orderreceiptreportpath", "dm_passworduserreports", "dm_userreports", "dm_apiloginid", "dm_apitransactionkey", "dm_managementconsoletestmode" })
            };

            EntityCollection configurations = DataManager.RetrieveMultiple(configurationQuery);

            if (configurations.Entities.Count > 0)
            {
                configuration = ConfigurationMapper.EntityToDomain(configurations.Entities[0]);
            }

            return configuration;
        }
    }
}
