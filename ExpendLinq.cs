using System;
using System.Collections.Generic;

namespace zic_dotnet {

    public static class ExpendLinq {

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            foreach (var item in source) {
                action(item);
            }
        }
    }
}