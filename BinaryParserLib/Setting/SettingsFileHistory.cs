using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryParserLib.Setting
{
    public class SettingsFileHistory
    {
        private List<string> _history = new List<string>();

        public SettingsFileHistory(StringCollection? storage = null)
        {
            if (storage == null) return;

            foreach (var item in storage)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    _history.Add(item);
                }
            }
        }

        internal void Add(string testPath)
        {
            if (!_history.Contains(testPath))
            {
                _history.Add(testPath);
            }
        }

        internal List<string> GetHistory()
        {
            return _history.ToList();
        }

        internal void SaveToStorage(StringCollection storage)
        {
            storage.Clear();
            foreach (var item in _history)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    storage.Add(item);
                }
            }
        }
    }
}
