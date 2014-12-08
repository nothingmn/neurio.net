using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Neurio.Client;
using Neurio.Commands;

namespace Neurio.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {

        public event LoginComplete OnLoginComplete;

        public LoginViewModel(NeurioClient neurioClient)
            : base(neurioClient)
        {

        }

        private ICommand loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                if (loginCommand == null)
                    loginCommand = new LoginCommand(base.NeurioClient, this);

                (loginCommand as LoginCommand).OnLoginComplete += LoginViewModel_OnLoginComplete;
                return loginCommand;
            }
            set
            {
                loginCommand = value;
            }
        }

        void LoginViewModel_OnLoginComplete(NeurioClient client, Client.Entities.Results.LoginResult result)
        {
            if (result.Success)
            {
                StatusText = "Login was successful.";

            }
            else
            {
                StatusText = "Login failed.  Try again.";
                
            }
            if (OnLoginComplete != null) OnLoginComplete(client, result);
        }
        private string _statusText;
        public string StatusText
        {
            get { return _statusText; }
            set { _statusText = value; OnPropertyChanged(); }
        }

        private string _passwordLabelText;
        public string PasswordLabelText
        {
            get { return _passwordLabelText; }
            set { _passwordLabelText = value; OnPropertyChanged(); }
        }

        private string _emailLabelText;
        public string EmailLabelText
        {
            get { return _emailLabelText; }
            set { _emailLabelText = value; OnPropertyChanged(); }
        }

        private string _loginButtonText;
        public string LoginButtonText
        {
            get { return _loginButtonText; }
            set { _loginButtonText = value; OnPropertyChanged(); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }


    }
}
