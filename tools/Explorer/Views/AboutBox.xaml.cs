using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XppReasoningWpf.Views
{
    /// <summary>
    /// Interaction logic for AboutBox.xaml
    /// </summary>
    public partial class AboutBox : Window
    {
        private readonly static Assembly assembly = Assembly.GetEntryAssembly();

        public static string AssemblyVersion => assembly.GetName().Version.ToString();

        public static string AssemblyName => assembly.GetName().ToString();

        public static string FrameworkName =>assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName;

        public static string AssemblyTitle => assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;

        public static string Copyright => assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
        public static string Description => assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
        public static string Company => assembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company;

        public AboutBox()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        public void Navigate(object sender, RequestNavigateEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = e.Uri.ToString(),
                UseShellExecute = true
            };
            Process.Start(psi);

            e.Handled = true;
        }
    }
}
