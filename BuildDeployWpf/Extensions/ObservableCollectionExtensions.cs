using System.Collections.ObjectModel;
using System.Windows;

namespace BuildDeployWpf.Extensions;
public static class ObservableCollectionExtensions
{
    public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items) =>
        AddRangeThreadSafe(collection, items);

    private static void AddRangeThreadSafe<T>(ObservableCollection<T> collection, IEnumerable<T> items)
    {
        if (Application.Current.Dispatcher.CheckAccess())
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
        else
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var item in items)
                {
                    collection.Add(item);
                }
            });
        }
    }
}
