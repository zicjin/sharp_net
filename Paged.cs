﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zic_dotnet {
    public class PagedResult<T> {
        public IList<T> Result { get; set; }
        public int Count { get; set; }
    }
}
