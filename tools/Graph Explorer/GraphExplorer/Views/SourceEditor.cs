// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace GraphExplorer.Views
{
    using System;
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
    using GraphExplorer.Models;
    using MaterialDesignThemes.Wpf;
    using GraphExplorer.SourceEditor;

    /// <summary>
    /// Base class for all the text editors.s
    /// </summary>
    public class SourceEditor : AvalonSourceEditor
    {
        public SourceEditor()
        {
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

