using AutoMapper;
using Pavliks.WAM.ManagementConsole.BL;
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Implementation;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using Pavliks.WAM.ManagementConsole.ManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Controllers
{
    public class SMSMessageController : ApiController
    {
        private SMSMessageBL _SMSMessageBL;

        public SMSMessageController(ISMSMessageRepository ISMSMessageRepository)
        {
            _SMSMessageBL = new SMSMessageBL(ISMSMessageRepository);
        }



        [System.Web.Http.HttpPost()]
        public HttpResponseMessage ProcessSMSMessage(SMSMessageViewModel smsMessageViewModel)
        {
            try
            {

                AutoMapper.MapperConfiguration config = new AutoMapper.MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<SMSMessageViewModel, SMSMessage>();
                });
                IMapper mapper = config.CreateMapper();
                var smsMessage = mapper.Map<SMSMessageViewModel, SMSMessage>(smsMessageViewModel);
                SMSMessage smsMessageComplete= _SMSMessageBL.GetSMSMessageByMessageId(smsMessage.MessageId);
                smsMessageComplete.MessageStatus = smsMessage.MessageStatus;

                _SMSMessageBL.SetStatus(smsMessageComplete);

                return Request.CreateResponse(HttpStatusCode.OK, new { Message = "SMS message was updated" });


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "An error occurred updating the sms message." + ex.Message });
            }
        }

    }
}