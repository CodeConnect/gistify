using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeConnect.Gistify.Extension
{
    class SolutionManager
    {
        #region Boilerplate

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package _package;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionManager"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private SolutionManager(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this._package = package;
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static SolutionManager Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this._package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new SolutionManager(package);
        }

        #endregion

        internal static Workspace CurrentWorkspace
        {
            get
            {
                var componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
                return componentModel.GetService<VisualStudioWorkspace>();
            }
        }

        internal static Solution CurrentSolution
        {
            get { return CurrentWorkspace?.CurrentSolution; }
        }

    }
}
