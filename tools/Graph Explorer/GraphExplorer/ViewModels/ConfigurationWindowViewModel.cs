using MaterialDesignExtensions.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace GraphExplorer.ViewModels
{
    using MaterialDesignExtensions.Model;

    public class StepperColorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public StepperColorViewModel()
        {

        }
    }

    public class StepperShapeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public StepperShapeViewModel()
        {

        }
    }


    class ConfigurationWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ConfigurationWindowViewModel()
        {
        }
    }
}
