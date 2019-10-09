using System;

namespace SortFiles
{
    class Program : ServiceHost.Startup
    {
        public Program()
            : base("SortFiles")
        { }

        static void Main(string[] args)
            => new Program().Run(args);
    }
}
