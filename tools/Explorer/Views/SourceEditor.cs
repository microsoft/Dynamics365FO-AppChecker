﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;

namespace XppReasoningWpf
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
    public class SourceEditor : TextEditor, INotifyPropertyChanged
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
            DependencyProperty.Register("Text", typeof(string), typeof(SourceEditor), new PropertyMetadata((obj, args) =>
            {
                var target = (SourceEditor)obj;
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

        protected Model Model { get { return (App.Current.MainWindow as MainWindow).Model; } }

        protected IHighlightingDefinition LoadHighlightDefinition(string mode)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream syntaxModeStream = null;
            try
            {
                syntaxModeStream = assembly.GetManifestResourceStream("XppReasoningWpf.Resources." + mode);

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
                    catch (ArgumentOutOfRangeException )
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

        public SourceEditor()
        {
            this.TextArea.Caret.PositionChanged += (object sender, EventArgs a) =>
            {
                var caret = sender as ICSharpCode.AvalonEdit.Editing.Caret;
                this.Model.CaretPositionString = string.Format(CultureInfo.CurrentCulture, "Line: {0} Column: {1}", caret.Line, caret.Column);
            };

            // Install the search panel that appears in the upper left corner.
            ICSharpCode.AvalonEdit.Search.SearchPanel.Install(this.TextArea);

            this.PreviewMouseWheel += MouseWheelHandler;
            this.IsReadOnly = true;

            var fontFamilyBinding = new Binding("SourceFont")
            {
                Source = Properties.Settings.Default
            };
            this.SetBinding(FontFamilyProperty, fontFamilyBinding);

            var fontSizeBinding = new Binding("SourceFontSize")
            {
                Source = Properties.Settings.Default
            };
            this.SetBinding(FontSizeProperty, fontSizeBinding);

            var showLineNumbersBinding = new Binding("ShowLineNumbers")
            {
                Source = Properties.Settings.Default
            };
            this.SetBinding(ShowLineNumbersProperty, showLineNumbersBinding);

            this.Options = new TextEditorOptions
            {
                ConvertTabsToSpaces = true,
                IndentationSize = 4,
            };

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
                    new MenuItem {
                        Command = ApplicationCommands.Delete,
                        Icon = new Image {
                                Source = new BitmapImage(new Uri("images/Cancel_16x.png", UriKind.Relative))
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

        protected virtual void MouseWheelHandler(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Delta > 0)
                {
                    if (Properties.Settings.Default.SourceFontSize < 48)
                        Properties.Settings.Default.SourceFontSize += 1;

                }
                else
                {
                    if (Properties.Settings.Default.SourceFontSize < 48)
                        Properties.Settings.Default.SourceFontSize -= 1;
                }
            }
        }
    }
}
