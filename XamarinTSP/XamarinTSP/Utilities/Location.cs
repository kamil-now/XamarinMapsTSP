using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XamarinTSP.Abstractions;

namespace XamarinTSP.Utilities
{
    public class Location : PropertyChangedBase, IDisposable
    {
        public Location()
        {
            _name = "";
            Pin = new Pin() { Label = _name };
        }
        public EventHandler OnDispose;
        public EventHandler OnEdit;
        private bool _startedEdit;
        private volatile object _lck = new object();
        private string _name;
        public Pin Pin { get; set; }
        public string Name// { get; set; }
        {
            get => _name; set
            {
                _name = value;

                //if (!_startedEdit)
                //{
                //    _startedEdit = true;
                //    Helper.InvokeOnMainThreadAsync(() =>
                //    {
                //        if (_startedEdit)
                //        {
                //            _startedEdit = false;
                //            EditFinishedCommand.Execute(null);
                //        }
                //    }, delay: 1000);
                //}
                NotifyOfPropertyChange(() => Name);
            }
        }
        public ICommand EditFinishedCommand => new Command(() =>
        {
            lock (_lck)
            {
                OnEdit.Invoke(null, null);
                NotifyOfPropertyChange(() => Name);
            }

        });
        public void SetPinPosition(Position position)
        {
            Pin.Position = position;
            NotifyOfPropertyChange(() => Pin);
        }

        public void Dispose() => OnDispose(this, null);
    }
}
