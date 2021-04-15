using System;
using System.Web;
using System.Data;

namespace Dapper.Samples.Advanced
{
    public class AdminPageHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext ctx)
        {
            Console.Write("Incoming Request For Admin page");
            string page = ctx.Request.Cookies["page"].Value;
            Utils.errorPage(ctx, page);
            Console.Write("Request Served");
        }

        public bool IsReusable{ get { return true; } }
    }

    public class FrontPageHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext ctx)
        {
            Console.Write("Incoming Request for front page");
            Utils.frontPage(ctx);
            Console.Write("Request Served");
        }

        public bool IsReusable{ get { return true; } }
    }

    public class UserIDHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext ctx)
        {
            string username = ctx.Request.Headers["username"];
            DataSet ds = Utils.GetUserId(username);
            string uid = (string)ds.Relations["Results"].ChildTable.Rows[0]["USERID"];
            ctx.Response.Write("<p>User ID: " + uid + "</p>");
        }

        public bool IsReusable{ get { return true; } }
    }

    public class CategoryHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext ctx)
        {
            string category = ctx.Request.QueryString["category"];
            string item = (string)Utils.GetDataSetByCategory(category).Relations["Results"].ChildTable.Rows[0]["ITEM"];
            ctx.Response.Write("<p>Item: " + item + "</p>");
        }

        public bool IsReusable{ get { return true; } }
    }
}
