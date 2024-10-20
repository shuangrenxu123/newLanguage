using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return Equality();
        }

        private Stmt Declaration()
        {
            try
            {
                if (Match(TokenType.Var))
                {
                    return VarDeclaration();
                }
                return Statement();
            } catch (Exception e) {
                
                return null;
            }
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
                statements.Add(Statement());
            }
            return statements;
        }
        Stmt Statement()
        {
            if (Match(TokenType.Print))
            {
                return PrintStatement();
            }
            if (Match(TokenType.Var))
            {
                return VarDeclaration();
            }
            return ExpressionStatement();
        }
        Stmt PrintStatement()
        {
            Expr value = Expression();
            Consume(TokenType.Semicolon,"没有分号结尾");
            return new Print(value);
        }
        Stmt ExpressionStatement()
        {
            Expr expr = Expression();
            Consume(TokenType.Semicolon, "Expect ';' after expression.");
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
    }
}
