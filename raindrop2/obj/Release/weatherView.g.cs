﻿

#pragma checksum "C:\Users\rsawhney\Desktop\raindrop2_release9\raindrop2\weatherView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4E062B044DB43DC3FD58347D98252A0B"
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
    partial class weatherView : global::raindrop2.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 26 "..\..\weatherView.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.doManualRefresh;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 23 "..\..\weatherView.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoToWunderground;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 36 "..\..\weatherView.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.locationManager;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 57 "..\..\weatherView.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 69 "..\..\weatherView.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.showAlert;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 66 "..\..\weatherView.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.showMenu;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


