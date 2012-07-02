using System;
using System.Linq.Expressions;

namespace zic_dotnet.Specifications {

    public sealed class AnySpecification<T> : Specification<T> {

        public override Expression<Func<T, bool>> GetExpression() {
            return o => true;
        }

    }
}