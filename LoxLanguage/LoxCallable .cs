using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxLanguage
{
    public abstract class LoxCallable
    {
        public virtual int Arity { get; set; }
        public abstract object Call
            (Interpreter interpreter, List<object> args);
    }
}
