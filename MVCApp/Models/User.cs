using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCApp
{
    public class User
    {
        private FClubEntities db = new FClubEntities();
        public static bool IsAutorized = false;
        public static string Role;
        public static string ProfileID;
    }
}
