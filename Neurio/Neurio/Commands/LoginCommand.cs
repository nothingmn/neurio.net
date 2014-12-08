using System;
using System.Collections.Generic;
using System.Text;
using Neurio.Client;
using Neurio.Client.Entities.Results;
using Neurio.ViewModels;

namespace Neurio.Commands
{
    public delegate void LoginComplete(NeurioClient client, LoginResult result);

    public class LoginCommand : BaseCommand
    {
        public event LoginComplete OnLoginComplete;

        public LoginCommand(NeurioClient neurioClient, BaseViewModel viewModel) : base(neurioClient, viewModel)
        {
        }

        public override async void Execute(object parameter)
        {
            var vm = _viewModel as LandingViewModel;
            vm.StatusText = "Attempting to connect to neur.io";

            var loginResult = (await _neurioClient.Login(vm.EmailText, vm.PasswordText)) as LoginResult;
            vm.StatusText = "Connected to neur.io";
            if (loginResult != null && OnLoginComplete != null)
            {
                vm.StatusText = string.Format("Downloading your profile...");
                var user = (await _neurioClient.LoadCurrentUser()) as CurrentUserResult;
                vm.StatusText = string.Format("Welcome {0}, downloading your appliances...", user.Name);
                foreach (var l in user.Locations)
                {
                    var appliances = await _neurioClient.LoadAppliances(l.Id);
                }
                vm.StatusText = "All done";
                OnLoginComplete(_neurioClient, loginResult);
                
            }

        }
    }
}