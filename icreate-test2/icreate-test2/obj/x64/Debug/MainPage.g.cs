﻿

#pragma checksum "C:\Users\Jiajianiu\Documents\GitHub\ivle_metro_win8\icreate-test2\icreate-test2\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "861400141DD2DF7D1433F25E7EE0B729"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace icreate_test2
{
    partial class MainPage : global::icreate_test2.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 33 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Logoff_Button_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 29 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 30 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Logoff_Button_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 131 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.ModuleTileTapped;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 201 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.prevDay;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 222 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.nextDay;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 53 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.ItemListView_SelectionChanged;
                 #line default
                 #line hidden
                #line 54 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.AnnoucementListViewTapped;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

