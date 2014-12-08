using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Neurio.Client;
using Xamarin.Forms;


namespace Neurio.WinPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            var client = new NeurioClient();

            Forms.Init();
            Content = Neurio.App.GetMainPage(client).ConvertPageToUIElement(this);
        }
    }
}
