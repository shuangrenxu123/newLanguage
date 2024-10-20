namespace LoxLanguage;
public abstract class Stmt
{
    public interface IVisitor<T>
    {
        public T VisitExpressionStmt(Expression stmt);
        public T VisitPrintStmt(Print stmt);
        public T VisitVarStmt(Var stmt);
        public T VisitBlockStmt(Block stmt);
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
public class Var : Stmt
{
    public Token name;
    public Expr initializer;
    public Var(Token name, Expr initializer)
    {
        this.name = name;
        this.initializer = initializer;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitVarStmt(this);
    }
}
public class Block : Stmt
{
    public List<Stmt> statements;
    public Block(List<Stmt> statements)
    {
        this.statements = statements;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitBlockStmt(this);
    }
}

