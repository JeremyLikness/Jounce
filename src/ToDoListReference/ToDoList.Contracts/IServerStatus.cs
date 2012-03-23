using System;

namespace ToDoList.Contracts
{
    public interface IServerStatus
    {
        bool Online { get; set; }
        DateTime LastSyncDate { get; set; }
        int ItemsToSync { get; set; }
        string LastSyncDisplay { get; }
        string SyncStatus { get; }
    }
}