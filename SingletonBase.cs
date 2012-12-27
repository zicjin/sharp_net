using System;
using System.Collections.Generic;
using System.Linq;

namespace sharp_net {
    public class SingletonBase<T> where T : new() {
        public static readonly T Instance = new T();
    }
    //public class MySingleton2 : SingletonBase<MySingleton2>{}
}