using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestOnlineBase.Enum;

namespace TestOnline.Object
{
    public class ResultObject
    {
        public Enums.StatusCode StatusCode { get; set; }

        public string Message { get; set; }

        public object Result { get; set; }
    }
}
