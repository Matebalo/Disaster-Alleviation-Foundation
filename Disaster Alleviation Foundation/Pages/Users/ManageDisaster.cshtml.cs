using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;

namespace Disaster_Alleviation_Foundation.Pages.Users
{
    public class ManageDisasterModel : PageModel
    {
        public Disaster disaster = new Disaster();
        public string errorMessage = "";
        public string successMessage = "";
        public string name = "";
        public void OnGet()
        {
            try
            {
                var role = JsonConvert.DeserializeObject(HttpContext.Session.GetString("role"));

                if (role.ToString().Equals("admin")||role.ToString().Equals("Disaster"))
                {

                    var user = JsonConvert.DeserializeObject(HttpContext.Session.GetString("name"));

                    name = user.ToString();
                }
                else
                {
                    Response.Redirect("/Users/disaster");
                    HttpContext.Session.SetString("access", JsonConvert.SerializeObject("denied"));

                }

               
            }
            catch (Exception e)
            {

                Response.Redirect("/Index");
            }
          
        }

        public void OnPost()
        {
            disaster.id = Convert.ToInt32(Request.Form["id"]);
            disaster.startdate = Request.Form["start_date"];
            disaster.end = Request.Form["end_date"];
            disaster.description = Request.Form["description"];
            disaster.location = Request.Form["location"];
            disaster.aid = Request.Form["aid"];

            if(disaster.startdate.Length==0||disaster.end.Length==0||disaster.description.Length==0||disaster.location.Length==0||disaster.aid.Length==0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                String connectiontring = "Data Source=austin.database.windows.net;Initial Catalog=AlleviationDB;User ID=Reaperfam!;Password=Palitladi!";
                using (SqlConnection connection = new SqlConnection(connectiontring))
                {

                    connection.Open();
                    string sql = "INSERT INTO tbldisaster" + "(id,start_date,end_date,location,description,aid) VALUES " +
                        "(@id,@start,@end,@location,@description,@aid);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", disaster.id);
                        command.Parameters.AddWithValue("@start", disaster.startdate);
                        command.Parameters.AddWithValue("@end", disaster.end);
                        command.Parameters.AddWithValue("@location", disaster.location);
                        command.Parameters.AddWithValue("@description", disaster.description);
                        command.Parameters.AddWithValue("@aid", disaster.aid);

                        command.ExecuteNonQuery();
                    }

                }


            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            successMessage = "Added Successfully!!";

            Response.Redirect("/Users/disaster");
        }
    }
}
