using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpcommon {
    public class PagedResult<T> {
        public IQueryable<T> Result { get; set; }
        public int Count { get; set; }
    }
}
