
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.BL
{
   public class OrderTransactionBL
    {
       #region Attributes

       private IOrderTransactionRepository _OrderTransactionRepository;

        #endregion

        public OrderTransactionBL(IOrderTransactionRepository _OrderTransactionRepository)
        {
            this._OrderTransactionRepository = _OrderTransactionRepository;
        }

        #region Methods

        public OrderTransaction GetFirstTransaction(Guid salesOrderId)
        {
            return _OrderTransactionRepository.GetFirstTransaction(salesOrderId);
        }
        public List<OrderTransaction> GetTransactionByOrder(Guid salesOrderId)
        {
            return _OrderTransactionRepository.GetTransactionByOrder(salesOrderId);
        }
        #endregion
    }
}
