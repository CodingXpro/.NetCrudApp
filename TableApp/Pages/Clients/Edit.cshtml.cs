using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace TableApp.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientinfo=new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
                using(SqlConnection connection= new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients where id=@id";
                    using (SqlCommand command = new SqlCommand(sql,connection)) { 
                    
                        command.Parameters.AddWithValue("@id", id);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientinfo.id = "" + reader.GetInt32(0);
                                clientinfo.name=reader.GetString(1);
                                clientinfo.email=reader.GetString(2);
                                clientinfo.phone=reader.GetString(3);
                                clientinfo.address=reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

        }
        public void OnPost()
        {
              
            clientinfo.id = Request.Form["id"];
            clientinfo.name = Request.Form["name"];
            clientinfo.email = Request.Form["email"];
            clientinfo.address = Request.Form["address"];
            clientinfo.phone = Request.Form["phone"];

            if (clientinfo.name.Length == 0 || clientinfo.id.Length == 0 || clientinfo.email.Length==0
                || clientinfo.address.Length==0 || clientinfo.phone.Length==0) {


                errorMessage = "All Fields are required";
                return;
            }
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE clients"+"SET name=@name,email=@email,phone=@phone,address=@address"+
                        "WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientinfo.name);
                        command.Parameters.AddWithValue("@id", clientinfo.id);
                        command.Parameters.AddWithValue("@email",clientinfo.email);
                        command.Parameters.AddWithValue("@phone", clientinfo.phone);
                        command.Parameters.AddWithValue("@address", clientinfo.address);

                        command.ExecuteNonQuery();
                        
                    }
                }

            }
            catch(Exception ex)
            {
                errorMessage=ex.Message;
            }

            Response.Redirect("/Clients/index");

        }
    }
}
