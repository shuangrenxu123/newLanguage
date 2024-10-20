using LoxLanguage.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LoxLanguage
{
    internal class Environment
    {
        /// <summary>
        /// 外部环境
        /// </summary>
        Environment? enclosing;


        public Environment()
        {
            enclosing = null;
        }
        public Environment(Environment environment)
        {
            enclosing = environment;
        }
        private Dictionary<int, object> variables = new();
        public object GetVariables(Token token)
        {
            var hashCode = token.lexeme.GetHashCode();
            //现在自己的环境中寻找
            if (variables.TryGetValue(hashCode, out object? value))
            {
                return value;
            }
            // 如果有外层环境,那就尝试去外层环境找找
            if (enclosing != null) 
                return enclosing.GetVariables(token);
            throw new RuntimeError(token,$"变量未定义{token.lexeme}");
        }

        /// <summary>
        /// 定义一个变量
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="variable"></param>
        public void Define(string varName,Object variable)
        {
            int hashCode = varName.GetHashCode();
            variables[hashCode] = variable;
        }

        public void Assign(Token name,Object variable)
        {
            int hashCode = name.lexeme. GetHashCode();
            //如果是本环境的值,那就更新一下
            if (variables.ContainsKey(hashCode))
            {
                variables[hashCode] = variable;
                return;
            }
            //尝试更新外层环境
            if (enclosing != null)
            {
                enclosing.Assign(name, variable);
                return;
            }
            throw new RuntimeError(name, $"变量未定义{name.lexeme}");
        }

    }
}
