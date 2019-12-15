using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Helpers
{
    /// <summary>
    ///  Mapper class that converts a price list entity to a price list domain.
    /// </summary>
    public class CurrencyMapper
    {
        /// <summary>
        /// Mapper that converts a price list entity to a price list domain.
        /// </summary>
        /// <param name="currencyEntity">Price list as entity.</param>
        /// <returns>Price list converts as price list domain.</returns>
        public Currency EntityToDomain(Entity currencyEntity)
        {
            Currency currency = new Currency();
            currency.Id = currencyEntity.Id;
            if (currencyEntity.Contains("name"))
            {
                currency.Name = (string)currencyEntity["name"];
            }
            return currency;
        }

    }
}
