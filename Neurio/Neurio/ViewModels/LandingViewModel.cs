using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Neurio.Client;
using Neurio.Client.Entities;
using Neurio.Commands;

namespace Neurio.ViewModels
{
    public class LandingViewModel : BaseViewModel
    {
        public bool LoginVisible
        {
            get { return _loginVisible; }
            set
            {
                _loginVisible = value;
                OnPropertyChanged();
            }
        }

        public bool ApplicancesTabVisible
        {
            get { return _applicancesTabVisible; }
            set
            {
                _applicancesTabVisible = value;
                OnPropertyChanged();
            }
        }

        public LandingViewModel(NeurioClient neurioClient)
            : base(neurioClient)
        {
            LoginVisible = true;
            ApplicancesTabVisible = false;
            LoginTitle = "Login with your neurio credentials.";
            AppliancesTitle = "Your Appliances";
            EmailLabelText = "Email Address:";
            PasswordLabelText = "Password:";
            LoginButtonText = "Sign In";

        }

        private ICommand loginCommand;

        public ICommand LoginCommand
        {
            get
            {
                if (loginCommand == null)
                    loginCommand = new LoginCommand(base.Client, this);

                (loginCommand as LoginCommand).OnLoginComplete += LoginViewModel_OnLoginComplete;
                return loginCommand;
            }
            set { loginCommand = value; }
        }

        private void LoginViewModel_OnLoginComplete(NeurioClient client,
            Neurio.Client.Entities.Results.LoginResult result)
        {
            if (result.Success)
            {
                StatusText = "Login was successful.";
            }
            else
            {
                StatusText = "Login failed.  Try again.";
            }
            ApplicancesTabVisible = result.Success;
            LoginVisible = !result.Success;
            OnPropertyChanged("Appliances");
        }

        private string _statusText;

        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                OnPropertyChanged();
            }
        }

        private string _passwordLabelText;

        public string PasswordLabelText
        {
            get { return _passwordLabelText; }
            set
            {
                _passwordLabelText = value;
                OnPropertyChanged();
            }
        }

        private string _emailLabelText;

        public string EmailLabelText
        {
            get { return _emailLabelText; }
            set
            {
                _emailLabelText = value;
                OnPropertyChanged();
            }
        }

        private string _loginButtonText;

        public string LoginButtonText
        {
            get { return _loginButtonText; }
            set
            {
                _loginButtonText = value;
                OnPropertyChanged();
            }
        }

        private string _loginTitle;
        private bool _loginVisible;
        private bool _applicancesTabVisible;
        private string _appliancesTitle;

        public string LoginTitle
        {
            get { return _loginTitle; }
            set
            {
                _loginTitle = value;
                OnPropertyChanged();
            }
        }

        public string AppliancesTitle
        {
            get { return _appliancesTitle; }
            set
            {
                _appliancesTitle = value;
                OnPropertyChanged();
            }
        }



        private string _emailText;

        public string EmailText
        {
            get { return _emailText; }
            set
            {
                _emailText = value;
                OnPropertyChanged();
            }
        }

        private string _passwordText;

        public string PasswordText
        {
            get { return _passwordText; }
            set
            {
                _passwordText = value;
                OnPropertyChanged();
            }
        }


        public List<Appliance> Appliances
        {
            get
            {
                if (base.Client.IsAuthenticated && base.Client.User != null)
                {
                    return base.Client.User.Appliances;
                }
                else
                {
                    return null;
                }
            }
        }


    }
}