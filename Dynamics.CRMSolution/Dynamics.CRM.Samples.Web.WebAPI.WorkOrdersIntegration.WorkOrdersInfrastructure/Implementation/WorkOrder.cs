using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Xml;
using Arcos.CUC.WorkOrdersInfrastructure.Interfaces;
using CoreModels = Arcos.CUC.WorkOrdersIntegration.Core;

namespace Arcos.CUC.WorkOrdersInfrastructure.Implementation
{
    public class WorkOrder : IWorkOrder
    {
        PetaPoco.Database db;
        DataUtilities dataUtilities;

        public WorkOrder()
        {
            db = new PetaPoco.Database(DataUtilities.GetConnectionString(), DataUtilities.GetProvider());
            db.EnableAutoSelect = false;
            dataUtilities = new DataUtilities();
        }

        public string GetAssociatedWorkOrders(string customerid, string locationid)
        {
            //List<CoreModels.WorkOrder> list = new List<CoreModels.WorkOrder>();
            //list = db.Fetch<CoreModels.WorkOrder>(@";EXEC OPR.CUST_WORKORDERS_GET @@CUSTOMER_ID =@0,@@LOCATION_ID = @1", customerid, locationid);
            //string workorderlist = string.Join<CoreModels.WorkOrder>(",", list.ToArray());
            //return workorderlist;    
            List<string> list = new List<string>();
            list = db.Fetch<string>(@";EXEC OPR.CUST_WORKORDERS_GET @@CUSTOMER_ID =@0,@@LOCATION_ID = @1", customerid, locationid);
            string workorderlist = string.Join(",", list.ToArray());
            return workorderlist;
        }

        public CoreModels.WorkOrder GetWorkOrderDetails(string workordernumber)
        {
            string inputData = string.Format(@"<WFJOinquiryBODY>                        
                                          <SECattributes>                       
                                            <PsRid>{0}</PsRid>                 
                                            <UserSid>{1}</UserSid>   
                                          </SECattributes>                      
                                          <ENVattributes>                       
                                            <ProgramLib>HTEPGM</ProgramLib>
                                            <DataLib>HTEDTA</DataLib>     
                                            <Application>HT</Application>    
                                            <AdditionalLib></AdditionalLib>  
                                            <Suffix></Suffix>                
                                            <Environment></Environment>  
                                          </ENVattributes>                      
                                          <WFJOinquiryIN> 
                                            <ThirdPtyID>3RDPTY</ThirdPtyID>                      
                                            <RequestNbr>{2}</RequestNbr>
                                            <JobNbr></JobNbr>
                                          </WFJOinquiryIN>                       
                                        </WFJOinquiryBODY>", Properties.Settings.Default.userId, Properties.Settings.Default.password , workordernumber);

            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(inputData);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(Properties.Settings.Default.url));
            request.Method = "POST";
            request.ContentType = "text/xml; encoding='utf-8'";
            request.ContentLength = byteArray.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(byteArray, 0, byteArray.Length);
            requestStream.Close();

            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            string responseStr = string.Empty;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                responseStr = new StreamReader(responseStream).ReadToEnd();
            }

            CoreModels.WorkOrder workorder = new WorkOrdersIntegration.Core.WorkOrder();

            if (responseStr != string.Empty)
            {
                
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(responseStr);

                XmlNodeList xnList = xml.SelectNodes("/Results");
                foreach (XmlNode xn in xnList)
                {
                    XmlNode anode = xn.SelectSingleNode("WFJOinquiryRSP");
                    if (anode != null)
                    {
                        workorder.CustomerId = anode["CustomerID"].InnerText;
                        workorder.CustomerName = anode["CustName"].InnerText;
                        workorder.LocationAddress = anode["LocAddr"].InnerText;
                        workorder.LocationId = anode["LocationID"].InnerText;
                        workorder.RequestCategory = anode["ReqCatg"].InnerText;
                        workorder.RequestOrigin = anode["ReqOrigin"].InnerText;
                        workorder.RequestorName = anode["CustomerID"].InnerText;
                        //workorder.ScheduledStartDate = anode["WRSchStrDte"].InnerText;
                        workorder.ShortDescription = anode["ShortDsc"].InnerText;
                        workorder.WorkOrderNumber = anode["RequestNbr"].InnerText;
                        workorder.WorkType = anode["WorkType"].InnerText;

                    }
                }
              }
            return workorder;
        }
    }
}









