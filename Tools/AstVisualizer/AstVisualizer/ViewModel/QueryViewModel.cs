// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Windows.Input;

namespace AstVisualizer
{
    public class QueryViewModel
    {
        private readonly ICommand keyboardExecuteQueryCommand; 
        private readonly ICommand executeQueryCommand;
        public ICommand KeyboardExecuteQueryCommand => this.keyboardExecuteQueryCommand;
        public ICommand ExecuteQueryCommand => this.executeQueryCommand;
        public QueryViewModel()
        {
            keyboardExecuteQueryCommand = new RelayCommand(
                p => { 
                }
            );

            executeQueryCommand = new RelayCommand(
                p => {
                }
            );

        }
    }
}
