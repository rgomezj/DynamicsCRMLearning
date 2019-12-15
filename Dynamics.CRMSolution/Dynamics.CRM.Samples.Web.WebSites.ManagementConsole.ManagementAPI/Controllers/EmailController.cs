using AutoMapper;
using Pavliks.WAM.ManagementConsole.BL;
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using Pavliks.WAM.ManagementConsole.ManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Controllers
{
    public class EmailController : ApiController
    {
        private EmailBL _EmailBL;

        public EmailController(IEmailRepository _emailRepository, IConfigurationRepository _IConfigurationRepository)
        {
            _EmailBL = new EmailBL(_emailRepository, _IConfigurationRepository);
        }

        [System.Web.Http.HttpPost]
        public bool SendsEmailReceipt(EmailViewModel emailViewModel)
        {
            #region convertion
            //It converts the Registration domain record to Registration Index ViewModel
            AutoMapper.MapperConfiguration config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EmailViewModel, Email>();
            });
            IMapper mapper = config.CreateMapper();
            var salesOrderViewModel = mapper.Map<EmailViewModel, Email>(emailViewModel);
            ;
            #endregion
            return _EmailBL.SendEmailReceipt(salesOrderViewModel);
        }
        [System.Web.Http.HttpGet]
        public string ReceiptReport(string id)
        {
            try
            {
                //Gets the report.
                byte[] report = _EmailBL.ReceiptReport(id);
                //Creates the path where the report will be save. The report will be saved in the PdfReceipt  folder.
                string path = HttpContext.Current.Server.MapPath("~/")+ "PdfReceipt\\" + id + ".pdf";
                //The report is saved as pdf.
                File.WriteAllBytes(path, report);
                //Gets the url.
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                char[] delimiterChars = { '/' };
                string []urlParts = url.Split(delimiterChars);

                string finalUrl=string.Empty;
                //Creates the final URL, with that final ulr the pdf will be rendered in the front end.
                foreach (var part in urlParts)
                {
                    if(!part.Equals("api"))
                    {
                        finalUrl += part+"/";
                    }else
                    {
                        break;
                    }
                    
                    
                }

                return finalUrl+"PdfReceipt\\" + id + ".pdf";
            }catch
            {

                return null;
            }
        }
    }
}
