using System.ComponentModel;
using System.Runtime.CompilerServices;

public static class ObservableHelper
{
    public static void OnPropertyChanged(PropertyChangedEventHandler PropertyChanged, [CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(name, new PropertyChangedEventArgs(name));
    }
}
