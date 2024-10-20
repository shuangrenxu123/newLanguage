namespace LoxLanguage
{
    internal class Program
    {
        static bool hadError = false;
        public static bool hasRuntimeError = false;
        private static Interpreter interpreter = new();
        static void Main(string[] args)
        {
            RunFile("C:\\Users\\Lenovo\\Desktop\\test.lox");
            //TestTree();
            //if(args.Length > 1)
            //{
            //    Console.WriteLine("Usage:los [scrpit]");
            //}
            //else if(args.Length ==1)
            //{
            //    RunFile(args[0]);
            //}
            //else
            //{
            //    RunPrompt();
            //}
         }

        static void RunFile(string path)
        {
            var strings = File.ReadAllText(path);
            Run(strings);
            if (hadError)
            {
                System.Environment.Exit(65);
            }
            if(hasRuntimeError)
            {
                System.Environment.Exit(70);
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
            var statements = parser.Parse();

            interpreter.Interpret(statements);
            //Console.WriteLine((new LoxPrint()).Debug(root));
        }

        public static void Error(int line,string message)
        {
            Console.WriteLine($"在行数：[{line}] 行出现了错误 : {message}");
        }
        private static void Report(int line,string where,string message)
        {
            Console.WriteLine("[line " + line + "] Error" + where + ": " + message);
            hadError = true;
        }
    }
}
