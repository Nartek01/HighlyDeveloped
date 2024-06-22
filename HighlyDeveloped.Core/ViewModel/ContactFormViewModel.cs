/** 
 * ContactFormViewModel.cs
 * 
 * This is a C# class that represents the data model for the Contact Form.cshtml. It defines the properties (fields) that the form will have, such as "Name", "EmailAddress", "Comment and "Subject" all are properties of (?)
 * 
 * ContactFormViewModel.cs also includes "data annotations" such as [Required] or [MaxLength(...)]
 */
using System.ComponentModel.DataAnnotations;

namespace HighlyDeveloped.Core.ViewModel
{
    public class ContactFormViewModel
    {
        [Required] // These are called data annotations
        [MaxLength(80, ErrorMessage = "Please try and limit to 80 characters")]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email adress")]
        public string EmailAddress { get; set; }
        [Required]
        [MaxLength(500, ErrorMessage = "Comments max length 500 characters")]
        public string Comment { get; set; }
        [MaxLength(255, ErrorMessage = "Please try and limit to 255 characters")]
        public string Subject { get; set; }
        public string ReCaptchaSiteKey { get; set; }
    }
}
