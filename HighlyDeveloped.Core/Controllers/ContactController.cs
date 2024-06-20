/** 
 * ContactController.cs
 * 
 * This is a C# controller class that handles the logic and flow of the Contact Form.cshtml, It has two methods (class functions)... RenderContactForm(): This method is responsible of creating an instance of the ContactFormViewModel and passing it to the Contact Form.cshtml view (view file), so that the view can render the form with the appropriate fields.
 * 
 * HandleContactForm(): This method is called when the user submits the form. It receives the form data (as an instance of ContactFormViewModel) and can perform any necessary actions, such as sending an email or saving the data to a database.
 */
using HighlyDeveloped.Core.ViewModel;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace HighlyDeveloped.Core.Controllers
{
    public class ContactController : SurfaceController
    {
        //Summary
        // This is for all operations regarding the contact form.
        //Summary
        public ActionResult RenderContactForm()
        {
            var viewModel = new ContactFormViewModel();

            return PartialView("~/Views/Partials/Contact Form.cshtml", viewModel);
            // call $ @Html.Action("RenderContactForm", "Contact", new { params... });
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
            // Return confirmation message to the user
            TempData["status"] = "OK";
            return RedirectToCurrentUmbracoPage();
        }
    }
}
