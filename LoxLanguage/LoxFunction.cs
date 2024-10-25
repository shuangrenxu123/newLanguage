using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxLanguage
{
    internal class LoxFunction : LoxCallable
    {
        private Function declaration;
        public override int Arity => declaration.parms.Count;
        public LoxFunction( Function function)
        {
            this.declaration = function;
        }
        public override object Call(Interpreter interpreter, List<object> args)
        {
            Environment environment = new Environment(interpreter.globals);
            for (int i = 0; i < declaration.parms.Count; i++) {

                environment.Define(declaration.parms[i].lexeme,
                    args[i]);
            }

            interpreter.ExecuteBlock(declaration.body,environment);
            return null;
        }
        public override string ToString()
        {
            return "<fn " + declaration.name.lexeme + ">";
        }
    }
}
