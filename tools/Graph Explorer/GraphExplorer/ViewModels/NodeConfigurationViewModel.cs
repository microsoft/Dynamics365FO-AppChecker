using GraphExplorer.Models;
using GraphExplorer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace GraphExplorer.ViewModels
{
    public class NodeConfigurationViewModel : INotifyPropertyChanged
    {
        public NodeConfigurationControl View { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<NodeConfiguration> Configurations { get; set; }

        private NodeConfiguration selectedItem;


        public NodeConfiguration SelectedItem {
            get
            {
                return this.selectedItem;
            }
            set
            {
                this.selectedItem = value;
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Init()
        {
            var labels = Neo4jDatabase.GetNodeLabels().Result;
            var conf = new ObservableCollection<NodeConfiguration>();

            foreach (var label in labels)
            {
                conf.Add(new NodeConfiguration { Color = "red", Selection = label, Size = 100 });
            }

            this.Configurations = conf;
        }

        public NodeConfigurationViewModel(NodeConfigurationControl view)
        {
            this.View = view;
        }
    }
}
