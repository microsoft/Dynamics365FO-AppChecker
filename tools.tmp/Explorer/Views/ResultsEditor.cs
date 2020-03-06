// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;

namespace XppReasoningWpf
{
    using ICSharpCode.AvalonEdit;
    using ICSharpCode.AvalonEdit.Folding;
    using ICSharpCode.AvalonEdit.Highlighting;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using System.Xml;
    using System.Xml.Linq;

    public class ResultsEditor : TextEditor, INotifyPropertyChanged
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
            DependencyProperty.Register("Text", typeof(string), typeof(ResultsEditor), new PropertyMetadata((obj, args) =>
            {
                var target = (ResultsEditor)obj;
                target.Text = (string)args.NewValue;
            }));

        /// <summary>
        /// Raises a property changed event
        /// </summary>
        /// <param name="property">The name of the property that updates</param>
        public void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected Model Model
        {
            get
            {
                if (App.Current.MainWindow != null)
                    return (App.Current.MainWindow as MainWindow).Model;
                return null;
            }
        }

        private XDocument resultDocument = null;
        public XDocument XmlDocument =>  this.resultDocument;  

        public ResultsEditor()
        {
            var fontFamilyBinding = new Binding("ResultFont")
            {
                Source = Properties.Settings.Default
            };
            this.SetBinding(FontFamilyProperty, fontFamilyBinding);

            var fontSizeBinding = new Binding("ResultsFontSize")
            {
                Source = Properties.Settings.Default
            };
            this.SetBinding(FontSizeProperty, fontSizeBinding);

            var showLineNumbersBinding = new Binding("ShowLineNumbers")
            {
                Source = Properties.Settings.Default
            };
            this.SetBinding(ShowLineNumbersProperty, showLineNumbersBinding);

            this.PreviewMouseWheel += ResultsEditor_PreviewMouseWheel;

            this.SyntaxHighlighting = (IHighlightingDefinition)(new HighlightingDefinitionTypeConverter()).ConvertFrom("XML");// this.LoadHighlightDefinition("XQuery.xshd");

            ICSharpCode.AvalonEdit.Search.SearchPanel.Install(this.TextArea);

            this.IsReadOnly = false;

            this.ContextMenu = new ContextMenu
            {
                ItemsSource = new[]
                {
                    new MenuItem {
                        Command = ApplicationCommands.Cut,
                        Icon = new Image {
                            Source = new BitmapImage(new Uri("images/Cut_16x.png", UriKind.Relative))
                        }},
                    new MenuItem {
                        Command = ApplicationCommands.Copy,
                        Icon = new Image {
                                Source = new BitmapImage(new Uri("images/Copy_16x.png", UriKind.Relative))
                            }},
                    new MenuItem {
                        Command = ApplicationCommands.Paste,
                        Icon = new Image {
                                Source = new BitmapImage(new Uri("images/Paste_16x.png", UriKind.Relative))
                        }},
                    new MenuItem()
                    {
                        Header = "Undo",
                        ToolTip = "Undo",
                        Icon = new Image
                        {
                            Source = new BitmapImage(new Uri("images/undo_16x.png", UriKind.Relative))
                        },
                        InputGestureText = "Ctrl+Z",
                        Command = new RelayCommand(
                            p => { this.Undo(); },
                            p => { return this.CanUndo; })
                    },
                    new MenuItem() {
                        Header = "Redo",
                        ToolTip = "Redo",
                        Icon = new Image
                        {
                            Source = new BitmapImage(new Uri("images/redo_16x.png", UriKind.Relative))
                        },
                        InputGestureText = "Ctrl+Y",
                        Command = new RelayCommand(
                            p => { this.Redo(); },
                            p => { return this.CanRedo; })
                    },
                    new MenuItem {
                        Command = ApplicationCommands.SelectAll,
                        Icon = new Image
                        {
                            Source = new BitmapImage(new Uri("images/SelectAll_16x.png", UriKind.Relative))
                        }
                    }
                }
            };
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Configure XML folding strategy:
            this.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
            var xmlFoldingStrategy = new XmlFoldingStrategy();
            var xmlFoldingManager = FoldingManager.Install(this.TextArea);

            try
            {
                xmlFoldingStrategy.UpdateFoldings(xmlFoldingManager, this.Document);
            }
            catch (XmlException)
            {
                // Nothing
            }

            if (this.Model != null)
            {
                // This may be the case when the designer in VS wants to render the view
                this.Model.Tick += (object sender, EventArgs args) =>
                {
                    try
                    {
                        xmlFoldingStrategy.UpdateFoldings(xmlFoldingManager, this.Document);
                    }
                    catch(XmlException)
                    {
                        // Nothing. The text in the result does not have to be an XML document.
                    }
                };
            }
        }

        private void ResultsEditor_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Delta > 0)
                {
                    if (Properties.Settings.Default.ResultsFontSize < 48)
                        Properties.Settings.Default.ResultsFontSize += 1;
                }
                else
                {
                    if (Properties.Settings.Default.ResultsFontSize > 8)
                        Properties.Settings.Default.ResultsFontSize -= 1;
                }
            }
        }

        /// <summary>
        /// Called when the content of the results gets modified. Attempt to create
        /// an XML document for the data in the result window.
        /// </summary>
        /// <param name="e">Not used.</param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            RaisePropertyChanged("Text");

            var text = this.Text;

            try
            {
                this.resultDocument = XDocument.Parse(text, LoadOptions.SetLineInfo);
            }
            catch (XmlException)
            {
                this.resultDocument = null;
            }
        }
    }
}
