using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Disaster_Alleviation_Foundation.Pages
{
    public class IndexModel : PageModel
    {
        public static bool IsLoggedIn = false;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public string Username = "";
        public string Password = "";
        public string errorMessage = "";
        public void OnGet()
        {

        }

        public void OnPost() 
        {

            Username = Request.Form["username"];
            Password = Request.Form["password"];

            if (Username.Length == 0 || Password.Length == 0) 
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                String conn = "Data Source=austin.database.windows.net;Initial Catalog=AlleviationDB;User ID=Reaperfam!;Password=Palitladi!";
                using (SqlConnection connection = new SqlConnection(conn))
                {

                    connection.Open();
                    string sql = "SELECT * FROM Usertbl WHERE password=@id AND username=@username;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id",Password);
                        command.Parameters.AddWithValue("@username", Username);
                       
                        using(SqlDataReader reader = command.ExecuteReader()) 
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read()) 
                                {
                                    string role = reader.GetString(2);
                                    HttpContext.Session.SetString("role",JsonConvert.SerializeObject(role));
                                    HttpContext.Session.SetString("name", JsonConvert.SerializeObject(reader.GetString(1)));
                                }
                                Response.Redirect("/Users/Index");
                                }
                            else 
                            {
                                errorMessage = "Incorrect Credentials Enter Correct Usename And Password";
                            }
                            
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;

            }

            
        }
    }
}
