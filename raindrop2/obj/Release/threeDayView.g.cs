﻿

#pragma checksum "C:\Users\rsawhney\Desktop\raindrop2_release9\raindrop2\threeDayView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "58CCB0B5B20388FCF94231A0EAACA36B"
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
    partial class threeDayView : global::raindrop2.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 27 "..\..\threeDayView.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.doManualRefresh;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 24 "..\..\threeDayView.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoToWunderground;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 37 "..\..\threeDayView.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.locationManager;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 59 "..\..\threeDayView.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 68 "..\..\threeDayView.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.showMenu;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


