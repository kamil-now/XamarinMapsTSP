using System;
using System.Collections.Generic;
using System.Text;
using XamarinTSP.Abstractions;

namespace XamarinTSP.Utilities
{
    public class Location : PropertyChangedBase
    {
        private string _name;

        public string Name
        {
            get => _name; set
            {
                _name = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
