using CodeShare.Model;
using Microsoft.VisualStudio.TextManager.Interop;

namespace CodeShare.Hosting
{
    interface ICodeHosting
    {
        void LogIn(string login, string password);
        void LogOut();
        string CreatePaste(TextViewSelection textSelection);
    }
}
