using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using CodeShare.Hosting;
using CodeShare.Hosting.Implementation;
using CodeShare.Model;
using CodeShare.SocialNetwork;
using CodeShare.SocialNetwork.Implementation;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;
using Task = System.Threading.Tasks.Task;

namespace CodeShare
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class ShareCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("73742319-b6fc-4bda-8d77-f47630ce7ae1");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage _package;

        public ICodeHosting Hosting { get; }
        public ISocialNetwork SocialNetwork { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareCommand"/> class.
        /// Adds our co*-mmand handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private ShareCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            Hosting = new GithubGists();
            SocialNetwork = new Vk();

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static ShareCommand Instance { get; private set; }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider => _package;

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in ShareCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new ShareCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var textSelection = GetSelection(ServiceProvider);
            var url = Hosting.CreatePaste(textSelection);

            if (url == null)
            {
                return;
            }

            SocialNetwork.SendUrl(url);
        }

        private TextViewSelection GetSelection(IAsyncServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetServiceAsync(typeof(SVsTextManager)).Result;
            var textManager = service as IVsTextManager2;
            textManager.GetActiveView2(1, null, (uint)_VIEWFRAMETYPE.vftCodeWindow, out var view);

            view.GetSelection(out int startLine, out int startColumn, out int endLine, out int endColumn);
            var start = new TextViewPosition(startLine, startColumn);
            var end = new TextViewPosition(endLine, endColumn);

            view.GetSelectedText(out string selectedText);

            EnvDTE80.DTE2 applicationObject = serviceProvider.GetServiceAsync(typeof(DTE)).Result as EnvDTE80.DTE2;
            string filename = applicationObject?.ActiveDocument.Name;

            TextViewSelection selection = new TextViewSelection(start, end, selectedText, filename ?? "default.txt");
            return selection;
        }
    }
}
