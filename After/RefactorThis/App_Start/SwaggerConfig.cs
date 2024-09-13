using Swashbuckle.Application;
using System.Web.Http;

namespace refactor_this
{
    internal class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Product API");
                    c.IncludeXmlComments(GetXmlCommentsPath());


                })
                .EnableSwaggerUi();

        }

        protected static string GetXmlCommentsPath()
        {
            return string.Format(@"{0}\bin\refactor-this.xml", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}