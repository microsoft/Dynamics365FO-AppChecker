using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace XppReasoningWpf
{
    public class LogEditor : TextEditor, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public new string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// The bindable text property dependency property
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(LogEditor), new PropertyMetadata((obj, args) =>
            {
                var target = (LogEditor)obj;
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

        public LogEditor() 
        { 
            this.IsReadOnly = true;
            this.WordWrap = true;

            var fontSizeBinding = new System.Windows.Data.Binding("LogFontSize")
            {
                Source = Properties.Settings.Default
            };
            this.SetBinding(FontSizeProperty, fontSizeBinding);

            var showLineNumbersBinding = new System.Windows.Data.Binding("ShowLineNumbers")
            {
                Source = Properties.Settings.Default
            };
            this.SetBinding(ShowLineNumbersProperty, fontSizeBinding);

            this.PreviewMouseWheel += PreviewMouseWheelImplementation;
        }

        private void PreviewMouseWheelImplementation(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Delta > 0)
                {
                    if (Properties.Settings.Default.LogFontSize < 48)
                        Properties.Settings.Default.LogFontSize += 1;
                }
                else
                {
                    if (Properties.Settings.Default.LogFontSize > 8)
                        Properties.Settings.Default.LogFontSize -= 1;
                }
            }
        }
    }
}
