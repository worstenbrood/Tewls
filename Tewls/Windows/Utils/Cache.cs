using System;
using System.Collections.Generic;

namespace Tewls.Windows.Utils
{
    public class Cache<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private readonly Func<TKey, TValue> _getValue;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="getValue">Default value retriever</param>
        public Cache(Func<TKey, TValue> getValue) 
        { 
            _getValue = getValue;
        }

        /// <summary>
        /// Get value based on key with the default value retriever
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public TValue Get(TKey key) 
        { 
            if (_getValue == null) 
            {
                throw new Exception("No default value retriever set.");
            }

            return Get(key, _getValue);
        }

        /// <summary>
        /// Get value based on key with a custom value retriever
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public TValue Get(TKey key, Func<TKey, TValue> getValue)
        {
            if (!TryGetValue(key, out TValue value))
            {
                this[key] = value = getValue(key);
            }

            return value;
        }

        /// <summary>
        /// Indexer uses the default value retriever
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new TValue this[TKey key]
        {
            get
            {
                return Get(key);
            }

            set
            {
                base[key] = value;
            }
        }
    }
}
