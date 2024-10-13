using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxLanguage
{
    public class Token
    {
        public TokenType type;
        public string lexeme;
        public Object literal;
        public int line;

        public Token(TokenType type, string lexeme, Object literal, int line)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }

        public override string ToString()
        {
            return $"{type} : {lexeme} : {literal}";
        }
    }
}
