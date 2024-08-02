using HighlyDeveloped.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighlyDeveloped.Core.Interfaces
{
    /// <summary>
    /// Handles all email operations
    /// </summary>
    public interface IEmailService
    {
        void SendContactNotificationToAdmin(ContactFormViewModel viewModel);
    }
}
