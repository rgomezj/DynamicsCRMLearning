using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using CrmToolkit;
using Pavliks.WAM.ManagementConsole.Helpers;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Implementation
{
   public class ProductCRM : IProductRepository
    {
        private const string PRODUCTTYPEMISCELLANEOUS = "2";
        private const string PRODUCTTYPEREGISTRATION = "602300000";

        public List<Product> GetRegistrationLevels(Guid eventId)
        {
            List<Product> products = new List<Product>();
            
            DataManager DataManager = new DataManager();
            
            QueryExpression registrationLevelsQuery = new QueryExpression("dm_dm_course_product")
            {
                ColumnSet = new ColumnSet(new string[] { "productid" })
            };

            registrationLevelsQuery.Criteria.AddCondition("dm_courseid", ConditionOperator.Equal, eventId);
            registrationLevelsQuery.LinkEntities.Add(new LinkEntity("dm_dm_course_product", "product", "productid", "productid", JoinOperator.Inner));
            registrationLevelsQuery.LinkEntities[0].EntityAlias = "Products";
            registrationLevelsQuery.LinkEntities[0].Columns.AddColumns(new string[] { "productid", "name", "pricelevelid" });
            registrationLevelsQuery.LinkEntities[0].LinkEntities.Add(new LinkEntity("productid","productpricelevel", "productid", "productid",JoinOperator.Inner));
            registrationLevelsQuery.LinkEntities[0].LinkCriteria.Conditions.Add(new ConditionExpression("producttypecode", ConditionOperator.Equal, PRODUCTTYPEREGISTRATION));
            registrationLevelsQuery.LinkEntities[0].LinkEntities[0].LinkEntities.Add(new LinkEntity("productpricelevel", "pricelevel", "pricelevelid", "pricelevelid", JoinOperator.Inner));
            registrationLevelsQuery.LinkEntities[0].LinkEntities[0].EntityAlias = "PriceLevel";
            registrationLevelsQuery.LinkEntities[0].LinkEntities[0].Columns.AddColumns(new string[] { "amount", "pricelevelid" });

            EntityCollection productsCollection = DataManager.RetrieveMultiple(registrationLevelsQuery);

            foreach (var productItem in productsCollection.Entities)
            {
                Product product = GetProductFromEntity(productItem);
                products.Add(product);

            }

            return products;
        }

        private static Product GetProductFromEntity(Entity productItem)
        {
            Product product = new Product();

            if (productItem.Contains("productid"))
            {
                product.Id = Guid.Parse(productItem["productid"].ToString());
            }
            if (productItem.Contains("Products.name"))
            {
                product.Name = ((AliasedValue)productItem["Products.name"]).Value.ToString();

            }

            string attribute = Mapping.GetAttributeName(productItem, "PriceLevel.pricelevelid");
            object valueAttribute = null;
            if (productItem.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                product.PriceList = new PriceList() { Id = ((EntityReference)((AliasedValue)valueAttribute).Value).Id };
            }

            if (productItem.Contains("PriceLevel.amount"))
            {
                product.UnitPrice = (((Money)(((AliasedValue)productItem["PriceLevel.amount"]).Value)).Value);

            }
            if (productItem.Contains("price"))
            {
                product.UnitPrice = (decimal)productItem["price"];
            }

            return product;
        }
    }
}
