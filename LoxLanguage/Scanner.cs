using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxLanguage
{
    internal class Scanner
    {
        string source;
        List<Token> tokens = new();

        int start = 0;
        int current = 0;
        int line = 1;

        bool isEnd => current >= source.Length;
        public Scanner(string source)
        {
            this.source = source;
        }

        public List<Token> scanTokens()
        {
            while (!isEnd)
            {
                start = current;
                ScanToken();
            }
            tokens.Add(new Token(TokenType.EOF,"",null,line));
            return tokens;
        }

        /// <summary>
        /// 扫描Token
        /// </summary>
        void ScanToken() {
            char c = Advance();
            switch (c)
            {
                case '(':
                    AddToken(TokenType.Left_Paren);
                    break;
                case ')':
                    AddToken(TokenType.Right_Paren);
                    break;
                case '{':
                    AddToken(TokenType.Left_Brace);
                    break;
                case '}':
                    AddToken(TokenType.Left_Paren);
                    break;
                case ',':
                    AddToken(TokenType.Comma);
                    break;
                case '.':
                    AddToken(TokenType.Dot);
                    break;
                case '-':
                    AddToken(TokenType.Minus);
                    break;
                case '+':
                    AddToken(TokenType.Plus);
                    break;
                case ';':
                    AddToken(TokenType.Semicolon);
                    break;
                case '*':
                    AddToken(TokenType.Star);
                    break;
                case '!':
                    AddToken(Match('=') ? TokenType.Bang_Equal : TokenType.Bang);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.Less_Equal: TokenType.Less);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.Equal_Equal : TokenType.Equal);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.Greater_Equal : TokenType.Greater);
                    break;
                default:
                    Program.Error(line,"Unexpected character");
                    break;
            }
        }
        
        /// <summary>
        /// 返回下一个字符
        /// </summary>
        /// <returns></returns>
        char Advance()
        {
            current++;
            return source[current - 1];
        }

        /// <summary>
        /// 判断下一个字符是不是我们想要的字符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        bool Match(char c)
        {
            if(isEnd)
                return false;
            if (source[current] != c)
            {
                return false;
            }
            current++;
            return true;
        }
        void AddToken(TokenType type)
        {
            AddToken(type, null);   
        }
        void AddToken(TokenType type,Object? literal)
        {
            string text = source.Substring(start, current);
            tokens.Add(new Token(type,text,literal,line));
        }
    }
}
