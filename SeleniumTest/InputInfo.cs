using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTest
{
    class InputInfo
    {
        public string email { get; set; }
        public string password { get; set; }
        public string recoverEmail { get; set; }

        public InputInfo(string email, string password, string recoverEmail)
        {
            this.email = email;
            this.password = password;
            this.recoverEmail = recoverEmail;
        }
    }
}
