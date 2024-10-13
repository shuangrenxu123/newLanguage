using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxLanguage
{
    /// <summary>
    /// 一个访问者模式，可以通过该方法来处理表达式的规约执行
    /// 这样我们就不需要在外面通过基类来不断规约了
    /// 也算是一种设计模式，这样我们想添加一个新的处理的时候在这类里面加就行了
    /// 
    /// </summary>
    internal class LoxPrint : Expr.IVisitor<string>
    {
        public string VisitBinaryExpr(Binary expr)
        {
            return Parenthesize(expr.opt.lexeme,
                        new List<Expr>() { expr.left, expr.right });
        }

        public string VisitGroupingExpr(Grouping expr)
        {
            return Parenthesize("group",
                       new List<Expr>() { expr.expression });
        }

        public string VisitLiteralExpr(Literal expr)
        {
            if (expr.value == null)
                return "nil";
            
            return expr.value.ToString();
        }

        public string VisitUnaryExpr(Unary expr)
        {
            return Parenthesize(expr.opt.lexeme,
                        new List<Expr>() { expr.right});
        }

        public string Debug(Expr ex)
        {
            //执行表达式，
            return ex.Accept(this);
        }
        /// <summary>
        /// 该函数会会打印表达式
        /// </summary>
        /// <param name="name"></param>
        /// <param name="exprs"></param>
        /// <returns></returns>
        string Parenthesize(string name,List<Expr> exprs)
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            stringBuilder.Append("(")
                .Append(name);
            //递归处理里面的函数
            foreach (Expr e in exprs) {
                stringBuilder.Append(" ");
                stringBuilder.Append(e.Accept(this));
                }
            
            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }

    }
}
