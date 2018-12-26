using CodeShare.Model;
using Microsoft.VisualStudio.TextManager.Interop;

namespace CodeShare.Hosting
{
    interface ICodeHosting
    {
        bool LogIn();
        void LogOut();
        string CreatePaste(TextViewSelection textSelection);
    }
}
