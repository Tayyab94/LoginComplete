using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginComplete.Models
{
    public class SiteUser
    {
        public int ID { get; set; }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

       
        public bool IsValid { get; set; }


    }
}