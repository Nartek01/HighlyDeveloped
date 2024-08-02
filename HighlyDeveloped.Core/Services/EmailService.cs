using HighlyDeveloped.Core.Interfaces;
using HighlyDeveloped.Core.ViewModel;
using System;
using System.Linq;
using System.Net.Mail;
using Umbraco.Web;
using Umbraco.Core.Logging;
using HighlyDeveloped.Core.Controllers;


namespace HighlyDeveloped.Core.Services
{
    public class EmailService : IEmailService
    {

        private UmbracoHelper _umbraco; // Inject UmbracoHelper into my custom EmailService.
        private ILogger _logger;


        public EmailService(UmbracoHelper umbraco, ILogger logger)
        {
            _umbraco = umbraco;
            _logger = logger;
        }
        public void SendContactNotificationToAdmin(ContactFormViewModel viewModel)
        {
            // Read the email from and to addresses
            // Get the site settings

            var siteSettings = _umbraco.ContentAtRoot().DescendantsOrSelfOfType("siteSettings").FirstOrDefault();
            if (siteSettings == null)
            {
                throw new Exception("There are no site settings");
            }

            var fromAddress = siteSettings.Value<string>("emailSettingsFromAddress");
            var toAddresses = siteSettings.Value<string>("emailSettingsAdminAccounts");

            if (string.IsNullOrEmpty(fromAddress))
            {
                throw new Exception("There needs to be a from address in the site settings");
            }

            if (string.IsNullOrEmpty(toAddresses))
            {
                throw new Exception("There needs to be a to address in the site settings");
            }

            // Construct the email
            var emailSubject = "There has been  a contact form submitted";
            var emailBody = $"A new contact form has been received from {viewModel.Name}";
            var smtpMessage = new MailMessage();
            smtpMessage.Subject = emailSubject;
            smtpMessage.Body = emailBody;
            smtpMessage.From = new MailAddress(fromAddress);

            var toList = toAddresses.Split(',');
            foreach (var item in toList)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    smtpMessage.To.Add(item);
                }
            }
            // send via email services
            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(smtpMessage);
                }
            }
            catch (Exception Error)
            {
                _logger.Error<ContactController>("Failed to send email", Error);
                // Handle the error appropriately
            }
        }
    }
}
