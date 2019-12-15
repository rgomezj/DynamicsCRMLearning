
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.BL
{
   public class ProductBL
   {
        #region Attributes

        private IProductRepository _ProductRepository;

        #endregion

        public ProductBL(IProductRepository _ProductRepository)
        {
            this._ProductRepository = _ProductRepository;
        }

        #region Methods

        public List<Product> GetRegistrationLevels(Guid eventId)
        {
            return _ProductRepository.GetRegistrationLevels(eventId);
        }

        #endregion

    }
}
