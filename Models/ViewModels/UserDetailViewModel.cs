using System;
using System.Data;

namespace AspNetCore_SimpleLogin.Models.ViewModels
{
    public class UserDetailViewModel
    {
        public int Id { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public static UserDetailViewModel FromDataRow(DataRow userRow)
        {
            var userDetailViewModel = new UserDetailViewModel
            {
                Cognome = Convert.ToString(userRow["Cognome"]),
                Nome = Convert.ToString(userRow["Nome"]),
                Email = Convert.ToString(userRow["Email"]),
                Username = Convert.ToString(userRow["Username"]),
                Password = Convert.ToString(userRow["Password"]),
                Id = Convert.ToInt32(userRow["Id"])
            };
            
            return userDetailViewModel;
        }
    }
}