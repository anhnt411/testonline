using System;
using System.Collections.Generic;
using System.Text;

namespace TestOnlineBase.Helper
{
    public class SendEmailResponse
    {
        public bool Successful => ErrorMsg == null;
        public string ErrorMsg { get; set; }
    }
}
