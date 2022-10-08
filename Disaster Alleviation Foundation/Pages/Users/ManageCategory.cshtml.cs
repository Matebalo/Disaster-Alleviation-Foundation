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
    public class ManageCategoryModel : PageModel
    {
        public Cat cat = new Cat();
        public string errorMessage = "";
        public string successMessage = "";
        public string name = "";
        public void OnGet()
        {
            try
            {
                var role = JsonConvert.DeserializeObject(HttpContext.Session.GetString("role"));
                if (role.ToString().Equals("admin") || role.ToString().Equals("Category"))
                {
                    var user = JsonConvert.DeserializeObject(HttpContext.Session.GetString("name"));

                    name = user.ToString();
                }
                else
                {
                    Response.Redirect("/Users/categories");
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

            cat.id = Convert.ToInt32(Request.Form["id"]);
            cat.name = Request.Form["category"];

            if(cat.id.ToString().Length==0||cat.name.Length==0)
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
                    string sql = "INSERT INTO tblCat" + "(id,name) VALUES " +
                        "(@id,@name);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", cat.id);
                        command.Parameters.AddWithValue("@name", cat.name);
 
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

            Response.Redirect("/Users/categories");
        }
    }
}
