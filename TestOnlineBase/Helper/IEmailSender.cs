using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestOnlineBase.Helper
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string userEmail, string emailSubject, string message);
    }
}
