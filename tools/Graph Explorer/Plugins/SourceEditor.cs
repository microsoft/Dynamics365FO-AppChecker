// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SocratexGraphExplorer.XppPlugin
{
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
    using MaterialDesignThemes.Wpf;
    using SocratexGraphExplorer.SourceEditor;

    /// <summary>
    /// Base class for all the text editors.s
    /// </summary>
    internal class SourceEditor : AvalonSourceEditor
    {
        public SourceEditor()
        {
            this.Style = FindResource("SourceEditorStyle") as Style;

            // Install the search panel that appears in the upper left corner.
            ICSharpCode.AvalonEdit.Search.SearchPanel.Install(this.TextArea);

            this.PreviewMouseWheel += MouseWheelHandler;

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

            this.ContextMenu = new ContextMenu
            {
                ItemsSource = new[]
                {
                    new MenuItem {
                        Command = ApplicationCommands.Copy,
                        Icon = new PackIcon() { Kind=PackIconKind.ContentCopy },
                    },
                    new MenuItem {
                        Command = ApplicationCommands.SelectAll,
                        Icon = new PackIcon() { Kind=PackIconKind.SelectAll },
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

