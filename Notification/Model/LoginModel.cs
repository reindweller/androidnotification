using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Notification.Model
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
        public int Role { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Id1 { get; set; }
        public string Id2 { get; set; }
        public bool TermsOfAgreement { get; set; }
    }


    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }
        
        public bool RememberMe { get; set; }
    }
}