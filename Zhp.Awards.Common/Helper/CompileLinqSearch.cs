using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Common.Helper
{
    public static class CompileLinqSearch
    {
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }

        //System.Linq.Expressions.
        private class ReplaceExpressionVisitor : System.Linq.Expressions.ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            /// <summary>
            /// 重写基类visit-  如果修改了该表达式或任何子表达式，则为修改后的表达式；否则返回原始表达式。
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }
    }

   
}
