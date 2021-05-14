using Caliburn.Micro;
using Flurl.Http.Configuration;
using GeoWrapper.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace geo1.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        private string _server;
        private string _login;
        private string _password;

        public string Server
        {
            get { return _server; }
            set
            {
                _server = value;
                NotifyOfPropertyChange(() => Server);
                NotifyOfPropertyChange(() => CanSend);
            }
        }

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                NotifyOfPropertyChange(() => Login);
                NotifyOfPropertyChange(() => CanSend);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanSend);
            }
        }

        public ObservableCollection<ListBoxCollectionItem> ListBoxCollection { get; set; }

        public MainViewModel()
        {
            Server = "http://localhost:8080/geoserver/rest";
            Login = "admin";
            Password = "geoserver";
            ListBoxCollection = new ObservableCollection<ListBoxCollectionItem>();
        }

        public bool CanSend
        {
            get { return !string.IsNullOrWhiteSpace(Server) && !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password); }
        }

        private void Print(string s)
        {
            ListBoxCollection.Add(new ListBoxCollectionItem(s));
        }

        public async void Send()
        {
            //try
            //{
            //    var wc = new WorkspaceService(new PerBaseUrlFlurlClientFactory(), Server, Login, Password);
            //    var workSpaceList = await wc.GetWorkspaces();
            //    foreach (var workspace in workSpaceList)
            //    {
            //        Print($"Name: {workspace.Name}");
            //        Print($"Href: {workspace.Href}");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Print(ex.Message);
            //}

            try
            {
                var wc = new WorkspaceService(new PerBaseUrlFlurlClientFactory());
                wc.Configure(Server, Login, Password);
                var IsCreated = await wc.AddWorkspace("test");
                Print($"Is created: {IsCreated}");
            }
            catch (Exception ex)
            {
                Print(ex.Message);
            }
        }

    }

    public class ListBoxCollectionItem
    {
        public string ListBoxItem {get; set;}
        public ListBoxCollectionItem(string listBoxItem)
        {
            ListBoxItem = listBoxItem;
        }
    }
}