namespace LoxLanguage;
public abstract class Stmt
{
    public interface IVisitor<T>
    {
        public T VisitExpressionStmt(Expression stmt);
        public T VisitPrintStmt(Print stmt);
        public T VisitIfStmt(If stmt);
        public T VisitFunctionStmt(Function stmt);
        public T VisitVarStmt(Var stmt);
        public T VisitWhileStmt(While stmt);
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
public class If : Stmt
{
    public Expr condition;
    public Stmt thenBranch;
    public Stmt elseBranch;
    public If(Expr condition, Stmt thenBranch, Stmt elseBranch)
    {
        this.condition = condition;
        this.thenBranch = thenBranch;
        this.elseBranch = elseBranch;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitIfStmt(this);
    }
}
public class Function : Stmt
{
    public Token name;
    public List<Token> parms;
    public List<Stmt> body;
    public Function(Token name, List<Token> parms, List<Stmt> body)
    {
        this.name = name;
        this.parms = parms;
        this.body = body;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitFunctionStmt(this);
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
public class While : Stmt
{
    public Expr condition;
    public Stmt body;
    public While(Expr condition, Stmt body)
    {
        this.condition = condition;
        this.body = body;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitWhileStmt(this);
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

