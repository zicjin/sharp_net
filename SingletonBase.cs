using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharp_net {
    public class SingletonBase<T> where T : new() {
        public static readonly T Instance = new T();
    }
}
