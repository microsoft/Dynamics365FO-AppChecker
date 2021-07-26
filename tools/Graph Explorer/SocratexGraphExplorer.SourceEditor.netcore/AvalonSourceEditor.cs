// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml;
//using MaterialDesignThemes.Wpf;

namespace GraphExplorer.SourceEditor
{

    /// <summary>
    /// Base class for all the text editors.
    /// </summary>
    public class AvalonSourceEditor : TextEditor, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// A bindable Text property
        /// </summary>
        public new string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// The bindable text property dependency property
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AvalonSourceEditor), new PropertyMetadata((obj, args) =>
            {
                var target = (AvalonSourceEditor)obj;
                target.Text = (string)args.NewValue;
            }));

        protected override void OnTextChanged(EventArgs e)
        {
            RaisePropertyChanged("Text");
            base.OnTextChanged(e);
        }

        /// <summary>
        /// Raises a property changed event
        /// </summary>
        /// <param name="property">The name of the property that updates</param>
        public void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Load the hightliht definitions (i.e. the colorizations) from the mode file in the 
        /// given assembly.
        /// </summary>
        /// <param name="mode">The qualified name of the resource containing the xshd file<./param>
        /// <param name="assembly">The assembly containing the embedded resource.</param>
        /// <returns>The highlight definition instance.</returns>
        public static IHighlightingDefinition LoadHighlightDefinition(string mode, Assembly assembly)
        {
            // Use the local (i.e. current) assembly if no assembly was provided
            if (assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }

            Stream syntaxModeStream = null;
            try
            {
                syntaxModeStream = assembly.GetManifestResourceStream(mode);

                using (var xshd_reader = new XmlTextReader(syntaxModeStream))
                {
                    xshd_reader.DtdProcessing = DtdProcessing.Prohibit;
                    xshd_reader.XmlResolver = null;

                    syntaxModeStream = null;
                    return ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(xshd_reader, HighlightingManager.Instance);
                }
            }
            finally
            {
                if (syntaxModeStream != null)
                    syntaxModeStream.Dispose();

            }
        }

        public void SetPosition(int startLine, int startCol, int endLine, int endCol)
        {
            TextEditor control = this;

            if (startLine > 0)
            {
                if (endLine < 0)
                {
                    // End line not provided, so do not build a selection, only position the cursor
                    if (startCol < 0)
                        startCol = 1;

                    control.TextArea.Caret.Position = new ICSharpCode.AvalonEdit.TextViewPosition(startLine, startCol);
                }
                else
                {
                    int startOffset, endOffset;

                    if (endCol < 0)
                        endCol = 1;

                    // Do not fail if the offsets are incorrect. The GetOffset methods
                    // below throw exceptions if the parameters do not designate valid offsets
                    try
                    {
                        // The user provided end coordinates. 
                        startOffset = control.Document.GetOffset(startLine, startCol);
                        endOffset = control.Document.GetOffset(endLine, endCol);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        startOffset = 0;
                        endOffset = 0;
                        startLine = 1;
                        startCol = 1;
                    }

                    control.TextArea.Caret.Position = new ICSharpCode.AvalonEdit.TextViewPosition(startLine, startCol);

                    var selection = ICSharpCode.AvalonEdit.Editing.Selection.Create(control.TextArea, startOffset, endOffset);
                    control.TextArea.Selection = selection;
                }

                control.TextArea.Caret.BringCaretToView();
            }
        }

        public AvalonSourceEditor()
        {
            this.Style = FindResource("SourceEditorStyle") as Style;

            ICSharpCode.AvalonEdit.Search.SearchPanel.Install(this.TextArea);

            this.Options = new TextEditorOptions
            {
                ConvertTabsToSpaces = true,
                IndentationSize = 4,
            };
        }
    }
}
