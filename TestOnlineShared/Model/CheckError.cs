using System;
using System.Collections.Generic;
using System.Text;

namespace TestOnlineShared.Model
{
    public class CheckError 
    {
        public bool IsError { get; set; }
        public string Code { get; set; }
        public Exception Exception { get; set; }
        public string Message { get; set; }
    }
}
