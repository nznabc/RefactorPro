using System.Web.Http;

namespace refactor_this
{
    /// <summary>
    /// Represents the entry point of the Web API application.
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Initializes the application.
        /// </summary>
        protected void Application_Start()
        {
            UnityConfig.RegisterComponents();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            SwaggerConfig.Register();
        }
    }
}
