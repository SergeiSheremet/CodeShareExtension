using CodeShare.Model;

namespace CodeShare.Hosting
{
    interface ICodeHosting
    {
        bool IsAuthorized { get; }
        bool LogIn();
        void LogOut();
        string CreatePaste(TextViewSelection textSelection);
    }
}
