namespace LoxLanguage
{
    internal class Program
    {
        static bool hadError = false;
        public static bool hasRuntimeError = false;
        private static Interpreter interpreter = new();
        static void Main(string[] args)
        {     

            //TestTree();
            if(args.Length > 1)
            {
                Console.WriteLine("Usage:los [scrpit]");
            }
            else if(args.Length ==1)
            {
                RunFile(args[0]);
            }
            else
            {
                RunPrompt();
            }
        }

        static void TestTree()
        {
             var expr = new Binary(
                 new Unary(new Token(TokenType.Minus,"-",null,1)
                          ,new Literal(123)),
                 new Token(TokenType.Star,"*",null,1),
                 new Grouping(new Literal(45.67))
             );

            Console.WriteLine((new LoxPrint()).Debug(expr));
        }

        static void RunFile(string path)
        {
            var strings = File.ReadAllText(path);
            Run(strings);
            if (hadError)
            {
                Environment.Exit(65);
            }
            if(hasRuntimeError)
            {
                Environment.Exit(70);
            }
        }

        /// <summary>
        /// 运行提示符
        /// </summary>
        static void RunPrompt()
        {
            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine();
                if (input == null)
                {
                    break;
                }
                Run(input);
                hadError = false;   
            }
        }

        static void Run(string source)
        {
            Scanner scanner = new(source);
            List<Token> tokens = scanner.scanTokens();
            var parser = new Parser(tokens);
            var root = parser.Parse();

            interpreter.Interpret(root);
            //Console.WriteLine((new LoxPrint()).Debug(root));
        }

        public static void Error(int line,string message)
        {
            Console.WriteLine($"在行数：[{line}] 行出现了错误{message}");
        }
        private static void Report(int line,string where,string message)
        {
            Console.WriteLine("[line " + line + "] Error" + where + ": " + message);
            hadError = true;
        }
    }
}
