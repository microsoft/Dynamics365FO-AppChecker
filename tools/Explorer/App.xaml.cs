// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Reflection;
using System.Windows;

namespace XppReasoningWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Specifies whether or not telemetry information should be sent to the 
        /// endpoint defined by the key below.
        /// </summary>

#if !NETCOREAPP
        private bool SendTelemetry = false;

        public TelemetryClient Telemetry { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Fill in the key to access your application insights telemetry. 
            string key = "";
            this.Telemetry = null;

            base.OnStartup(e);

            if (SendTelemetry)
            {
                this.Telemetry = new TelemetryClient();

                TelemetryConfiguration.Active.InstrumentationKey = key;
                this.Telemetry.InstrumentationKey = key;
                this.Telemetry.Context.Session.Id = Guid.NewGuid().ToString();
                this.Telemetry.Context.User.AccountId = "Test user"; // This will be initialized when the user logs on
                this.Telemetry.Context.User.Id = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                this.Telemetry.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
                this.Telemetry.Context.Component.Version = Assembly.GetEntryAssembly().GetName().Version.ToString();

                this.Telemetry.TrackEvent("Application Start");
            }
        }
#endif
        protected override void OnExit(ExitEventArgs e)
        {
#if !NETCOREAPP
            // Send outstanding events to the cloud.
            this.Telemetry?.Flush();

            // Allow time for flushing:
            System.Threading.Thread.Sleep(1000);
#endif
            base.OnExit(e);

        }
    }
}
