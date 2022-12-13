using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.SharePoint.Client;
using onlygodknows.Models;

namespace onlygodknows.Controllers
{
    public class sharepointtestController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();
        // GET: sharepointtest
        public async Task<ActionResult> Index()
        {
            var context = new ClientContext("https://citiscapegroupae.sharepoint.com/sites/sneden/");
            string inputString = "Xuw64016";

            SecureString pass = new SecureString();
            foreach (char c in inputString)
            {
                pass.AppendChar(c);
            }
            context.Credentials = new SharePointOnlineCredentials("sdiniz@citiscapegroup.com", pass);
            // Get a reference to the list
            var list = context.Web.Lists.GetByTitle("sharepointatt");

            // Query the list for items
            // var query = CamlQuery.CreateAllItemsQuery();
            var query = new CamlQuery();
            query.ViewXml = "<View><Query><Where><Eq><FieldRef Name='TMonth'/><Value Type='DateTime'>" +
                            DateTime.Now.ToString("yyyy-MM-dd") + "</Value></Eq></Where></Query></View>";
            var items = list.GetItems(query);

            // Load the items into the context and execute the query
            context.Load(items);
            await context.ExecuteQueryAsync();
            // Iterate through the items and get the title of each item
            
            foreach (var item in items)
            {
                var tests = new sharepointatt();
                // Ensure that the ListItem object is initialized before trying to access its properties
                context.Load(item);
                await context.ExecuteQueryAsync();

                // Now you can access the Title property of the ListItem object
                var titleValue = item["TMonth"];
                DateTime.TryParse(titleValue.ToString(),out var datesp );
                tests.TMonth = datesp;
                titleValue = item["ManPowerSupplier"];
                int.TryParse(titleValue.ToString(), out var temp);
                tests.ManPowerSupplier = temp;
                tests.Project = 456;
                tests.EmpID = 6435;
                db.sharepointatts.Add(tests);
                db.SaveChanges();

            }
            
            return View();
        }
        
    }
}
