using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace Disaster_Alleviation_Foundation.Pages.Users
{
    public class categoriesModel : PageModel
    {
      public  List<Cat> catlist = new List<Cat>();
        public string errMessage = "";
        public string name = "";

        public void OnGet()
        {
            try
            {
                var role = JsonConvert.DeserializeObject(HttpContext.Session.GetString("role"));
                var user = JsonConvert.DeserializeObject(HttpContext.Session.GetString("name"));

                name = user.ToString();
                try
                {
                    var access = JsonConvert.DeserializeObject(HttpContext.Session.GetString("access"));
                    if (access.ToString().Equals("denied"))
                    {
                        errMessage = "User Does Not Have Privileges to Access Page ";
                    }

                }
                catch (Exception error)
                {


                }

            }
            catch (Exception e)
            {

                Response.Redirect("/Index");
            }

            try
            {
                String connectiontring = "Data Source=austin.database.windows.net;Initial Catalog=AlleviationDB;User ID=Reaperfam!;Password=Palitladi!";


                using (SqlConnection connection = new SqlConnection(connectiontring))
                {
                    connection.Open();
                    String sql = "SELECT * FROM tblCat";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                         

                            while (reader.Read()) 
                            {
                                Cat cats = new Cat();
                                cats.id = reader.GetInt32(0);
                                cats.name = reader.GetString(1);

                                catlist.Add(cats);
                            }
                            
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
            }

        }
    }

    public class Cat 
    {
        public int id;
        public string name;
    }
}
