using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShare.SocialNetwork
{
    interface ISocialNetwork
    {
        void LogIn(string login, string password);
        void LogOut();
        void SendUrl(string url);
    }
}
