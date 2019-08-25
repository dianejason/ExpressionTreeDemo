using System;
using System.Text;
using System.Linq.Expressions;

namespace ExpressionTreeDemo
{
    /// <summary>
    /// 表达式解析辅助类
    /// </summary>
    public class AnalyseExpressionHelper : ExpressionVisitor
    {
        private StringBuilder express = new StringBuilder();
        public string Result { get { return express.ToString(); } }

        public void AnalyseExpression<T>(Expression<Func<T, bool>> expression)
        {
            Visit(expression.Body);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.OrElse)
                express.Append("(");
            Visit(node.Left);
            express.Append($" {node.NodeType.TransferOperand()} ");
            Visit(node.Right);
            if (node.NodeType == ExpressionType.OrElse)
                express.Append(")");
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Type.IsValueType && node.Type != typeof(DateTime))
            {
                express.Append(node.Value);
            }
            else
            {
                express.Append($"'{node.Value}'");
            }
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            express.Append(node.Member.Name);
            return node;
        }
    }
}
