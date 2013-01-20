﻿using System.ComponentModel.Composition;
using EnvDTE;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

namespace UIHelpers
{
    /// <summary>
    /// Listener responsible for injection of our command filters into every newly created editor window. This
    /// class is required to support non-native for visual studio syntaxes like SASS and LESS.
    /// </summary>
    [TextViewRole("DOCUMENT"), ContentType("Plain Text"), Export(typeof(IVsTextViewCreationListener))]
    public class PlainTextViewCreationListener : ViewCreationListenerBase, IVsTextViewCreationListener
    {
        [Import]
        internal override ICompletionBroker CompletionBroker { get; set; }

        [Import]
        internal override IVsEditorAdaptersFactoryService EditorAdaptersFactoryService { get; set; }

        /// <summary>
        /// Gets the syntax of the created document.
        /// </summary>
        protected internal override EmmetSyntax Syntax
        {
            get { return EmmetSyntax.Css; }
        }

        /// <summary>
        /// Called when a <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsTextView" /> adapter has
        /// been created and initialized.
        /// </summary>
        /// <param name="textViewAdapter">The newly created and initialized text view adapter.</param>
        public override void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));
            string docName = dte.ActiveDocument.Name;

            string normalizedName = docName.ToLower();
            if (normalizedName.EndsWith(".scss") || normalizedName.EndsWith(".less"))
                base.VsTextViewCreated(textViewAdapter);
        }
    }
}