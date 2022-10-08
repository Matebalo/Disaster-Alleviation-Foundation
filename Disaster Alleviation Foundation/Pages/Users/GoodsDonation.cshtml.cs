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
    public class GoodsDonationModel : PageModel
    {
        public List<Goods> goodslist = new List<Goods>();
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
                    String sql = "SELECT * FROM tblGoods";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            

                            while (reader.Read()) 
                            {
                                Goods goods = new Goods();
                                goods.id = reader.GetInt32(0);
                                goods.date = reader.GetString(1);
                                goods.itemNo = reader.GetInt32(2);
                                goods.cat = reader.GetString(3);
                                goods.description = reader.GetString(4);
                                goods.name = reader.GetString(5);

                                goodslist.Add(goods);
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

    public class Goods 
    {

        public int id;
        public string date;
        public int itemNo;
        public string cat;
        public string description;
        public string name;


    }
}
