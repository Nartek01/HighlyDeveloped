using HighlyDeveloped.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighlyDeveloped.Core.Services
{
    public class EmailService : IEmailService
    {

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
