#pragma checksum "..\..\..\Profiles\Register.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4EB5A4270186E8F4924EC1BF6062E3E7319D7892"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Financing;
using Financing.Properties;
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


namespace Financing {
    
    
    /// <summary>
    /// Register
    /// </summary>
    public partial class Register : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\Profiles\Register.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Email;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Profiles\Register.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Email_box;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\Profiles\Register.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Email_Warning;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Profiles\Register.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Password;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\Profiles\Register.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox Password_box;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\Profiles\Register.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Password_Warning;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\Profiles\Register.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Confirm_password;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\Profiles\Register.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox Confirm_box;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\Profiles\Register.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Confirm_Warning;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Profiles\Register.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Submit;
        
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
            System.Uri resourceLocater = new System.Uri("/Financing;component/profiles/register.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Profiles\Register.xaml"
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
            
            #line 12 "..\..\..\Profiles\Register.xaml"
            ((Financing.Register)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseDown_1);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Email = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.Email_box = ((System.Windows.Controls.TextBox)(target));
            
            #line 22 "..\..\..\Profiles\Register.xaml"
            this.Email_box.KeyDown += new System.Windows.Input.KeyEventHandler(this.Email_box_KeyDown);
            
            #line default
            #line hidden
            
            #line 22 "..\..\..\Profiles\Register.xaml"
            this.Email_box.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.Email_box_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Email_Warning = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.Password = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.Password_box = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 27 "..\..\..\Profiles\Register.xaml"
            this.Password_box.KeyDown += new System.Windows.Input.KeyEventHandler(this.Password_box_KeyDown);
            
            #line default
            #line hidden
            
            #line 27 "..\..\..\Profiles\Register.xaml"
            this.Password_box.PasswordChanged += new System.Windows.RoutedEventHandler(this.Password_box_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.Password_Warning = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.Confirm_password = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.Confirm_box = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 36 "..\..\..\Profiles\Register.xaml"
            this.Confirm_box.KeyDown += new System.Windows.Input.KeyEventHandler(this.Confirm_box_KeyDown);
            
            #line default
            #line hidden
            
            #line 36 "..\..\..\Profiles\Register.xaml"
            this.Confirm_box.PasswordChanged += new System.Windows.RoutedEventHandler(this.Confirm_box_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.Confirm_Warning = ((System.Windows.Controls.Label)(target));
            return;
            case 11:
            this.Submit = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\..\Profiles\Register.xaml"
            this.Submit.Click += new System.Windows.RoutedEventHandler(this.Submit_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

