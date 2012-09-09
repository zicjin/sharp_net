using System;
using System.Linq.Expressions;

namespace sharp_net.Specifications {

    internal sealed class ExpressionSpecification<T> : Specification<T> {
        private Expression<Func<T, bool>> expression;

        public ExpressionSpecification(Expression<Func<T, bool>> expression) {
            this.expression = expression;
        }

        public override Expression<Func<T, bool>> GetExpression() {
            return this.expression;
        }

    }

}