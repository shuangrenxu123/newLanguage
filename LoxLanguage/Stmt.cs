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
/// ����˵Print������һ���ʽ������ֻ��Ϊ����ǰ����Ч��������д��
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

