﻿

#pragma checksum "C:\Users\rsawhney\Desktop\raindrop2\raindrop2\PreferencesUserControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "17A4AF69ED11172C388C19343A5EA84E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace raindrop2
{
    partial class PreferencesUserControl : global::Windows.UI.Xaml.Controls.UserControl, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 15 "..\..\PreferencesUserControl.xaml"
                ((global::Windows.UI.Xaml.Controls.TextBox)(target)).TextChanged += this.firstName_TextChanged;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 18 "..\..\PreferencesUserControl.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target)).Checked += this.tempRadioC_Checked;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 19 "..\..\PreferencesUserControl.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target)).Checked += this.tempRadioF_Checked;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 22 "..\..\PreferencesUserControl.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target)).Checked += this.distRadioM_Checked;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 23 "..\..\PreferencesUserControl.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target)).Checked += this.distRadioI_Checked;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

