using System.Collections.Generic;
using JetBrains.Annotations;

namespace Utilities.String
{
    /// <summary>
    /// Provides variants of given name with sequential numbers added to the end of it (Untitled, Untitled1, Untitled2)
    /// </summary>
    public class SequentialNumberNaming
    {
        private readonly object _lock = new object();
        private readonly Dictionary<string,int> _names = new Dictionary<string, int>();

        /// <summary>
        /// Creates a new instance of the <see cref="SequentialNumberNaming"/>.
        /// </summary>
        /// <param name="skipZero">If true, will not append 0 for the first time specified name is requested</param>
        public SequentialNumberNaming(bool skipZero = true)
        {
            SkipZero = skipZero;
        }

        /// <summary>
        /// If true, the first variant for a name will be just the name itself, and subsequent ones will be name1, name2...
        /// </summary>
        [UsedImplicitly]
        public bool SkipZero { get; }

        /// <summary>
        /// Returns a name with a sequential number added to the end of it.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [UsedImplicitly]
        public string Get(string name)
        {
            lock (_lock)
            {
                if (!_names.ContainsKey(name))
                    _names[name] = 0;
                var number = _names[name];
                var numberedName = $"{name}{(number == 0 && SkipZero ? string.Empty : number.ToString())}";
                _names[name] = ++number;
                return numberedName;
            }
        }
    }
}