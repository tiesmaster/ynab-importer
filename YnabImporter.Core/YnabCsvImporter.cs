using System.Collections.Generic;
using System.IO;
using System.Linq;

using CsvHelper;
using CsvHelper.Configuration;

namespace YnabImporter.Core
{
    public static class YnabCsvImporter
    {
        private static readonly Configuration _csvWriterConfiguration = new Configuration
        {
            QuoteAllFields = true
        };

        public static string FromRabobank(string rabobankCsvText)
        {
            var inputRecords = ReadRecords(rabobankCsvText);
            var convertedRecords = inputRecords.Select(ConvertToYnabRecord);
            var ynabCsvText = WriteRecords(convertedRecords);

            return ynabCsvText;
        }

        private static IEnumerable<dynamic> ReadRecords(string csvText)
        {
            var csvReader = new CsvReader(new StringReader(csvText));
            return csvReader.GetRecords<dynamic>();
        }

        private static YnabRecord ConvertToYnabRecord(dynamic rawRecord)
        {
            string inAndOutFlowAmount = rawRecord.Bedrag;
            var isOutflow = inAndOutFlowAmount[0] == '-';
            var positiveAmount = inAndOutFlowAmount.Substring(1);

            return new YnabRecord
            {
                Date = rawRecord.Datum,
                Payee = GetPropertyByName(rawRecord, "Naam tegenpartij"),
                Memo = GetPropertyByName(rawRecord, "Omschrijving-1"),
                Outflow = isOutflow ? positiveAmount : null,
                Inflow = isOutflow ? null : positiveAmount
            };
        }

        private static string GetPropertyByName(dynamic firstRecord, string propertyName)
        {
            return (string)((IDictionary<string, object>)firstRecord)[propertyName];
        }

        private static string WriteRecords(IEnumerable<YnabRecord> convertedRecords)
        {
            var innerWriter = new StringWriter();

            var csvWriter = new CsvWriter(innerWriter, _csvWriterConfiguration);
            csvWriter.WriteRecords(convertedRecords);

            return innerWriter.ToString();
        }
    }
}