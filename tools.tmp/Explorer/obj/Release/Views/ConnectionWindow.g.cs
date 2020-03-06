﻿#pragma checksum "..\..\..\Views\ConnectionWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5BEC18025FBCAB3C6A8070CA97AED76ACBC1E6F99836172AA34EADC0B36EED44"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using XppReasoningWpf;
using XppReasoningWpf.Properties;


namespace XppReasoningWpf {
    
    
    /// <summary>
    /// ConnectionWindow
    /// </summary>
    public partial class ConnectionWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\..\Views\ConnectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ServerNameControl;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\Views\ConnectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PortControl;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\Views\ConnectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox UsernameControl;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\Views\ConnectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox PasswordControl;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\Views\ConnectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtVisiblePasswordbox;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\Views\ConnectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image ImgShowHide;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\Views\ConnectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label StatusControl;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SocrateX;component/views/connectionwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\ConnectionWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ServerNameControl = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.PortControl = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.UsernameControl = ((System.Windows.Controls.TextBox)(target));
            
            #line 42 "..\..\..\Views\ConnectionWindow.xaml"
            this.UsernameControl.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.UserNameTextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.PasswordControl = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 51 "..\..\..\Views\ConnectionWindow.xaml"
            this.PasswordControl.PasswordChanged += new System.Windows.RoutedEventHandler(this.TxtPasswordbox_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.txtVisiblePasswordbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.ImgShowHide = ((System.Windows.Controls.Image)(target));
            
            #line 58 "..\..\..\Views\ConnectionWindow.xaml"
            this.ImgShowHide.MouseLeave += new System.Windows.Input.MouseEventHandler(this.ImgShowHide_MouseLeave);
            
            #line default
            #line hidden
            
            #line 59 "..\..\..\Views\ConnectionWindow.xaml"
            this.ImgShowHide.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.ImgShowHide_PreviewMouseDown);
            
            #line default
            #line hidden
            
            #line 60 "..\..\..\Views\ConnectionWindow.xaml"
            this.ImgShowHide.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(this.ImgShowHide_PreviewMouseUp);
            
            #line default
            #line hidden
            return;
            case 7:
            this.StatusControl = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            
            #line 78 "..\..\..\Views\ConnectionWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OkButtonClicked);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

