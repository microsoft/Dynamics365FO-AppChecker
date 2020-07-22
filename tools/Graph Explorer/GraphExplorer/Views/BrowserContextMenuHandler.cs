// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using CefSharp;
using SocratexGraphExplorer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocratexGraphExplorer
{
    public class BrowserContextMenuHandler : IContextMenuHandler
    {
        private CefMenuCommand HideSelectedCommand = CefMenuCommand.UserFirst;
        private CefMenuCommand ShowRelationsForAll = CefMenuCommand.UserFirst + 1;
        private EditorViewModel ViewModel { get; set; }

        public BrowserContextMenuHandler(EditorViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        //This method prepares the context menu
        public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            //Clear the model
            model.Clear();

            //Add the menu items
            model.AddItem(HideSelectedCommand, "Hide Selected node");
            model.AddItem(ShowRelationsForAll, "Show Relations from all nodes");

            //Enable as needed
            // model.SetEnabledAt(0, !string.IsNullOrWhiteSpace(parameters.SelectionText));
        }

        // This method handles the menu item click
        public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters,
            CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            // If the 'Search' menu item is pressed
            if (commandId == HideSelectedCommand)
            {
                // Notify if the context menu click is handled
                // this.hide
                return true;
            }

            if (commandId == ShowRelationsForAll)
            {
                // Notify if the context menu click is handled
                return true;
            }

            // Otherwise, let the default handler to handle the click
            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
        }

        public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }
}
