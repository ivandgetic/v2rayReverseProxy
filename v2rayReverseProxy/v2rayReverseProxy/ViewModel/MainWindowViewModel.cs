using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace v2rayReverseProxy.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        public string ReverseGuid { get; set; }
        public string ReverseAddress { get; set; }
        public int ReversePort { get; set; } = 16823;
        public int Socks5Port { get; set; } = 2077;
        public bool LogInfo { get; set; } = true;
        public bool AcceptAnotherInbound { get; set; } = false;
        public string AnotherGuid { get; set; }
        public int AnotherPort { get; set; } = 11872;
        public int AnotherAlterId { get; set; } = 64;
    }
}
