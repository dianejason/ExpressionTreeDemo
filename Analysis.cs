using System;
using System.Text;
using System.Linq.Expressions;


namespace ExpressionTreeDemo
{
    public class Analysis
    {
        private StringBuilder builder = new StringBuilder();
        public string AnalysisExpression<TDelegate>(Expression<TDelegate> expression)
        {
            if (expression.Body is MemberExpression)
            {
                var m = (MemberExpression)expression.Body;
                var value = Convert.ToInt32(!expression.Body.ToString().Contains("!"));
                builder.Append($"  ({m.Member.Name}={value}) ");
                return builder.ToString();
            }
            var body = (BinaryExpression)expression.Body;
            if (body.NodeType == ExpressionType.AndAlso || body.NodeType == ExpressionType.OrElse)
            {
                AnalysisExpressionChild((BinaryExpression)body.Left, body.NodeType);
                AnalysisExpressionChild((BinaryExpression)body.Right, body.NodeType);
            }
            else
            {
                var r = (ConstantExpression)body.Right;
                var l = (MemberExpression)body.Left;
                var value = r.Type.IsValueType ? r.Value : $"'{r.Value}'";
                builder.Append($" { l.Member.Name} {Operand(body.NodeType)} {value} ");
            }
            return builder.ToString();
        }

        //解析表达式树
        private void AnalysisExpressionChild(BinaryExpression expression, ExpressionType pType, string brackets = "")
        {
            if (expression.NodeType != ExpressionType.AndAlso && expression.NodeType != ExpressionType.OrElse)
            {
                var r = (ConstantExpression)expression.Right;
                var l = (MemberExpression)expression.Left;
                var value = r.Type.IsValueType ? r.Value : $"'{r.Value}'";
                builder.Append($" {Operand(pType)} {brackets} { l.Member.Name} {Operand(expression.NodeType)} {value} ");
            }
            else
            {
                if (expression.NodeType == ExpressionType.OrElse)
                {
                    brackets = "( ";
                }
                AnalysisExpressionChild((BinaryExpression)expression.Left, pType, brackets);
                AnalysisExpressionChild((BinaryExpression)expression.Right, expression.NodeType);
                if (expression.NodeType == ExpressionType.OrElse)
                {
                    builder.Append(" )");
                }
            }
        }

        //操作符转换
        private string Operand(ExpressionType type)
        {
            string operand = string.Empty;
            switch (type)
            {
                case ExpressionType.AndAlso:
                    operand = "AND";
                    break;
                case ExpressionType.OrElse:
                    operand = "OR";
                    break;
                case ExpressionType.Equal:
                    operand = "=";
                    break;
                case ExpressionType.LessThan:
                    operand = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    operand = "<=";
                    break;
                case ExpressionType.GreaterThan:
                    operand = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    operand = ">";
                    break;
            }
            return operand;
        }

    }
}
