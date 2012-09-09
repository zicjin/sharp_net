using System;
using System.Linq.Expressions;

namespace sharp_net.Specifications {

    public sealed class NoneSpecification<T> : Specification<T> {

        public override Expression<Func<T, bool>> GetExpression() {
            return o => false;
        }

    }
}