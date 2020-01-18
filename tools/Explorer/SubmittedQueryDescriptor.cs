// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.ComponentModel;

namespace XppReasoningWpf
{
    public class SubmittedQueryDescriptor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private string jobId;
        public string JobId
        {
            get { return this.jobId; }
            private set
            {
                if (this.jobId != value)
                {
                    this.jobId = value;
                    OnPropertyChanged(nameof(JobId));
                }
            }
        }

        private string query = "";
        public string Query {
            get { return this.query; }
            set
            {
                if (string.Compare(this.query, value) != 0)
                {
                    this.query = value;
                    OnPropertyChanged(nameof(Query));
                }
            }
        }

        private TimeSpan duration = TimeSpan.MinValue;
        public TimeSpan Duration {
            get { return this.duration; }
            set
            {
                if (this.duration != value)
                {
                    this.duration = value;
                    OnPropertyChanged(nameof(Duration));
                }
            }
        }

        private bool isrunning = false;
        public bool IsRunning
        {
            get { return this.isrunning; }
            set
            {
                if (this.isrunning != value)
                {
                    this.isrunning = value;
                    OnPropertyChanged(nameof(IsRunning));
                }
            }
        }

        private string user = "";
        public string User
        {
            get { return this.user; }
            set
            {
                if (string.Compare(this.user, value) != 0)
                {
                    this.user = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }

        public bool Delete { get; set; } // Internal bookkeeping, not shown on UI

        public SubmittedQueryDescriptor(string jobId)
        {
            this.JobId = jobId;
            this.Delete = false;
        }
    }
}
