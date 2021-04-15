using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Dapper.Samples.Advanced
{
    public class Utils
    {
        private static string connectionString = getConnectionString();

        public static void errorPage(HttpContext ctx, string page){
            ctx.Response.Write("<H1>An Error occurred</H1>");
            ctx.Response.Write("<p>Details:</p>");
            ctx.Response.Write(
                "The page \"" + page + "\" was not found.");
        }

        public static void frontPage(HttpContext ctx){
            ctx.Response.Write("<H1>This is an example application</H1>");
            ctx.Response.Write("<H1>This is an HttpHandler Test.</H1>");
            ctx.Response.Write("<p>Your Browser:</p>");
            ctx.Response.Write("Type: " + ctx.Request.Browser.Type + "<br>");
            ctx.Response.Write("Version: " + ctx.Request.Browser.Version);
        }

        public static DataSet GetDataSetByCategory(string category)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query1 = "SELECT ITEM,PRICE FROM PRODUCT WHERE ITEM_CATEGORY='"
                  + category + "' ORDER BY PRICE";
                var adapter = new SqlDataAdapter(query1, connection);
                var result = new DataSet();
                adapter.Fill(result);
                return result;
            }
        }

        public static DataSet GetUserId(string username)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query1 = "SELECT USERID FROM USERS WHERE USERNAME='"
                  + username;
                var adapter = new SqlDataAdapter(query1, connection);
                var result = new DataSet();
                adapter.Fill(result);
                return result;
            }
        }

        public static string getConnectionString(){
            return
                "Data Source=.SQLExpress;" +
                "User Instance=true;" +
                "Integrated Security=true;" +
                "AttachDbFilename=|DataDirectory|DataBaseName.mdf;";
        }
    }
}
