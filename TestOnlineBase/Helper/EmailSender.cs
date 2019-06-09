using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.SmtpApi;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TestOnlineBase.EmailHelper;

namespace TestOnlineBase.Helper
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private ILogger<EmailSender> _logger;
        public EmailSender(IOptions<EmailSettings> emailSettings, ILogger<EmailSender> logger)
        {
            this._logger = logger;
            this._emailSettings = emailSettings.Value;
            
        }

       

        public async Task  SendEmailAsync(string userEmail,string emailSubject,string message)
        {
            try
            {
                // Credentials
                var credentials = new NetworkCredential(_emailSettings.Sender, _emailSettings.Password);

                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.Sender, _emailSettings.SenderName),
                    Subject = emailSubject,
                    Body = message,
                    IsBodyHtml = true
                };

                mail.To.Add(new MailAddress(userEmail));

                // Smtp client
                var client = new SmtpClient()
                {
                    Port = _emailSettings.MailPort,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = _emailSettings.MailServer,
                    EnableSsl = true,
                    Credentials = credentials
                };

                // Send it...         
                client.Send(mail);
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                _logger.LogError(ex, ex.Message);
            }

        }

      
        

    }
}
