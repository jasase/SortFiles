using System.Collections;
using System.Collections.Generic;

namespace SortFilesPlugin.FileInformationExtractors
{
    public class FileInformation : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly Dictionary<string, object> _dictionary;

        public FileInformation()
        {
            _dictionary = new Dictionary<string, object>();
        }

        public void AddInformation(string key, object value)
        {
            _dictionary.Add(key, value);
        }

        public object this[string key]
        {
            get { return _dictionary[key]; }
            set { _dictionary[key] = value; }
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }
    }
}