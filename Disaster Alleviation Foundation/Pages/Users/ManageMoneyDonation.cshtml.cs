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
    public class ManageMoneyDonationModel : PageModel
    {
        public funds funds = new funds();
        public string errorMessage = "";
        public string successMessage = "";
        public string name = "";
        public void OnGet()
        {
            try
            {
                var role = JsonConvert.DeserializeObject(HttpContext.Session.GetString("role"));
                

                if (role.ToString().Equals("admin")|| role.ToString().Equals("Money"))
                {
                    var user = JsonConvert.DeserializeObject(HttpContext.Session.GetString("name"));

                    name = user.ToString();
                }

                else
                {
                    Response.Redirect("/Users/Index");
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

            funds.id = Request.Form["id"];
            funds.name = Request.Form["name"];
            funds.date = DateTime.Today.ToString();
            funds.Amount = Request.Form["amount"];

            if (funds.Amount.Length == 0 || funds.date.Length == 0 || funds.name.Length == 0 || funds.id.Length == 0) 
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
                    string sql = "INSERT INTO tblMoney" + "(id,name,date,amount) VALUES " +
                        "(@id,@name,@date,@amount);";
                    
                    using (SqlCommand command = new SqlCommand(sql,connection))
                    {
                        command.Parameters.AddWithValue("@id", funds.id);
                        command.Parameters.AddWithValue("@name", funds.name);
                        command.Parameters.AddWithValue("@date", funds.date);
                        command.Parameters.AddWithValue("@amount", funds.Amount);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
                return;
            }

            funds.id = funds.name = funds.Amount = funds.date = "";
            successMessage = "Added Successfully!!";

            Response.Redirect("/Users/Index");


        }
    }
}
