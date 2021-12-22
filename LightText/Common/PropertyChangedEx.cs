using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightText.Common
{
    public class PropertyChangedEx : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                WritePropertyChanged(propertyName, oldValue, newValue);
        }

        public virtual void WritePropertyChanged(string propertyName, object oldValue, object newValue)
        {

        }
    }
}
