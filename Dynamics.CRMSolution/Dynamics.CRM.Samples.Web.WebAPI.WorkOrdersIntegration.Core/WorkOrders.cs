using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arcos.CUC.WorkOrdersIntegration.Core
{
    public class WorkOrder
    {
        public string WorkOrderNumber { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string LocationId { get; set; }

        public string LocationAddress { get; set; }

        public string RequestorName { get; set; }

        public string RequestOrigin { get; set; }

        public string RequestCategory { get; set; }

        public string WorkType { get; set; }

        public string GeneralLocation { get; set; }

        public string ScheduledStartDate { get; set; }

        public string ShortDescription { get; set; }

    }
}