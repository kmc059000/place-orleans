using Place.WebApi.App_Start;
using System.Web.Http;

namespace Place.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            OrleansConfig.Register();
        }
    }
}
