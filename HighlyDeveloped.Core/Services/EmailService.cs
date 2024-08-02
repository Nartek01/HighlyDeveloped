using HighlyDeveloped.Core.Interfaces;
using HighlyDeveloped.Core.ViewModel;
using System;
using System.Linq;
using System.Net.Mail;
using Umbraco.Web;

namespace HighlyDeveloped.Core.Services
{
    public class EmailService : IEmailService
    {

        private UmbracoHelper _umbraco; // Inject UmbracoHelper into my custom EmailService.

        public EmailService(UmbracoHelper umbraco)
        {
            _umbraco = umbraco;
        }
            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(smtpMessage);
                }
            }
            catch (Exception Error)
            {
                Logger.Error<ContactController>("Failed to send email", Error);
                // Handle the error appropriately
            }
    }
}
