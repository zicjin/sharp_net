using System;

namespace zic_dotnet.Domain {

    public interface IEntity<TKey> {
        TKey ID { get; set; }
    }
}