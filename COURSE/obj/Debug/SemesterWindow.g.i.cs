﻿#pragma checksum "..\..\SemesterWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "4A1D7AD28CD65568AFC213904BA0ADE79F556532071C7171A58B8B8C4ABC0136"
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
    /// SemesterWindow
    /// </summary>
    public partial class SemesterWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\SemesterWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox MajorCB;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\SemesterWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock YearBlock;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\SemesterWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox YearText;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\SemesterWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock SemesterBlock;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\SemesterWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SemesterText;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\SemesterWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock SubjectCodeBlock;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\SemesterWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SubjectCodeText;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\SemesterWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid SemesterGrid;
        
        #line default
        #line hidden
        
        
        #line 114 "..\..\SemesterWindow.xaml"
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
            System.Uri resourceLocater = new System.Uri("/COURSE;component/semesterwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SemesterWindow.xaml"
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
            
            #line 15 "..\..\SemesterWindow.xaml"
            ((System.Windows.Controls.Border)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Border_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.MajorCB = ((System.Windows.Controls.ComboBox)(target));
            
            #line 22 "..\..\SemesterWindow.xaml"
            this.MajorCB.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.MajorCB_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.YearBlock = ((System.Windows.Controls.TextBlock)(target));
            
            #line 36 "..\..\SemesterWindow.xaml"
            this.YearBlock.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.YearText_MouseDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.YearText = ((System.Windows.Controls.TextBox)(target));
            
            #line 37 "..\..\SemesterWindow.xaml"
            this.YearText.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.YearText_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.SemesterBlock = ((System.Windows.Controls.TextBlock)(target));
            
            #line 47 "..\..\SemesterWindow.xaml"
            this.SemesterBlock.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.SemesterText_MouseDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.SemesterText = ((System.Windows.Controls.TextBox)(target));
            
            #line 48 "..\..\SemesterWindow.xaml"
            this.SemesterText.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SemesterText_TextChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 53 "..\..\SemesterWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddSemesterButtonClick);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 54 "..\..\SemesterWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.EditSemesterButtonClick);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 55 "..\..\SemesterWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.DeleteSemesterButtonClick);
            
            #line default
            #line hidden
            return;
            case 10:
            this.SubjectCodeBlock = ((System.Windows.Controls.TextBlock)(target));
            
            #line 64 "..\..\SemesterWindow.xaml"
            this.SubjectCodeBlock.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.SubjectCodeText_MouseDown);
            
            #line default
            #line hidden
            return;
            case 11:
            this.SubjectCodeText = ((System.Windows.Controls.TextBox)(target));
            
            #line 65 "..\..\SemesterWindow.xaml"
            this.SubjectCodeText.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SubjectCodeText_TextChanged);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 71 "..\..\SemesterWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddSubjectButtonClick);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 72 "..\..\SemesterWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.DeleteSubjectButtonClick);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 78 "..\..\SemesterWindow.xaml"
            ((System.Windows.Controls.Border)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Border_MouseDown);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 82 "..\..\SemesterWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ManageClassesButtonClick);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 83 "..\..\SemesterWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ExitButtonClick);
            
            #line default
            #line hidden
            return;
            case 17:
            this.SemesterGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 95 "..\..\SemesterWindow.xaml"
            this.SemesterGrid.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.SemesterDataGrid_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 18:
            this.SubjectGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 114 "..\..\SemesterWindow.xaml"
            this.SubjectGrid.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.SubjectDataGrid_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

