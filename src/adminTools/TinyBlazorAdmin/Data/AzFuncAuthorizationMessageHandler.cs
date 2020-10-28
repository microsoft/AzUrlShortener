using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;

namespace TinyBlazorAdmin.Data
{
    /// <summary>
    /// Handler to ensure proper credentials are sent with requests.
    /// </summary>
    public class AzFuncAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public string Endpoint { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="AzFuncAuthorizationMessageHandler"/>
        /// class.
        /// </summary>
        /// <param name="config"><see cref="IConfiguration"/> to access endpoint.</param>
        /// <param name="provider"><see cref="IAccessTokenProvider"/> service.</param>
        /// <param name="navigation"><see cref="NavigationManager"/> to navigate based on authentication.</param>
        public AzFuncAuthorizationMessageHandler(
            IConfiguration config,
            IAccessTokenProvider provider, 
            NavigationManager navigation) : base(provider, navigation)
        {
            var section = config.GetSection(nameof(UrlShortenerSecuredService));
            Endpoint = section.GetValue<string>(nameof(Endpoint));
            ConfigureHandler(new[] { Endpoint });
        }
    }
}