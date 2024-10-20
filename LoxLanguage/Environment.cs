using LoxLanguage.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxLanguage
{
    internal class Environment
    {
        private Dictionary<int, object> variables = new();

        public object GetVariables(Token token)
        {
            var hashCode = token.lexeme.GetHashCode();
            if (variables.ContainsKey(hashCode))
            {
                return variables[hashCode];
            }
            throw new RuntimeError(token,$"变量未定义{token.lexeme}");
        }

        /// <summary>
        /// 定义一个变量
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="variable"></param>
        public void Define(string varName,Object variable)
        {
            int hastCode = varName.GetHashCode();
            variables.Add(hastCode,variable);
        }

    }
}
