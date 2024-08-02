using HighlyDeveloped.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Security;

namespace HighlyDeveloped.Core
{
    public class RegisterServicesComposer : IUserComposer
    {
        // Create a public "compose" method
        public void Compose(Composition composition)
        {
            composition.Register<IEmailService, EmailService>(Lifetime.Request);
        }
    }
}
