using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Ebsco.Shared.Caching.Interfaces
{

    public abstract class NamespacedKey<T> where T : NamespacedKey<T>, new()
    {
        protected virtual string Namespace
        {
            get
            {
                throw new NotImplementedException("Derive from this class to properly namespace caching.");
            }
        }

        private string _key;

        protected string Key
        {
            set { _key = Namespace + "." + value; }
        }

        public static implicit operator string(NamespacedKey<T> nk)
        {
            return nk._key;
        }

        public static implicit operator RedisKey(NamespacedKey<T> nk)
        {
            return (string)nk;
        }

        public static T Create(string prefix, Object keyObject = null)
        {
            if (String.IsNullOrEmpty(prefix)) throw new ArgumentNullException("prefix");
            string s;
            if (keyObject == null) s = prefix;
            else s = String.Format("{0}.{1}", prefix, JsonConvert.SerializeObject(keyObject));
            var t = new T { Key = s };
            return t;
        }
    }


}
