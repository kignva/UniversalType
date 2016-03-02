using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public class Universal
    {
        private readonly IDictionary<string, object> _props;
        //private readonly long _id;

        public Universal()
        {
            _props = new Dictionary<string, object>();
        }

        public object this[string key]
        {
            get
            {
                return _props.ContainsKey(key) ? _props[key] : null;
            }
            set { _props[key] = value; }
        }

        public T Get<T>(string key)
        {
            if (_props.ContainsKey(key) &&
                _props[key] is T)
            { return (T)_props[key]; }

            return default(T);
        }

        public IDictionary<string, object> Properties { get { return _props; } }

        public Universal AddOrUpdateProperty(string key, object value)
        {
            _props[key] = value;

            return this;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var item in _props)
            {
                sb.AppendFormat("{0}, {1}: {2}\n", item.Key, item.Value.GetType(), item.Value);
            }

            return sb.ToString();
        }


    }
}
