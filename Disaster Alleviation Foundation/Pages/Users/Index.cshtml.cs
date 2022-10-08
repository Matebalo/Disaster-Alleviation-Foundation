using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Session;

namespace Disaster_Alleviation_Foundation.Pages.Users
{
   
    public class IndexModel : PageModel
    {
        public List<funds> fundslist= new List<funds>();
        public string errMessage = "";
        public string name = "";

        public void OnGet()
        {
            try
            {
                var role = JsonConvert.DeserializeObject(HttpContext.Session.GetString("role"));
                var user = JsonConvert.DeserializeObject(HttpContext.Session.GetString("name"));
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
             

                

                name = user.ToString();
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
                    String sql = "SELECT * FROM tblMoney";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader()) 
                        {

                           
                            while (reader.Read()) 
                            {
                                funds funds = new funds();
                                funds.id = "" + reader.GetInt32(0);
                                funds.date = reader.GetString(1);
                                funds.Amount = "" + reader.GetDecimal(2);
                                funds.name = reader.GetString(3);

                                fundslist.Add(funds);
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

    public class funds
    {
        public String id;
        public String date;
        public String Amount;
        public String name;

    }

}
