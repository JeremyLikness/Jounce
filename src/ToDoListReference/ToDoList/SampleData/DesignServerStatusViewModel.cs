using System;
using ToDoList.Contracts;

namespace ToDoList.SampleData
{
#if DEBUG 
    public class DesignServerStatusViewModel : IServerStatus 
    {
        public bool Online
        {
            set { }
            get { return false; }
        }

        public DateTime LastSyncDate
        {
            set { }
            get { return DateTime.Now; }
        }

        public int ItemsToSync
        {
            set { }
            get { return 2; }
        }

        public string LastSyncDisplay
        {
            get { return "3 Hours Ago"; }
        }

        public string SyncStatus
        {
            get { return "Offline; Waiting to Synchronize 5 Items"; }
        }
    }
#endif
}