using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Neurio.Client;

namespace Neurio.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        public BaseViewModel(NeurioClient neurioClient)
        {
            this.Client = neurioClient;
        }
        public BaseViewModel()
        {
        }

        public NeurioClient Client { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
