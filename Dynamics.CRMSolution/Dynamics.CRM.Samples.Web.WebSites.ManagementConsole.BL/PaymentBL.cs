
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizeNet;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using AuthorizeNet.Api.Controllers;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Pavliks.WAM.ManagementConsole.BL
{
    public class PaymentBL
    {
        #region Attributes
        private IConfigurationRepository _ConfigurationRepository;
        private IOrderTransactionRepository _OrderTransactionRepository;


        #endregion

        public PaymentBL(IConfigurationRepository _ConfigurationRepository, IOrderTransactionRepository _OrderTransactionRepository)
        {
            this._ConfigurationRepository = _ConfigurationRepository;
            this._OrderTransactionRepository = _OrderTransactionRepository;


        }


        public PaymentResponse Refund(SalesOrder salesOrder, bool refundTrans)
        {
            Configuration configuration = _ConfigurationRepository.GetConfiguration();
            OrderTransaction orderTransaction = _OrderTransactionRepository.GetFirstTransaction(salesOrder.Id);
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = null;
            if (configuration.ManagementConsoleTestMode)
            {
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            }
            else
            {
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;
            }

            PaymentResponse paymentResponse = new PaymentResponse();
            if (orderTransaction.CCNumber != null)
            {
                // define the merchant information (authentication / transaction id)
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
                {
                    name = configuration.ApiLoginID,
                    ItemElementName = ItemChoiceType.transactionKey,
                    Item = configuration.ApiTransactionKey
                };

                var creditCard = new creditCardType
                {
                    cardNumber = orderTransaction.CCNumber,
                    expirationDate = "1234"
                };

                //standard api call to retrieve response
                var paymentType = new paymentType { Item = creditCard };

                string transactionType = transactionTypeEnum.voidTransaction.ToString();

                if (refundTrans)
                {
                    transactionType = transactionTypeEnum.refundTransaction.ToString();
                }

                var transactionRequest = new transactionRequestType
                {
                    transactionType = transactionType,    // refund type
                    payment = paymentType,
                    amount = salesOrder.PaidAmount.Value,
                    refTransId = orderTransaction.TransactionId
                };

                var request = new createTransactionRequest { transactionRequest = transactionRequest };

                // instantiate the contoller that will call the service
                var controller = new createTransactionController(request);
                controller.Execute();

                // get the response from the service (errors contained if any)
                var response = controller.GetApiResponse();

                //validate
                if (response != null)
                {
                    if (response.messages.resultCode == messageTypeEnum.Ok)
                    {
                        if (response.transactionResponse.messages != null)
                        {
                            if (refundTrans)
                            {
                                orderTransaction.Name = "Refund: " + salesOrder.PaidAmount.Value;
                            }
                            else
                            {
                                orderTransaction.Name = "Void: " + salesOrder.PaidAmount.Value;
                            }
                            orderTransaction.TransactionId = response.transactionResponse.transId;
                            orderTransaction.TransactionAmount = salesOrder.PaidAmount.Value;
                            orderTransaction.TransactionType = TransactionType.Refund;
                            orderTransaction.TransactionComments = ConvertToXML(response);
                            orderTransaction.Id = Guid.Empty;
                            orderTransaction.Id = _OrderTransactionRepository.CreateOrderTransaction(orderTransaction);
                            paymentResponse.OK = true;
                            paymentResponse.Message = response.transactionResponse.messages[0].description;

                        }
                        else
                        {
                            paymentResponse.OK = false;
                            if (response.transactionResponse.errors != null)
                            {
                                paymentResponse.Message = response.transactionResponse.errors[0].errorText;
                            }
                        }
                    }
                    else
                    {
                        if (!refundTrans)
                        {
                            paymentResponse = Refund(salesOrder, true);
                        }
                        else
                        {
                            paymentResponse.OK = false;
                            if (response.transactionResponse != null && response.transactionResponse.errors != null)
                            {
                                paymentResponse.Message = response.transactionResponse.errors[0].errorText;

                                Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                                Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                            }
                            else
                            {
                                paymentResponse.Message = response.messages.message[0].text;
                            }
                        }

                    }
                }
                else
                {
                    paymentResponse.OK = false;
                }

                return paymentResponse;
            }
            else
            {
                paymentResponse.OK = false;
                paymentResponse.Message = "";
                return paymentResponse;
            }
        }

        public string ConvertToXML(AuthorizeNet.Api.Contracts.V1.ANetApiResponse response)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(AuthorizeNet.Api.Contracts.V1.createTransactionResponse));
            StringWriter sww = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(sww))
            {
                xsSubmit.Serialize(writer, response);
                var xml = sww.ToString(); // Your XML
                return xml;
            }
        }

    }
}
