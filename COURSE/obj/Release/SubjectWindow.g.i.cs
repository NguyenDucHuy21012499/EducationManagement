﻿#pragma checksum "..\..\SubjectWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "83A1A50EE1A243BA3821A709A75F85100B13CC1C9F9D44F27837E90FE54446C5"
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
    /// SubjectWindow
    /// </summary>
    public partial class SubjectWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\SubjectWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock SubjectNameBlock;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\SubjectWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SubjectNameText;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\SubjectWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock SubjectCodeBlock;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\SubjectWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SubjectCodeText;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\SubjectWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock CreditsBlock;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\SubjectWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox CreditsText;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\SubjectWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock PrerequisiteSubjectBlock;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\SubjectWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PrerequisiteSubjectText;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\SubjectWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SearchText;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\SubjectWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox SearchCB;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\SubjectWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid SubjectGrid;
        
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
            System.Uri resourceLocater = new System.Uri("/COURSE;component/subjectwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SubjectWindow.xaml"
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
            
            #line 14 "..\..\SubjectWindow.xaml"
            ((System.Windows.Controls.Border)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Border_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SubjectNameBlock = ((System.Windows.Controls.TextBlock)(target));
            
            #line 24 "..\..\SubjectWindow.xaml"
            this.SubjectNameBlock.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.SubjectNameText_MouseDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.SubjectNameText = ((System.Windows.Controls.TextBox)(target));
            
            #line 25 "..\..\SubjectWindow.xaml"
            this.SubjectNameText.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SubjectNameText_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.SubjectCodeBlock = ((System.Windows.Controls.TextBlock)(target));
            
            #line 34 "..\..\SubjectWindow.xaml"
            this.SubjectCodeBlock.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.SubjectCodeText_MouseDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.SubjectCodeText = ((System.Windows.Controls.TextBox)(target));
            
            #line 35 "..\..\SubjectWindow.xaml"
            this.SubjectCodeText.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SubjectCodeText_TextChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.CreditsBlock = ((System.Windows.Controls.TextBlock)(target));
            
            #line 44 "..\..\SubjectWindow.xaml"
            this.CreditsBlock.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.CreditCodeText_MouseDown);
            
            #line default
            #line hidden
            return;
            case 7:
            this.CreditsText = ((System.Windows.Controls.TextBox)(target));
            
            #line 45 "..\..\SubjectWindow.xaml"
            this.CreditsText.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.CreditCodeText_TextChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.PrerequisiteSubjectBlock = ((System.Windows.Controls.TextBlock)(target));
            
            #line 54 "..\..\SubjectWindow.xaml"
            this.PrerequisiteSubjectBlock.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.PrerequisiteSubjectText_MouseDown);
            
            #line default
            #line hidden
            return;
            case 9:
            this.PrerequisiteSubjectText = ((System.Windows.Controls.TextBox)(target));
            
            #line 55 "..\..\SubjectWindow.xaml"
            this.PrerequisiteSubjectText.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.PrerequisiteSubjectText_TextChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 59 "..\..\SubjectWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddButtonClick);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 60 "..\..\SubjectWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.EditButtonClick);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 61 "..\..\SubjectWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.DeleteButtonClick);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 64 "..\..\SubjectWindow.xaml"
            ((System.Windows.Controls.Border)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Border_MouseDown);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 67 "..\..\SubjectWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SearchButtonClick);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 68 "..\..\SubjectWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ExitButtonClick);
            
            #line default
            #line hidden
            return;
            case 16:
            this.SearchText = ((System.Windows.Controls.TextBox)(target));
            return;
            case 17:
            this.SearchCB = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 18:
            this.SubjectGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 85 "..\..\SubjectWindow.xaml"
            this.SubjectGrid.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.SubjectDataGrid_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
