﻿using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ICSBel.Presentation.Base;

public abstract class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    public virtual bool HasErrors => false;
    public virtual IEnumerable GetErrors(string propertyName) => Array.Empty<object>();
    
    public event PropertyChangedEventHandler PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    protected void OnErrorsChanged([CallerMemberName] string propertyName = null)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}