using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeUsuarios
{
    class SystemSupportMail: MasterMailServer
    {
        public SystemSupportMail()
        {
            senderMail = "soportesandsys@gmail.com";
            password = "pablo123456";
            host = "smtp.gmail.com";
            port = 587;
            ssl = true;
            initializeSmtpClient();
        }
    }
}
