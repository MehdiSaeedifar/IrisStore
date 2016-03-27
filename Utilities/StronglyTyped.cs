using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Utilities
{
    public class StronglyTyped : ExpressionVisitor
    {
        public static string PropertyName<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            return new StronglyTyped().Name(expression);
        }

        private Stack<string> _stack;
        public string Path(Expression expression)
        {
            _stack = new Stack<string>();
            Visit(expression);
            return _stack.Aggregate((s1, s2) => s1 + "." + s2);
        }

        protected override Expression VisitMember(MemberExpression expression)
        {
            if (_stack != null)
                _stack.Push(expression.Member.Name);
            return base.VisitMember(expression);
        }

        public string Name<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            return Path(expression);
        }
    }
}