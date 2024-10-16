namespace LoxLanguage;
public abstract class Stmt
{
    public interface IVisitor<T>
    {
        public T VisitExpressionStmt(Expression stmt);
        public T VisitPrintStmt(Print stmt);
    }

    public abstract T Accept<T>(IVisitor<T> visitor);
}
public class Expression : Stmt
{
    public Expr expression;
    public Expression(Expr expression)
    {
        this.expression = expression;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitExpressionStmt(this);
    }
}
/// <summary>
/// 理论说Print并不是一表达式，这里只是为了提前看到效果才这样写的
/// </summary>
public class Print : Stmt
{
    public Expr expression;
    public Print(Expr expression)
    {
        this.expression = expression;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitPrintStmt(this);
    }
}

