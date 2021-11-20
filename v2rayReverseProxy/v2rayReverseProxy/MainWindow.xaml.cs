using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using v2rayReverseProxy.Model;
using v2rayReverseProxy.ViewModel;

namespace v2rayReverseProxy
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
        string settingsPath = "设置.json";

        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists(settingsPath))
            {
                string json = File.ReadAllText(settingsPath, Encoding.Default);
                mainWindowViewModel = JsonConvert.DeserializeObject<MainWindowViewModel>(json);
            }
            else
            {
                mainWindowViewModel.ReverseGuid = Guid.NewGuid().ToString();
                mainWindowViewModel.AnotherGuid = Guid.NewGuid().ToString();
            }
            DataContext = mainWindowViewModel;
        }

        public void CreateClient()
        {
            ConfigModel configModel = new ConfigModel();
            configModel.SetSimpleBridges();
            if (mainWindowViewModel.LogInfo)
            {
                configModel.log = new Log() { loglevel = "info" };
            }
            configModel.outbounds.First().settings.vnext.First().address = mainWindowViewModel.ReverseAddress;
            configModel.outbounds.First().settings.vnext.First().port = mainWindowViewModel.ReversePort;
            configModel.outbounds.First().settings.vnext.First().users.First().id = mainWindowViewModel.ReverseGuid;

            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Formatting = Formatting.Indented;
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            string json = JsonConvert.SerializeObject(configModel, jsonSerializerSettings);
            string bridgeDir = "公司或学校bridges配置";
            if (!Directory.Exists(bridgeDir))
            {
                Directory.CreateDirectory(bridgeDir);
            }
            File.WriteAllText(Path.Combine(bridgeDir, "config.json"), json, Encoding.Default);
        }

        public void CreateServer()
        {

            ConfigModel configModel = new ConfigModel();
            configModel.SetSimplePortals();
            if (mainWindowViewModel.LogInfo)
            {
                configModel.log = new Log() { loglevel = "info" };
            }
            configModel.inbounds.Insert(0, new Inbounds()
            {
                port = mainWindowViewModel.Socks5Port,
                protocol = "socks"
            });

            configModel.inbounds.First(x => x.protocol == "vmess").settings.clients.First().id = mainWindowViewModel.ReverseGuid;
            configModel.inbounds.First(x => x.protocol == "vmess").port = mainWindowViewModel.ReversePort;

            if (mainWindowViewModel.AcceptAnotherInbound)
            {
                configModel.inbounds.Insert(1, new Inbounds()
                {
                    port = mainWindowViewModel.AnotherPort,
                    protocol = "vmess",
                    settings = new Settings()
                    {
                        clients = new List<Clients>
                        {
                            new Clients()
                            {
                                id=mainWindowViewModel.AnotherGuid,
                                alterId=mainWindowViewModel.AnotherPort
                            }
                        }
                    }
                });
            }

            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Formatting = Formatting.Indented;
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            string json = JsonConvert.SerializeObject(configModel, jsonSerializerSettings);
            string bridgeDir = "家里或服务器portals配置";
            if (!Directory.Exists(bridgeDir))
            {
                Directory.CreateDirectory(bridgeDir);
            }
            File.WriteAllText(Path.Combine(bridgeDir, "config.json"), json, Encoding.Default);
        }

        private void ButtonCreateNewGuid_Cilck(object sender, RoutedEventArgs e)
        {
            string Id = Guid.NewGuid().ToString();
            if ((sender as Button).Tag.ToString() == "0")
            {
                mainWindowViewModel.ReverseGuid = Id;
            }
            else if ((sender as Button).Tag.ToString() == "0")
            {
                mainWindowViewModel.AnotherGuid = Id;
            }
        }

        private void ButtonCreateConfig_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(mainWindowViewModel.ReverseAddress))
            {
                MessageBox.Show("需要填写域名或者ip地址");
                return;
            }
            string json = JsonConvert.SerializeObject(mainWindowViewModel, Formatting.Indented);
            File.WriteAllText(settingsPath, json, Encoding.Default);
            CreateClient();
            CreateServer();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
        }
    }
}
