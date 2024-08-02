/** 
 * ContactController.cs
 * 
 * This is a C# controller class that handles the logic and flow of the Contact Form.cshtml, It has two methods (class functions)... RenderContactForm(): This method is responsible of creating an instance of the ContactFormViewModel and passing it to the Contact Form.cshtml view (view file), so that the view can render the form with the appropriate fields.
 * 
 * HandleContactForm(): This method is called when the user submits the form. It receives the form data (as an instance of ContactFormViewModel) and can perform any necessary actions, such as sending an email or saving the data to a database.
 */
using HighlyDeveloped.Core.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Mvc;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace HighlyDeveloped.Core.Controllers
{
    public class ContactController : SurfaceController
    {
        /// <summary>
        /// This is for all operations regarding the contact form.
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderContactForm()
        {
            var viewModel = new ContactFormViewModel();
            // Set reCaptcha Site Key
            var siteSettings = Umbraco.ContentAtRoot().DescendantsOrSelfOfType("siteSettings").FirstOrDefault();
            if (siteSettings != null)
            {
                var siteKey = siteSettings.Value<string>("reCaptchaSiteKey");
                viewModel.ReCaptchaSiteKey = siteKey;
            }
            // call $ @Html.Action("RenderContactForm", "Contact", new { params... });
            return PartialView("~/Views/Partials/Contact Form.cshtml", viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleContactForm(ContactFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Please check the form.");
                return CurrentUmbracoPage();
            }
            var siteSettings = Umbraco.ContentAtRoot().DescendantsOrSelfOfType("siteSettings").FirstOrDefault();
            if(siteSettings != null)
            {
                var secretKey = siteSettings.Value<string>("reCaptchaSecretKey");
                var captchaToken = Request.Form["GoogleCaptchaToken"];
                var isCaptchaValid = IsCaptchaValid(captchaToken, secretKey);
                if (!isCaptchaValid)
                {
                    ModelState.AddModelError("Captcha", "The captcha is not valid, please try again later...");
                    return CurrentUmbracoPage();
                }
            }
            try
            {
                // Create a new contact form in umbraco
                // Get a handle to Contact Forms
                var contactForms = Umbraco.ContentAtRoot().DescendantsOrSelfOfType("contactForms").FirstOrDefault();
                if (contactForms != null)
                {
                    var newContact = Services.ContentService.Create(viewModel.Name, contactForms.Id, "contactForm");
                    newContact.SetValue("contactName", viewModel.Name);
                    newContact.SetValue("contactEmail", viewModel.EmailAddress);
                    newContact.SetValue("contactSubject", viewModel.Subject);
                    newContact.SetValue("contactComment", viewModel.Comment);
                    Services.ContentService.SaveAndPublish(newContact);
                }
                // Send out an email to the site admin
                //SendContactFormReceivedEmail(viewModel);
                _emailService.SendContactNotificationToAdmin(viewmodel);

                // Return confirmation message to the user
                TempData["status"] = "OK"; // This is necessary since we have an conditional that listens to this value on Contact Form.cshtml

                return RedirectToCurrentUmbracoPage();

            }
            catch (Exception Error)
            {
                Logger.Error<ContactController>("Something went wrong", Error.Message);
                ModelState.AddModelError("Error", "Sorry there was problem submitting your email, please try again later.");
            }

            return CurrentUmbracoPage();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="secretKey"></param>
        /// <returns>true | false </returns>
        private bool IsCaptchaValid(string token, string secretKey)
        {
            // Sending token to google api
            HttpClient httpClient = new HttpClient();
            var response = httpClient
                .GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={token}")
                .Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return false;
            // get response
            string jsonResponse = response.Content.ReadAsStringAsync().Result;
            dynamic jsonData = JObject.Parse(jsonResponse);
            if (jsonData.success != "true")
            {
                return false;
            }
            // was good ?
            return true;

        }

        private void SendContactFormReceivedEmail(ContactFormViewModel viewModel)
        {
            //Summary
            // This will send out an email to site admins.
            //Summary


            // Read the email from and to addresses
            // Get the site settings

            var siteSettings = Umbraco.ContentAtRoot().DescendantsOrSelfOfType("siteSettings").FirstOrDefault();
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
                Logger.Error<ContactController>("Failed to send email", Error);
                // Handle the error appropriately
            }
        }
    }
}
