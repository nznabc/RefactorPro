using refactor_this.DataAccess;
using refactor_this.Services;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace refactor_this
{
    internal static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<IProductOptionService, ProductOptionService>();
            container.RegisterType<IDataAccessLayer, DataAccessLayer>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}