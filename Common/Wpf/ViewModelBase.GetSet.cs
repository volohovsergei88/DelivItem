using Standard;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Wpf
{
    public  abstract partial class ViewModelBase : BaseInpc
    {
        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        protected T Get<T>([CallerMemberName] string propertyName = "")
        {
            T value;
            if (_properties.TryGetValue(propertyName, out object _prop))
            {
                value = (T)_prop;
            }
            else
            {
                value = default;
            }
            return value;
        }

        protected void Set<T>(T newValue, [CallerMemberName] string propertyName = "")
        {
            T oldValue = Get<T>(propertyName);
            _properties[propertyName] = newValue;
            Set(ref oldValue, newValue, propertyName);
        }
    }
}
