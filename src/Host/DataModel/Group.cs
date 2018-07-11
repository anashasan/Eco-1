using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{

    public class Group<K, T>
    {
        public K Key;
        public IEnumerable<T> Values;
    }
}
