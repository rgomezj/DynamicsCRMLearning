using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Helpers
{
    public class ConfigurationMapper
    {
        public ConfigurationMapper()
        {
        }

        public static Configuration EntityToDomain(Entity configurationEntity)
        {
            Configuration configuration = new Configuration();
            configuration.Id = configurationEntity.Id;

            string attribute = string.Empty;
            object valueAttribute = null;


            attribute = Mapping.GetAttributeName(configurationEntity, "dm_name");

            if (configurationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                configuration.Name = valueAttribute.ToString();
            }

            attribute = Mapping.GetAttributeName(configurationEntity, "dm_emailsender");

            if (configurationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                EntityReference reference = (EntityReference)valueAttribute;
                configuration.Emailsender = new User() { Id = reference.Id, FullName = reference.Name };
            }

            attribute = Mapping.GetAttributeName(configurationEntity, "dm_eventcalendarurl");

            if (configurationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                configuration.EventCalendarURL = valueAttribute.ToString();
            }

            attribute = Mapping.GetAttributeName(configurationEntity, "dm_managementconsoleurl");

            if (configurationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                configuration.ManagementConsoleURL = valueAttribute.ToString();
            }

            attribute = Mapping.GetAttributeName(configurationEntity, "dm_paymenturl");

            if (configurationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                configuration.PaymentURL = valueAttribute.ToString();
            }

            attribute = Mapping.GetAttributeName(configurationEntity, "dm_reportserverurl");

            if (configurationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                configuration.ReportServerURL = valueAttribute.ToString();
            }

            attribute = Mapping.GetAttributeName(configurationEntity, "dm_orderreceiptreportpath");

            if (configurationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                configuration.OrderReceiptReportPath = valueAttribute.ToString();
            }

            attribute = Mapping.GetAttributeName(configurationEntity, "dm_passworduserreports");

            if (configurationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                configuration.PasswordReportServer = valueAttribute.ToString();
            }

            attribute = Mapping.GetAttributeName(configurationEntity, "dm_userreports");

            if (configurationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                configuration.UserNameReportServer = valueAttribute.ToString();
            }
            attribute = Mapping.GetAttributeName(configurationEntity, "dm_apiloginid");

            if (configurationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                configuration.ApiLoginID = valueAttribute.ToString();
            }
            attribute = Mapping.GetAttributeName(configurationEntity, "dm_apitransactionkey");

            if (configurationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                configuration.ApiTransactionKey = valueAttribute.ToString();
            }

            attribute = Mapping.GetAttributeName(configurationEntity, "dm_managementconsoletestmode");

            if (configurationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                configuration.ManagementConsoleTestMode = (bool)valueAttribute;
            }
            return configuration;
        }
    }
}
