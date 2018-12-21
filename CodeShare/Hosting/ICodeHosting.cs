namespace CodeShare.Hosting
{
    interface ICodeHosting
    {
        void LogIn(string login, string password);
        void LogOut();
        string CreatePaste(string code, string title = null);
    }
}
