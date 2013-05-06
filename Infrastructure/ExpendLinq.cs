using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace sharp_net {

    public static class ExpendLinq {

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            foreach (var item in source) {
                action(item);
            }
        }

        public static bool IsExist<T>(this IQueryable<T> queryable) {
            return queryable.Count() > 0;
        }

        public static bool IsNotExist<T>(this IQueryable<T> queryable) {
            return queryable.Count() <= 0;
        }

        public static bool IsExistAndSingle<T>(this IQueryable<T> queryable) where T : class {
            return queryable.SingleOrDefault() != null;
        }

        public static Expression<Func<T, bool>> True<T>() {
            return (Expression<Func<T, bool>>)(f => true);
        }

        public static Expression<Func<T, bool>> False<T>() {
            return (Expression<Func<T, bool>>)(f => false);
        }

        //http://stackoverflow.com/questions/457316/combining-two-expressions-expressionfunct-bool 只能给linq to sql 使用
        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2) {
            ParameterExpression param = expr1.Parameters[0];
            if (ReferenceEquals(param, expr2.Parameters[0])) {
                return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, expr2.Body), param);
            }
            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(expr1.Body, Expression.Invoke(expr2, param)),
                param);
        }

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2) {
            ParameterExpression param = expr1.Parameters[0];
            if (ReferenceEquals(param, expr2.Parameters[0])) {
                return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, expr2.Body), param);
            }
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(expr1.Body, Expression.Invoke(expr2, param)), 
                param);
        }

    }
}