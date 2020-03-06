// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Windows.Input;

namespace XppReasoningWpf
{
    using System.Windows;

    class ApplicationExitCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parm)
        {
            return true;
        }

        public void Execute(object parm)
        {
            Application.Current.Shutdown();
        }
    }
}
