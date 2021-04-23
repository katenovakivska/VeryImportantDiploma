using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagment
{
    public class JwtAuthOptions
    {
        private const string SECURITY_KEY = "super_secure_key_of_auction_application";

        public const int LIFETIME_MINUTES = 60;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECURITY_KEY));
        }
        
    }
}
