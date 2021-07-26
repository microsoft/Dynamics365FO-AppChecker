namespace GraphExplorer.Views
{
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Navigation;
    using MaterialDesignExtensions.Controls;

    /// <summary>
    /// Interaction logic for AboutBox.xaml
    /// </summary>
    public partial class AboutBox : MaterialWindow
    {
        private static Assembly assembly = Assembly.GetEntryAssembly();

        public string AssemblyVersion
        {
            get
            {
                return assembly.GetName().Version.ToString();
            }
        }

        public string AssemblyName
        {
            get
            {
                return assembly.GetName().ToString();
            }
        }

        public string FrameworkName
        {
            get
            {
                return assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName;
            }
        }

        public string AssemblyTitle => assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
        public string Copyright => assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
        public string Description => assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
        public string Company => assembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company;

        public AboutBox()
        {
            InitializeComponent();
            this.DataContext = this;
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