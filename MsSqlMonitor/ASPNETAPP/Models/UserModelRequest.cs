using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETAPP.Models
{
    public class UserModelRequest
    {
        public User User { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
