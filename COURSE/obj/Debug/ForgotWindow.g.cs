﻿#pragma checksum "..\..\ForgotWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "C06A54D3C63E12CEDA9E7BEAEE490B041DCEFA062BA5710AFD6BCA136016AE8C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using COURSE;
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


namespace COURSE {
    
    
    /// <summary>
    /// ForgotWindow
    /// </summary>
    public partial class ForgotWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\ForgotWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock EmailBlock;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\ForgotWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox EmailText;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\ForgotWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock UsernameBlock;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\ForgotWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox UsernameText;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\ForgotWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock PassBlock;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\ForgotWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox PassText;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\ForgotWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ConfirmPassBlock;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\ForgotWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox ConfirmPassText;
        
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
            System.Uri resourceLocater = new System.Uri("/COURSE;component/forgotwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ForgotWindow.xaml"
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
            
            #line 12 "..\..\ForgotWindow.xaml"
            ((System.Windows.Controls.Border)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Border_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 14 "..\..\ForgotWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Exit_Button_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.EmailBlock = ((System.Windows.Controls.TextBlock)(target));
            
            #line 27 "..\..\ForgotWindow.xaml"
            this.EmailBlock.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.EmailText_MouseDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.EmailText = ((System.Windows.Controls.TextBox)(target));
            
            #line 28 "..\..\ForgotWindow.xaml"
            this.EmailText.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.EmailText_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.UsernameBlock = ((System.Windows.Controls.TextBlock)(target));
            
            #line 40 "..\..\ForgotWindow.xaml"
            this.UsernameBlock.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.UsernameText_MouseDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.UsernameText = ((System.Windows.Controls.TextBox)(target));
            
            #line 41 "..\..\ForgotWindow.xaml"
            this.UsernameText.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.UsernameText_TextChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.PassBlock = ((System.Windows.Controls.TextBlock)(target));
            
            #line 53 "..\..\ForgotWindow.xaml"
            this.PassBlock.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.PassText_MouseDown);
            
            #line default
            #line hidden
            return;
            case 8:
            this.PassText = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 54 "..\..\ForgotWindow.xaml"
            this.PassText.PasswordChanged += new System.Windows.RoutedEventHandler(this.PassText_TextChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.ConfirmPassBlock = ((System.Windows.Controls.TextBlock)(target));
            
            #line 65 "..\..\ForgotWindow.xaml"
            this.ConfirmPassBlock.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.ConfirmPassText_MouseDown);
            
            #line default
            #line hidden
            return;
            case 10:
            this.ConfirmPassText = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 66 "..\..\ForgotWindow.xaml"
            this.ConfirmPassText.PasswordChanged += new System.Windows.RoutedEventHandler(this.ConfirmPassText_TextChanged);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 69 "..\..\ForgotWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Save_Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

