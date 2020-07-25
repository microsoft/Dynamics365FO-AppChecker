// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using SocratexGraphExplorer.Models;
using SocratexGraphExplorer.ViewModels;

namespace SocratexGraphExplorer.Views
{
    public class BrowserCSObjects
    {
        private Model Model { get; set; }
        private EditorViewModel ViewModel { get; set; }

        public string show (string message)
        {
            // MessageBox.Show(message);
            return message;
        }

        /// <summary>
        /// Called when the user clicks on a node or an edge in the browser. One of
        /// the two parameters will be different from 0, indicating the selected 
        /// artifact.
        /// </summary>
        /// <param name="node">The selected node, or 0, indicating that the user selected an edge.</param>
        /// <param name="edge">The selected edge, or 0, indicating that the user selected a node.</param>
        public async void select(long node, long edge)
        {
            string cypher;

            // Select the single node or edge:
            if (node != 0)
            {
                cypher = string.Format("MATCH (c) where id(c) = {0} return c limit 1", node);
                this.ViewModel.SelectedNode = node;
            }
            else
            {
                cypher = string.Format("MATCH (c) -[r]- (d) where id(r) = {0} return r limit 1", edge);
                this.ViewModel.SelectedEdge = edge;
            }
            var res = await Model.ExecuteCypherAsync(cypher);
            this.ViewModel.GeneratePropertyNodeListView(res);
        }

        public BrowserCSObjects(Model model, EditorViewModel viewModel)
        {
            this.Model = model;
            this.ViewModel = viewModel;
        }
    }
}
