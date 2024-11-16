using FinalProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FinalProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FinalProject.Areas.Identity.Pages.Account
{
    public class ViewEmailModel : PageModel
    {
        public string EmailSubject { get; set; }
        public string EmailSender { get; set; }
        public string EmailDate { get; set; }
        public string EmailMessage { get; set; }

        private readonly string connectionString = "Server=tcp:inventory-cs436.database.windows.net,1433;Initial Catalog=inventory;Persist Security Info=False;User ID=startos;Password=Star7788;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public async Task OnGet(int emailid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // SQL query to get the email by its ID
                    string sql = "SELECT * FROM Emails WHERE Id = @EmailID";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@EmailID", emailid);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                EmailSubject = reader.GetString(2);
                                EmailSender = reader.GetString(5);
                                EmailDate = reader.GetDateTime(4).ToString("yyyy-MM-dd HH:mm:ss");
                                EmailMessage = reader.GetString(3);
                            }
                            else
                            {
                                // Handle the case where the email wasn't found
                                EmailSubject = "Email not found";
                                EmailMessage = "The email you're looking for could not be found.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                EmailSubject = "Error";
                EmailMessage = "There was an error retrieving the email.";
            }
        }
    }
}
