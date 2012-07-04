using System;
using System.Linq;
using System.Linq.Expressions;

namespace zic_dotnet.Specifications {


    public class AndNotSpecification<T> : CompositeSpecification<T> {

        public AndNotSpecification(ISpecification<T> left, ISpecification<T> right) : base(left, right) { }

        public override Expression<Func<T, bool>> GetExpression() {
            var bodyNot = Expression.Not(Right.GetExpression().Body);
            var bodyNotExpression = Expression.Lambda<Func<T, bool>>(bodyNot, Right.GetExpression().Parameters);

            return Left.GetExpression().And(bodyNotExpression);
        }

    }
}