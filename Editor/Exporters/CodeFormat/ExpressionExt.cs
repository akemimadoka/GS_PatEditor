using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Exporters.CodeFormat
{
    public static class ExpressionExt
    {
        public static Expression MakeIndex(this Expression expr, string index)
        {
            return new IndexExpr(expr, index);
        }

        public static Expression Assign(this Expression expr, Expression val)
        {
            return new BiOpExpr(expr, val, BiOpExpr.Op.Assign);
        }

        public static Expression Call(this Expression expr, params Expression[] args)
        {
            return new FunctionCallExpr(expr, args);
        }

        public static ILineObject Statement(this Expression expr)
        {
            return new ExprStatement(expr);
        }

        public static Expression GreaterZero(this Expression expr)
        {
            return new BiOpExpr(expr, new ConstNumberExpr(0), BiOpExpr.Op.Greater);
        }

        public static Expression LessZero(this Expression expr)
        {
            return new BiOpExpr(expr, new ConstNumberExpr(0), BiOpExpr.Op.Less);
        }

        public static Expression NotZero(this Expression expr)
        {
            return new BiOpExpr(expr, new ConstNumberExpr(0), BiOpExpr.Op.NotEqual);
        }

        public static Expression IsZero(this Expression expr)
        {
            return new BiOpExpr(expr, new ConstNumberExpr(0), BiOpExpr.Op.Equal);
        }

        public static Expression AndAll(params Expression[] expr)
        {
            if (expr == null || expr.Length == 0)
            {
                return new ConstNumberExpr(1);
            }
            var ret = expr[0];
            for (int i = 1; i < expr.Length; ++i)
            {
                ret = new BiOpExpr(ret, expr[i], BiOpExpr.Op.And);
            }
            return ret;
        }

        public static Expression OrAll(params Expression[] expr)
        {
            if (expr == null || expr.Length == 0)
            {
                return new ConstNumberExpr(1);
            }
            var ret = expr[0];
            for (int i = 1; i < expr.Length; ++i)
            {
                ret = new BiOpExpr(ret, expr[i], BiOpExpr.Op.Or);
            }
            return ret;
        }
    }
}
