using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Arcos.CUC.WorkOrdersIntegration.WebAPIClient
{
    /*
        This class is created to transfer the data to the javascript callers. The reason is that we need to use the DataMember/DataContract attributes
        for WCF, but we don't want to do that directly in the object of Arcos.CUC.WorkOrdersIntegration.Core because they should not have any dependency
        on a superior layer like the WCF service.
        Autommaper is a librar that can be used to map this values easily without doing the process manually
    */

    [DataContract]
    public class WorkOrderData
    {
        [DataMember]
        public string WorkOrderNumber { get; set; }

        [DataMember]
        public string CustomerId { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string LocationId { get; set; }

        [DataMember]
        public string LocationAddress { get; set; }

        [DataMember]
        public string RequestorName { get; set; }

        [DataMember]
        public string RequestOrigin { get; set; }

        [DataMember]
        public string RequestCategory { get; set; }

        [DataMember]
        public string WorkType { get; set; }

        [DataMember]
        public string GeneralLocation { get; set; }

        [DataMember]
        public string ScheduledStartDate { get; set; }

        [DataMember]
        public string ShortDescription { get; set; }
    }
}