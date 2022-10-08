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
    public class disasterModel : PageModel
    {
        public List<Disaster> disasters = new List<Disaster>();
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
                    String sql = "SELECT * FROM tbldisaster";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                           
                            while (reader.Read())
                            {
                                Disaster d = new Disaster();
                                d.id = reader.GetInt32(0);
                                d.startdate = reader.GetString(1);
                                d.end = reader.GetString(2);
                                d.location = reader.GetString(3);
                                d.description = reader.GetString(4);
                                d.aid = reader.GetString(5);
                                disasters.Add(d);
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

    public class Disaster
        {
        public int id;
        public string startdate;
        public string end;
        public string location;
        public string description;
        public string aid;

}
}
