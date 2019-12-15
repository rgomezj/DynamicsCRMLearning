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
    ///  Mapper class that converts a product entity to a product domain.
    /// </summary>
    public class ProductMapper
    {

        private IOrganizationService _organizationService;
        public ProductMapper(IOrganizationService service)
        {

            this._organizationService = service;
        }


        /// <summary>
        /// Mapper that converts a registration entity to a registration domain.
        /// </summary>
        /// <param name="productEntity">Registration as entity.</param>
        /// <returns>Registration converts as sales order domain.</returns>
        public Product EntityToDomain(Entity productEntity)
        {
            Product product = new Product();

            if (productEntity.Contains("productid"))
            {
                product.Id = Guid.Parse(productEntity["productid"].ToString());
            }
            if (productEntity.Contains("Products.name"))
            {
                product.Name = ((AliasedValue)productEntity["Products.name"]).Value.ToString();

            }
            if (productEntity.Contains("PriceLevel.amount"))
            {
                product.UnitPrice = (((Money)(((AliasedValue)productEntity["PriceLevel.amount"]).Value)).Value);

            }
            if (productEntity.Contains("price"))
            {
                product.UnitPrice = (decimal)productEntity["price"];
            }


            if (productEntity.Contains("pricelevelid"))
            {
                product.PriceList = getPriceList((EntityReference)productEntity["pricelevelid"]);
            }

            if (productEntity.Contains("description"))
            {
                product.Description =(string)productEntity["description"];
            }
            return product;
        }

        /// <summary>
        /// Method that gets the the price list associated to the product.
        /// </summary>
        /// <param name="priceListReference"></param>
        /// <returns> The price list with all its attributes. </returns>
        public PriceList getPriceList(EntityReference priceListReference)
        {

            Entity priceList = _organizationService.Retrieve(priceListReference.LogicalName, priceListReference.Id, new ColumnSet(true));

            PriceListMapper priceListMapper = new PriceListMapper();

            return priceListMapper.EntityToDomain(priceList);
        }
    }
}
