﻿#pragma checksum "C:\Users\David\Documents\kinect-kollage\KinectKollagePhoneApp\KinectKollagePhoneApp\TextPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F7526DA68FB7B6FDB346058DAE063BEF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace KinectKollagePhoneApp {
    
    
    public partial class TextPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Canvas ContentPanelCanvas;
        
        internal System.Windows.Controls.Image image2;
        
        internal System.Windows.Controls.TextBox enteredtxt;
        
        internal System.Windows.Controls.TextBox textsize;
        
        internal System.Windows.Controls.ListBox colorBox;
        
        internal System.Windows.Controls.ListBox fontBox;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/KinectKollagePhoneApp;component/TextPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.ContentPanelCanvas = ((System.Windows.Controls.Canvas)(this.FindName("ContentPanelCanvas")));
            this.image2 = ((System.Windows.Controls.Image)(this.FindName("image2")));
            this.enteredtxt = ((System.Windows.Controls.TextBox)(this.FindName("enteredtxt")));
            this.textsize = ((System.Windows.Controls.TextBox)(this.FindName("textsize")));
            this.colorBox = ((System.Windows.Controls.ListBox)(this.FindName("colorBox")));
            this.fontBox = ((System.Windows.Controls.ListBox)(this.FindName("fontBox")));
        }
    }
}

