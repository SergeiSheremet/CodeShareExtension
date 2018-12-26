using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CodeShare.SocialNetwork
{
    interface ISocialNetwork
    {
        bool IsAuthorized { get; }
        bool LogIn();
        void LogOut();
        void SendUrl(string url);
    }
}
