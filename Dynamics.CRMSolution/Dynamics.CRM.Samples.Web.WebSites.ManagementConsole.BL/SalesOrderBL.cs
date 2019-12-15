using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pavliks.WAM.ManagementConsole.BL
{


    public class SalesOrderBL
    {

        #region Attributes
        private ISalesOrderRepository _SalesOrderRepository;
        #endregion
        #region Properties
        #endregion
        #region Methods
        public SalesOrderBL(ISalesOrderRepository salesOrderRepository)
        {
            this._SalesOrderRepository = salesOrderRepository;
        }

        public SalesOrder GetSalesOrder(string id)
        {
            return _SalesOrderRepository.GetSalesOrderById(id);
        }

        public void ChangeStatus(SalesOrder salesOrderChange)
        {
            if(salesOrderChange.StateOrder == StateOrder.Active)
            {
                _SalesOrderRepository.OpenOrder(salesOrderChange.Id);
            }
            else
            {
                if(salesOrderChange.StateOrder == StateOrder.Canceled)
                {
                    _SalesOrderRepository.CancelOrder(salesOrderChange.Id);
                }
                else
                {
                    _SalesOrderRepository.CloseOrder(salesOrderChange.Id);
                }
            }

        }
        #endregion




    }
}
