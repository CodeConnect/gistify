using System;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Text.Projection;

namespace CodeConnect.Gistify.Extension
{
    public static class TextManagerExtensions
    {
        /// <summary>
        /// Obtains caret position and file name associated with
        /// current selection in the editor (supplied via textManager)
        /// </summary>
        /// <param name="textManager">Instance of IVsTextManager taken from the Package</param>
        public static bool TryFindDocumentAndPosition(this IVsTextManager textManager, out string filePath, out int selectionStart, out int selectionEnd)
        {
            IWpfTextView view = GetActiveTextView(textManager);
            if (view == null)
            {
                // We couldn't get the ITextDocument
                filePath = null;
                selectionStart = -1;
                selectionEnd = -1;
                return false;
            }

            ITextBuffer sourceBuffer = view.TextBuffer;
            selectionStart = view.Selection.Start.Position.Position;
            selectionEnd = view.Selection.End.Position.Position;
            //caretPosition = view.Caret.Position.BufferPosition;

            //Check if this is an elision buffer
            //If so, we need to take a look at the actual source buffer
            var elisionBuffer = sourceBuffer as IElisionBuffer;
            if (elisionBuffer != null)
            {
                // TODO: Handle Elision Buffers
                //sourceBuffer = elisionBuffer.SourceBuffer;
                //caretPosition = elisionBuffer.CurrentSnapshot.MapToSourceSnapshot(caretPosition);
            }

            // See what file are we in
            var textDocument = (ITextDocument)sourceBuffer.Properties.GetProperty(typeof(ITextDocument));
            filePath = textDocument.FilePath;
            // Editors that don't host code files get filePath of "Temp.txt". Ignore these.
            if (filePath == "Temp.txt")
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the active IWpfTextView, if one exists.
        /// </summary>
        /// <returns>The active IWpfTextView, or null if no such IWpfTextView exists.</returns>
        private static IWpfTextView GetActiveTextView(IVsTextManager textManager)
        {
            IWpfTextView view = null;
            IVsTextView vTextView = null;

            textManager.GetActiveView(
                fMustHaveFocus: 1
                , pBuffer: null
                , ppView: out vTextView);

            IVsUserData userData = vTextView as IVsUserData;
            if (null != userData)
            {
                IWpfTextViewHost viewHost;
                object holder;
                Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
                userData.GetData(ref guidViewHost, out holder);
                viewHost = (IWpfTextViewHost)holder;
                view = viewHost.TextView;
            }

            return view;
        }
    }
}
