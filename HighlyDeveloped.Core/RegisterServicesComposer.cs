using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using HighlyDeveloped.Core.Services;
using HighlyDeveloped.Core.Interfaces;

namespace HighlyDeveloped.Core
{
    public class RegisterServicesComposer : IUserComposer
    {
        // Create a public "compose" method
        public void Compose(Composition composition)
        {
            composition.Register<IEmailService, EmailService>(Lifetime.Request);
            composition.Register<ILogger>(factory => Current.Logger);
        }
    }
}
