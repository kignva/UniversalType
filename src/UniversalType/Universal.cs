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

        public Universal(string key, object value)
        {
            _props = new Dictionary<string, object>();
            _props[key] = value;
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

        public Universal SetProperty(string key, object value)
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

        public Universal ToJson(Universal options = null)
        {
            int level = (options != null && options["level"] != null) ? options.Get<int>("level") : 0;
            bool addType = (options != null && options["add-type"] != null)
                ? options.Get<bool>("add-type")
                :
#if (DEBUG)
 true;
#else         
            false;

#endif
            var sb = new StringBuilder("{\n");
            foreach (var item in _props)
            {
                sb.Append(RepeatChar('\t', level + 1));
                sb.AppendFormat("\"{0}\": ", item.Key);

                if (addType)
                {
                    sb.AppendFormat("{{ \"type\": \"{0}\", \"value\": ", item.Value.GetType());
                }

                if (item.Value.GetType() == typeof(string))
                {
                    sb.AppendFormat("\"{0}\"", item.Value);
                }
                else if (item.Value.GetType().IsArray)
                {
                    sb.Append("[");
                    foreach (var v in (Array)item.Value)
                    {
                        sb.Append(v + ", ");
                    }
                    sb.Length = sb.Length - 2;
                    sb.Append("]");
                    //if (item.Value.GetType().GetElementType().IsValueType)
                }
                else if (item.Value.GetType() == typeof(Universal))
                {
                    sb.AppendFormat("{0}", ((Universal)item.Value).ToJson(level > 0
                        ? options.SetProperty("level", level + 1)
                        : new Universal("level", 1))["json"]);
                }
                else
                {
                    sb.AppendFormat("{0}", item.Value);
                }

                if (addType)
                {
                    sb.Append("}");
                }

                sb.Append(",\n");
            }
            if (sb[sb.Length - 1].Equals('\n') && sb[sb.Length - 2].Equals(','))
            {
                sb.Length = sb.Length - 2;
            }
            sb.Append("\n");
            sb.Append(RepeatChar('\t', level));
            sb.Append("}");

            return new Universal("json", sb.ToString());
            //return new Universal("Error", "Not yet implemented.");
        }

        private string RepeatChar(char c, int count)
        {
            var result = new char[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = c;
            }

            return new string(result);
        }


    }
}
