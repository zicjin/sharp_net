using System;
using System.Linq.Expressions;

namespace sharp_net.Specifications {

    /// <summary>
    /// Represents the combined specification which indicates that either of the given
    /// specification should be satisfied by the given object.
    /// </summary>
    /// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
    public class OrSpecification<T> : CompositeSpecification<T> {

        public OrSpecification(ISpecification<T> left, ISpecification<T> right) : base(left, right) { }

        public override Expression<Func<T, bool>> GetExpression() {
            //var body = Expression.OrElse(Left.GetExpression().Body, Right.GetExpression().Body);
            //return Expression.Lambda<Func<T, bool>>(body, Left.GetExpression().Parameters);
            return Left.GetExpression().Or(Right.GetExpression());
        }

    }
}