using HighlyDeveloped.Core.ViewModel;
using System.Web.Mvc;
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
            return null;
        }
    }
}
