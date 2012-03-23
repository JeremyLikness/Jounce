using System;
using System.ComponentModel.Composition;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Threading.Tasks;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using ToDoList.Contracts;

namespace ToDoList.ViewModels
{
    [Export(typeof (IServerStatus))]
    [ExportAsViewModel(typeof (ServerStatusViewModel))]
    public class ServerStatusViewModel : BaseViewModel, IServerStatus
    {
        private const string LAST_SYNC = "LasySyncDate";
        private const string ONLINE = "Online";
        private const string OFFLINE = "Offline";
        private const string NO_ITEMS = "No items to Sync";
        private const string SYNC_OFFLINE = "Waiting to Sync {0} Items";
        private const string SYNC_ONLINE = "Synchronizing {0} Items";
        private const string OFFLINE_NOTIFY = "Server is Offline";
        private const string OFFLINE_TEXT =
            "The server has gone offline. Changes will be saved locally and synchronized when the server comes back online.";        
        private const string ONLINE_NOTIFY = "Server is Online";
        private const string ONLINE_TEXT =
            "The server has come online. The local database is currently synchronizing with the server.";

        [Import]
        public INotification Notification { get; set; }

        [Import]
        public IToDoListApplicationContext AppContext { get; set; }        

        private bool _online;

        public bool Online
        {
            get { return _online; }
            set
            {
                ProcessStatus(value);
                _online = value;
                RaisePropertyChanged(() => Online);
            }
        }

        private void ProcessStatus(bool value)
        {
            if (!AppContext.IsRunningOutOfBrowser || 
                value == _online)
            {
                return;
            }

            var title = value ? ONLINE_NOTIFY : OFFLINE_NOTIFY;
            var text = value ? ONLINE_TEXT : OFFLINE_TEXT;
            Notification.Notify(title, text,
                                TimeSpan.FromSeconds(5));
        }

        private DateTime _lastSyncDate;

        public DateTime LastSyncDate
        {
            get { return _lastSyncDate; }
            set
            {
                _lastSyncDate = value;
                IsolatedStorageSettings.ApplicationSettings[LAST_SYNC] = value;
                RaisePropertyChanged(() => LastSyncDate);                
            }
        }

        private int _itemsToSync;

        public int ItemsToSync
        {
            get { return _itemsToSync; }
            set
            {
                _itemsToSync = value;
                RaisePropertyChanged(() => ItemsToSync);
            }
        }

        public string LastSyncDisplay { get; private set; }

        public string SyncStatus { get; private set; }

        private void UpdateSyncStatus()
        {
            var status =
                ItemsToSync > 0
                    ? string.Format(
                        Online ? SYNC_ONLINE : SYNC_OFFLINE,
                        ItemsToSync)
                    : NO_ITEMS;
            var fullStatus = string.Format("{0}; {1}",
                                           Online ? ONLINE : OFFLINE, status);

            JounceHelper.ExecuteOnUI(() =>
                                         {
                                             SyncStatus = fullStatus;
                                             RaisePropertyChanged(() => SyncStatus);
                                         });
        }

        private void UpdateSyncDate()
        {
            string dateValue;

            var time = DateTime.Now - LastSyncDate;

            var days = (int) Math.Floor(time.TotalDays);
            var hours = (int) Math.Floor(time.TotalHours);
            var minutes = (int) Math.Floor(time.TotalMinutes);
            var seconds = (int) Math.Floor(time.TotalSeconds);

            if (days > 365)
            {
                dateValue = "Never";
            }
            else if (days > 1)
            {
                dateValue = string.Format("{0} days ago",
                                          days);
            }
            else if (days == 1)
            {
                dateValue = "1 day ago";
            }
            else if (hours > 1)
            {
                dateValue = string.Format("{0} hours ago",
                                          hours);
            }
            else if (hours == 1)
            {
                dateValue = "1 hour ago";
            }
            else if (minutes > 1)
            {
                dateValue = string.Format("{0} minutes ago",
                                          minutes);
            }
            else if (minutes == 1)
            {
                dateValue = "1 minute ago";
            }
            else if (seconds > 1)
            {
                dateValue = string.Format("{0} seconds ago",
                                          seconds);
            }
            else
            {
                dateValue = "Now";
            }

            JounceHelper.ExecuteOnUI(() =>
                                         {
                                             LastSyncDisplay = dateValue;
                                             RaisePropertyChanged(() => LastSyncDisplay);
                                         });
        }

        private void StatusUpdates()
        {            
            while (ItemsToSync >= 0)
            {
                UpdateSyncDate();
                UpdateSyncStatus();
                Thread.Sleep(1000);
            }
        }

        protected override void InitializeVm()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(LAST_SYNC))
            {
                LastSyncDate = (DateTime) IsolatedStorageSettings.ApplicationSettings[LAST_SYNC];
            }
            else
            {
                LastSyncDate = DateTime.MinValue;
            }

            Task.Factory.StartNew(
                StatusUpdates,
                TaskCreationOptions.LongRunning);

            base.InitializeVm();
        }        
    }
}