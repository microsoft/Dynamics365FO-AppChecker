// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;

namespace AstVisualizer
{
    using ICSharpCode.AvalonEdit;
    using ICSharpCode.AvalonEdit.Highlighting;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using System.Xml;

    /// <summary>
    /// Base class for all the text editors.s
    /// </summary>
    public class BindableEditor : TextEditor, INotifyPropertyChanged
    {
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
            DependencyProperty.Register("Text", typeof(string), typeof(BindableEditor), new PropertyMetadata((obj, args) =>
            {
                var target = (BindableEditor)obj;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected IHighlightingDefinition LoadHighlightDefinition(string mode)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream syntaxModeStream = null;
            try
            {
                syntaxModeStream = assembly.GetManifestResourceStream("AstVisualizer.Resources." + mode);

                using (var xshd_reader = new XmlTextReader(syntaxModeStream)
                {
                    XmlResolver = null,
                    DtdProcessing = DtdProcessing.Prohibit
                })
                {
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

        public BindableEditor()
        {
            this.TextArea.Caret.PositionChanged += (object sender, EventArgs a) =>
            {
                var caret = sender as ICSharpCode.AvalonEdit.Editing.Caret;
                // this.Model.CaretPositionString = string.Format(CultureInfo.CurrentCulture, "Line: {0} Column: {1}", caret.Line, caret.Column);
            };

            // Install the search panel that appears in the upper left corner.
            ICSharpCode.AvalonEdit.Search.SearchPanel.Install(this.TextArea);
        }
    }
}
