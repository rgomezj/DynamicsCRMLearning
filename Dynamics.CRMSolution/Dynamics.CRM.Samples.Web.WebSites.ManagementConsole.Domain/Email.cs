using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Domain
{
  public   class Email
    {

        #region Properties

      public List<Contact> Contacts { get; set; }
      public SalesOrder SalesOrder { get; set; }
      public TypeOfTransaction TypeOfTransaction { get; set; }
        public Configuration Configuration { get; set; }

        #endregion
    }

  public enum TypeOfTransaction
  {
      TransferRegistration = 1,
      SwapRegistration = 2
  }
}
