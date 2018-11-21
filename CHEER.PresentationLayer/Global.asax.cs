using System;
using System.Web;
using System.Web.Routing;
using System.Web.Compilation;
using System.Web.Http;
using CHEER.PresentationLayer;
using System.Data;
using CHEER.PresentationLayer.CommonUse;
using CHEER.PresentationLayer.App_Start;

namespace GlobalInit
{
    public static class GolbalLockKey
    {
        public static readonly object lockKey = new object();
    }

    //public class Global : HttpApplication
    //{
    //    protected void Application_Start(object sender, EventArgs e)
    //    {
    //        GlobalConfiguration.Configure(WebApiConfig.Register);
    //        GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
    //    }

    //    public const string SYNCAPPLICATION = "SYNCAPPLICATION";
    //    public const string SYNCPRODUCT = "SYNCPRODUCT";
    //    public const string EXECSQL = "EXECSQL";
    //}

    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        public const string SYNCAPPLICATION = "SYNCAPPLICATION";
        public const string SYNCPRODUCT = "SYNCPRODUCT";
        public const string EXECSQL = "EXECSQL";

        protected void Application_BeginRequest()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS, PUT, PATCH, DELETE");
            Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, TokenAuthorization");
            if (Context.Request.HttpMethod == "OPTIONS")
            {
                Response.StatusCode = 204;
                Response.End();
            }
        }


    }
}



