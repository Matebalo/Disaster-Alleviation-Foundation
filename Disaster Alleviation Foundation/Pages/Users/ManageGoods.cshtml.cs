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
   
    public class ManageGoodsModel : PageModel
    {
        public Goods goods = new Goods();
        public List<Cat> cat = new List<Cat>();
        public string errorMessage = "";
        public string successMessage = "";
        public string name = "";
        public void OnGet()
        {
            try
            {
                var role = JsonConvert.DeserializeObject(HttpContext.Session.GetString("role"));
                if (role.ToString().Equals( "admin") || role.ToString().Equals( "Goods"))
                {
                    var user = JsonConvert.DeserializeObject(HttpContext.Session.GetString("name"));

                    name = user.ToString();

                }

                else
                {
                    Response.Redirect("/Users/GoodsDonation");
                    HttpContext.Session.SetString("access", JsonConvert.SerializeObject("denied"));

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

                                cat.Add(cats);
                            }

                        }
                    }

                }

            }
            catch (Exception ex)
            {
               errorMessage= ex.Message;
            }


        }

        public void OnPost() 
        {

            goods.id = Convert.ToInt32(Request.Form["id"]);
            goods.date = DateTime.Today.ToString();
            goods.description = Request.Form["description"];
            goods.cat = Request.Form["category"];
            goods.itemNo = Convert.ToInt32(Request.Form["item"]);
            goods.name = Request.Form["donor"];

            if(goods.id.ToString().Length==0|| goods.description.Length==0 || goods.cat.Length==0 || goods.itemNo.ToString().Length==0)
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
                    string sql = "INSERT INTO tblGoods" + "(id,date,item_no,category,description,name) VALUES " +
                        "(@id,@date,@itemNo,@cat,@description,@name);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", goods.id);
                        command.Parameters.AddWithValue("@date", goods.date);
                        command.Parameters.AddWithValue("@itemNo", goods.itemNo);
                        command.Parameters.AddWithValue("@cat", goods.cat);
                        command.Parameters.AddWithValue("@description", goods.description);
                        command.Parameters.AddWithValue("@name", goods.name);

                        command.ExecuteNonQuery();
                    }

                }


            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
                return;
            }

             goods.description = goods.cat = goods.name = "";
            successMessage = "Added Successfully!!";

            Response.Redirect("/Users/GoodsDonation");
        }
    }
}
