using System.Collections.Generic;

namespace zic_dotnet {

    public class Pager<T> {

        public IEnumerable<T> Result { get; set; }

        public int Count { get; set; }
    }
}