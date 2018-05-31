using System;
using System.IO;

using YnabImporter.Core;

namespace YnabImporter.Cli
{
    internal static class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: ynabimporter [RABOBANK CSV FILE] [YNAB CSV FILE]");

                return 2;
            }

            var rabobankCsvFile = args[0];
            var ynabCsvFile = args[1];

            var rabobankCsvText = File.ReadAllText(rabobankCsvFile);

            var ynabCsvText = YnabCsvImporter.FromRabobank(rabobankCsvText);

            File.WriteAllText(ynabCsvFile, ynabCsvText);

            return 0;
        }
    }
}