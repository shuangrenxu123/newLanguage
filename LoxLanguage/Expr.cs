
namespace LoxLanguage;
public abstract class Expr
{
    public interface IVisitor<T>
    {
        public T VisitBinaryExpr(Binary expr);
        public T VisitGroupingExpr(Grouping expr);
        public T VisitLiteralExpr(Literal expr);
        public T VisitUnaryExpr(Unary expr);
    }

    public abstract T Accept<T>(IVisitor<T> visitor);
}
public class Binary : Expr
{
    public Expr left;
    public Token opt;
    public Expr right;
    public Binary(Expr left, Token opt, Expr right)
    {
        this.left = left;
        this.opt = opt;
        this.right = right;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitBinaryExpr(this);
    }
}
public class Grouping : Expr
{
    public Expr expression;
    public Grouping(Expr expression)
    {
        this.expression = expression;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitGroupingExpr(this);
    }
}
public class Literal : Expr
{
    public Object value;
    public Literal(Object value)
    {
        this.value = value;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitLiteralExpr(this);
    }
}
public class Unary : Expr
{
    public Token opt;
    public Expr right;
    public Unary(Token opt, Expr right)
    {
        this.opt = opt;
        this.right = right;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitUnaryExpr(this);
    }
}

