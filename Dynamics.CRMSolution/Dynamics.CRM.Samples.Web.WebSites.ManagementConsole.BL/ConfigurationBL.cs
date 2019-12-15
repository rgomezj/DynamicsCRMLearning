
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.BL
{
   public class ConfigurationBL
    {
       #region Attributes

       private IConfigurationRepository _ConfigurationRepository;

        #endregion

        public ConfigurationBL(IConfigurationRepository _ConfigurationRepository)
        {
            this._ConfigurationRepository = _ConfigurationRepository;
        }

        #region Methods

        public Configuration GetConfiguration()
        {
            return _ConfigurationRepository.GetConfiguration();
        }

        #endregion
   }
}
