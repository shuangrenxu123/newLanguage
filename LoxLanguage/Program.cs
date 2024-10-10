namespace LoxLanguage
{
    internal class Program
    {
        static bool hadError = false;
        static void Main(string[] args)
        {
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

        static void RunFile(string path)
        {
            var strings = File.ReadAllText(path);
            Run(strings);
            if (hadError)
            {
                throw new Exception("代码执行有错误");
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

            foreach (Token token in tokens)
            {
                Console.WriteLine(token);
            }    
        }

        public static void Error(int line,string message)
        {

        }
        private static void Report(int line,string where,string message)
        {
            Console.WriteLine("[line " + line + "] Error" + where + ": " + message);
            hadError = true;
        }
    }
}
