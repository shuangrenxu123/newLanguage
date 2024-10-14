using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxLanguage.Error
{
    internal class RuntimeError :Exception
    {
        public Token token;
        public RuntimeError(Token token,string messsage):base(messsage)
        {
            this.token = token;
        }

    }
}
