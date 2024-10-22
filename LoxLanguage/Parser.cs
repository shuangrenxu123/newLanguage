using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LoxLanguage
{
    internal class Parser
    {
        private List<Token> tokens;
        /// <summary>
        /// 待解析token下标
        /// </summary>
        private int current = 0;
        private bool isEnd => tokens[current].type == TokenType.EOF;
        /// <summary>
        /// 解析语句
        /// </summary>
        /// <returns></returns>
        private Expr Expression()
        {
            return Assigment();
        }


        private Stmt VarDeclaration()
        {
            Token name = Consume(TokenType.Identifier,"变量名不是一个合法的标识符");
            
            //赋值符号左边的 表达式
            Expr initExpr = null;
            if (Match(TokenType.Equal))
            {
                initExpr = Expression();
            }
            Consume(TokenType.Semicolon,"语句没有通过 ； 结尾");
            return new Var(name, initExpr);
        }

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }
        public List<Stmt> Parse()
        {
            List<Stmt> statements = new List<Stmt>();
            while (!isEnd)
            {
                statements.Add(Declaration());
            }
            return statements;
        }

        /// <summary>
        /// 检测变量声明语句
        /// </summary>
        /// <returns></returns>
        private Stmt Declaration()
        {
            try
            {
                if (Match(TokenType.Var))
                {
                    return VarDeclaration();
                }
                return Statement();
            }
            catch (Exception e)
            {

                return null;
            }
        }
        Stmt Statement()
        {

            if (Match(TokenType.If))
            {
                return IfStatement();
            }
            if (Match(TokenType.Print))
            {
                return PrintStatement();
            }
            if (Match(TokenType.While))
            {
                return WhileStatement();
            }
            if (Match(TokenType.For))
            {
                return ForStatement();
            }
            if (Match(TokenType.Left_Brace))
            {
                return new Block(Block());
            }
            return ExpressionStatement();
        }

        private Stmt ForStatement()
        {
            Consume(TokenType.Left_Paren, "For语句后必须加上(");
            Stmt initExpr;
            if (Match(TokenType.Semicolon))
            {
                initExpr = null;
            }
            else if (Match(TokenType.Var))
            {
                initExpr = VarDeclaration();
            }
            else
            {
                initExpr = ExpressionStatement();
            }
            // 新增部分开始
            Expr condition = null;
            if (!CheckCurrentTokenType(TokenType.Semicolon))
            {
                condition = Expression();
            }
            Consume(TokenType.Semicolon, "For条件判断表达式需要以；结尾");

            Expr updateExpr = null;
            if (!CheckCurrentTokenType(TokenType.Right_Paren))
            {
                updateExpr = Expression();
            }
            Consume(TokenType.Right_Paren, "For语句后必须加上)");

            Stmt body = Statement();
            if (updateExpr != null)
            {
                body = new Block(new List<Stmt>() {body,new Expression(updateExpr) });
            }
            if (condition == null) 
                condition = new Literal(true);
            body = new While(condition, body);

            if (initExpr != null)
            {
                body = new Block ( new List<Stmt>() {initExpr,body } );
            }
            return body;
        }

        private While WhileStatement()
        {
            Consume(TokenType.Left_Paren,"while关键字后必须加上（");
            var condition = Expression();
            Consume(TokenType.Right_Paren, "whild条件后必须跟上）");
            //获得执行体，不一定是由大括号组成的
            Stmt body = Statement();

            return new While(condition,body);
        }

        private Stmt IfStatement()
        {
            Consume(TokenType.Left_Paren, "if关键字后面必须接上左括号");
            Expr condition = Expression();
            Consume(TokenType.Right_Paren, "if后面必须接后括号");

            Stmt thenBranch = Statement();
            Stmt? elseBranch = null;
            if (Match(TokenType.Else))
            {
                elseBranch = Statement();
            }
            return new If(condition,thenBranch,elseBranch);
        }

        Stmt PrintStatement()
        {
            Expr value = Expression();
            Consume(TokenType.Semicolon,"没有分号结尾");
            return new Print(value);
        }
        /// <summary>
        /// 获得一个普通语句
        /// </summary>
        /// <returns></returns>
        Stmt ExpressionStatement()
        {
            Expr expr = Expression();
            Consume(TokenType.Semicolon, "没有分号结尾");
            return new Expression(expr);
        }

        /// <summary>
        /// 取出并推进一个Token
        /// </summary>
        /// <returns></returns>
        private Token Advance()
        {
            if (!isEnd)
                current++;
            return Previous();
        }

        private Token Peek()
        {
            return tokens[current];
        }
        /// <summary>
        /// 取出上一个访问过的Token,但是不推进
        /// </summary>
        private Token Previous()
        {
            return tokens[current - 1];
        }
        /// <summary>
        /// 检查当前的Token类型是否相同
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool CheckCurrentTokenType(TokenType type)
        {
            if (isEnd)
                return false;
            return type == Peek().type;
        }
        private bool Match(params TokenType[] tokens)
        {
            foreach (TokenType type in tokens)
            {
                if (CheckCurrentTokenType(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// != 或 == 运算符解析
        /// </summary>
        /// <returns></returns>
        private Expr Equality()
        {
            Expr left = Comparison();
            //当执行导致这里的时候，已经是将优先级比他高的解析完毕了
            //并返回了树的根节点
            while (Match(TokenType.Bang_Equal, TokenType.Equal_Equal))
            {
                //当我们访问到了一个 != 或 == 就会将得到的上一个树作为他的子节点
                //然后去生成他的右节点，这样该type与下个type之间的token就被消耗完毕了，
                //然后将该节点作为了 根节点返回回去
                //如果while执行了不止一次的话，那在循环体内会变成左子树
                Token opt = Previous();
                Expr right = Comparison();
                left = new Binary(left, opt, right);
            }

            return left;
        }
        /// <summary>
        /// 比较运算符 > >= < <=
        /// </summary>
        /// <returns></returns>
        private Expr Comparison()
        {
            Expr left = Term();
            
            while(Match(TokenType.Greater_Equal,TokenType.Greater,
                    TokenType.Less,TokenType.Less_Equal))
            {
                Token opt = Previous();
                Expr right = Term();
                left = new Binary(left, opt, right);
            }
            return left;
        }

        /// <summary>
        /// 加减
        /// </summary>
        /// <returns></returns>
        private Expr Term()
        {
            Expr left = Factor();
            while (Match(TokenType.Minus, TokenType.Plus))
            {
                Token opt = Previous();
                Expr right = Factor();
                left = new Binary(left,opt,right);
            }
            return left;
        }
        /// <summary>
        /// 乘除
        /// </summary>
        /// <returns></returns>
        private Expr Factor()
        {
            Expr left = Unary();
            while (Match(TokenType.Slash, TokenType.Star))
            {
                Token opt = Previous();
                Expr right = Unary();
                left = new Binary(left,opt,right);
            }

            return left;
        }
        /// <summary>
        /// 一元运算符
        /// </summary>
        /// <returns></returns>
        private Expr Unary()
        {
            if (Match(TokenType.Bang,TokenType.Bang_Equal))
            {
                Token opt = Previous();
                Expr right = Unary();
                return new Unary(opt,right);
            }
            return Primary();
        }

        /// <summary>
        /// 最终值
        /// </summary>
        /// <returns></returns>
        private Expr Primary()
        {
            var token = Advance();
            Expr? result;
            result = token.type switch
            {
                TokenType.False => new Literal(false),
                TokenType.True => new Literal(true),
                TokenType.Nil => new Literal(null),
                TokenType.Number or TokenType.Strings => new Literal(Previous().literal),
                _ => null
            };

            if(token.type == TokenType.Identifier)
            {
                return new Variable(token);
            }

            if(token.type == TokenType.Left_Paren)
            {
                Expr expr = Expression();
                Consume(TokenType.Right_Paren,"没有识别到右括号");
                return new Grouping(expr);
            }
            if(result == null)
            {
                Program.Error(token.line, "识别到未知的类型:"+ token.type);
            }
            return result;
        }
        /// <summary>
        /// 检测当前待使用的Tokentype，如果不匹配则报错
        /// </summary>
        /// <param name="type"></param>
        /// <param name="errMessage"></param>
        private Token Consume(TokenType type,string errMessage)
        {
            if (CheckCurrentTokenType(type))
            {
                return Advance();
            }
            else
            {
                Program.Error(Peek().line,errMessage);
                return null;
            }
        }

        private Expr Assigment()
        {
            ///拿到左侧表达式结果, 由于赋值语句的优先级比判断还低，
            ///所以我们先来执行检测赋值
            ///这里是拿到了左侧的表达式树，如果没有后续，那就返回。
            //Expr left = Equality();
            Expr left = Or(); 
            if (Match(TokenType.Equal))
            {
                Token equals = Previous();
                //形成一个赋值的语法树
                Expr value = Assigment();

                if(left is Variable)
                {
                    Variable? variable = (left as Variable);
                    Token name = variable.name;
                    return new Assign(name, value);
                }
                Program.Error(equals.line,"错误的赋值目标");
            }


            return left;
        }

        private Expr Or()
        {
            Expr left = And();
            while (Match(TokenType.Or))
            {
                Token opt = Previous();
                Expr right = And();
                left = new Logical(left,opt,right);
            }
            return left;
        }
        private Expr And()
        {
            Expr left = Equality();
            while (Match(TokenType.And))
            {
                Token opt = Previous();
                Expr right = Equality();
                left = new Logical(left,opt,right);
            }
            return left;
        }
        public List<Stmt> Block()
        {
            List<Stmt> result = new();
            //匹配后括号
            while(!CheckCurrentTokenType(TokenType.Right_Brace) && !isEnd)
            {
                result.Add(Declaration());
            }
            Consume(TokenType.Right_Brace, "没有检测到后括号 }");
            return result;

        }
        
    }
}
