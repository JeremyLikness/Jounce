using System.Collections.ObjectModel;

namespace VSMAggregator.Contracts
{
    public interface IBlueViewModel
    {
        ObservableCollection<string> Dates { get; }
    }
}