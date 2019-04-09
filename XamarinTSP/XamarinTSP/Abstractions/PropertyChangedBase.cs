﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace XamarinTSP.Abstractions
{
    public class PropertyChangedBase
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyOfPropertyChange([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public virtual void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property) => NotifyOfPropertyChange(property.Name);

    }
}
