using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CsvHelper;
using CsvHelper.Configuration;

namespace YnabImporter.Core
{
    public static class YnabCsvImporter
    {
        public static string FromRabobank(string csvText)
        {
            var csvReader = new CsvReader(new StringReader(csvText));
            var inputRecords = csvReader.GetRecords<dynamic>();
            var convertedRecords = inputRecords.Select(ConvertToYnabRecord);

            var csvWriterConfiguration = new Configuration
            {
                QuoteAllFields = true
            };

            var innerWriter = new StringWriter();

            var csvWriter = new CsvWriter(innerWriter, csvWriterConfiguration);
            csvWriter.WriteRecords(convertedRecords);

            return innerWriter.ToString();
        }

        private static YnabRecord ConvertToYnabRecord(dynamic firstRecord)
        {
            string amount = firstRecord.Bedrag;
            var isOutflow = amount[0] == '-';

            var result = new YnabRecord
            {
                Date = firstRecord.Datum,
                Payee = GetPropertyWithSpaces(firstRecord, "Naam tegenpartij"),
                Memo = GetPropertyWithSpaces(firstRecord, "Omschrijving-1"),
                Outflow = isOutflow ? amount.Substring(1) : string.Empty,
                Inflow = isOutflow ? string.Empty : amount.Substring(1)
            };

            return result;
        }

        private static string GetPropertyWithSpaces(dynamic firstRecord, string propertyName)
        {
            return (string)((IDictionary<string, object>)firstRecord)[propertyName];
        }
    }

    public class YnabRecord
    {
        public string Date { get; set; }
        public string Payee { get; set; }
        public string Memo { get; set; }
        public string Outflow { get; set; }
        public string Inflow { get; set; }
    }
}