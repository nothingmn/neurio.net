using System;
using System.Collections.Generic;
using System.Text;
using Neurio.Client;
using Neurio.ViewModels;
using Xamarin.Forms;

namespace Neurio.Views
{
    public class LandingViewPage : ContentPage
    {
        private readonly NeurioClient _client;
        public LandingViewModel ViewModel { get; set; }

        public LandingViewPage(NeurioClient client)
        {
            _client = client;
            ViewModel = new LandingViewModel(client);
            this.BindingContext = ViewModel;

            var root = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };


            root.Children.Add(LoadLoginLayout());
            root.Children.Add(LoadAppliancesLayout());

            this.Content = root;

        }

        private RelativeLayout LoadLoginLayout()
        {
            var loginTitle = new Label();
            loginTitle.SetBinding(Label.TextProperty, "LoginTitle");


            var emailTitle = new Label();
            emailTitle.SetBinding(Label.TextProperty, "EmailLabelText");
            var emailInput = new Entry();
            emailInput.SetBinding(Entry.TextProperty, "EmailText");
            emailInput.WidthRequest = 300;

            var passwordTitle = new Label();
            passwordTitle.SetBinding(Label.TextProperty, "PasswordLabelText");
            var passwordInput = new Entry() {IsPassword = true};
            passwordInput.SetBinding(Entry.TextProperty, "PasswordText");
            passwordInput.WidthRequest = 300;

            var loginButton = new Button();
            loginButton.SetBinding(Button.TextProperty, "LoginButtonText");
            loginButton.SetBinding(Button.CommandProperty, "LoginCommand");

            var statusLabel = new Label();
            statusLabel.SetBinding(Label.TextProperty, "StatusText");


            
            var padding = 5;
            var loginLayout = new RelativeLayout()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children =
                {
                    {loginTitle, Constraint.RelativeToParent((parent) => padding)},
                    {
                        emailTitle, Constraint.RelativeToParent((parent) => padding),
                        Constraint.RelativeToParent((parent) => loginTitle.Height + padding + (loginTitle.Height/2))
                    },
                    {
                        emailInput, Constraint.RelativeToParent((parent) => parent.Width/3),
                        Constraint.RelativeToParent((parent) => loginTitle.Height + padding)
                    },
                    {
                        passwordTitle, Constraint.RelativeToParent((parent) => padding),
                        Constraint.RelativeToParent(
                            (parent) =>
                                loginTitle.Height + padding + emailInput.Height + padding + (passwordTitle.Height/2))
                    },
                    {
                        passwordInput, Constraint.RelativeToParent((parent) => parent.Width/3),
                        Constraint.RelativeToParent(
                            (parent) => loginTitle.Height + padding + emailInput.Height + padding)
                    },
                    {
                        loginButton, Constraint.RelativeToParent((parent) => parent.Width/3),
                        Constraint.RelativeToParent(
                            (parent) =>
                                loginTitle.Height + padding + emailInput.Height + padding + passwordInput.Height +
                                padding)
                    },
                    {
                        statusLabel, Constraint.RelativeToParent((parent) => 0),
                        Constraint.RelativeToParent(
                            (parent) =>
                                loginTitle.Height + padding + emailInput.Height + padding + passwordInput.Height + padding + loginButton.Height + padding + padding)
                    },
                }
            };
            loginLayout.SetBinding(RelativeLayout.IsVisibleProperty, "LoginVisible");
            return loginLayout;

        }

        private RelativeLayout LoadAppliancesLayout()
        {
            var appliancesTitle = new Label();
            appliancesTitle.SetBinding(Label.TextProperty, "AppliancesTitle");

            var list = new ListView()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                ItemTemplate = new DataTemplate(() =>
                {
                    Label nameLabel = new Label();
                    nameLabel.SetBinding(Label.TextProperty, "Label");

                    return new ViewCell()
                    {
                        View = new StackLayout()
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Vertical,
                            HorizontalOptions = LayoutOptions.StartAndExpand,
                            VerticalOptions = LayoutOptions.Center,
                            Children =
                            {
                                nameLabel,
                            }
                        }
                    };
                })
            };
            list.SetBinding(ListView.ItemsSourceProperty, "Appliances");
            //list.SetBinding(ListView.SelectedItemProperty, "ApplianceSelected");
            var padding = 5;

            var appliancesLayout = new RelativeLayout()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children =
                {
                    {appliancesTitle, Constraint.RelativeToParent((parent) => 0)},
                    {list, Constraint.RelativeToParent((parent) => 0), Constraint.RelativeToParent((parent) => appliancesTitle.Height + padding )}
                }
            };
            appliancesLayout.SetBinding(RelativeLayout.IsVisibleProperty, "ApplicancesTabVisible");
            return appliancesLayout;
        }

    }
}