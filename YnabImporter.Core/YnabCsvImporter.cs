using System.Collections.Generic;
using System.IO;
using System.Linq;

using CsvHelper;
using CsvHelper.Configuration;

namespace YnabImporter.Core
{
    public static class YnabCsvImporter
    {
        private static readonly Configuration _csvReaderConfiguration = CreateCsvReaderConfiguration();

        private static readonly Configuration _csvWriterConfiguration = new Configuration
        {
            QuoteAllFields = true
        };

        private static Configuration CreateCsvReaderConfiguration()
        {
            var config = new Configuration();
            config.RegisterClassMap<InputCsvMap>();

            return config;
        }

        public static string FromRabobank(string rabobankCsvText)
        {
            var inputRecords = ReadRecords(rabobankCsvText);
            var convertedRecords = inputRecords.Select(ConvertToYnabRecord);
            var ynabCsvText = WriteRecords(convertedRecords);

            return ynabCsvText;
        }

        private static IEnumerable<RabobankRecord> ReadRecords(string csvText)
        {
            var csvReader = new CsvReader(new StringReader(csvText), _csvReaderConfiguration);
            return csvReader.GetRecords<RabobankRecord>();
        }

        private static YnabRecord ConvertToYnabRecord(RabobankRecord rabobankRecord)
        {
            string inAndOutFlowAmount = rabobankRecord.Bedrag;
            var isOutflow = inAndOutFlowAmount[0] == '-';
            var positiveAmount = inAndOutFlowAmount.Substring(1);

            return new YnabRecord
            {
                Date = rabobankRecord.Datum,
                Payee = rabobankRecord.NaamTegenpartij,
                Memo = rabobankRecord.Omschrijving,
                Outflow = isOutflow ? positiveAmount : null,
                Inflow = isOutflow ? null : positiveAmount
            };
        }

        private static string WriteRecords(IEnumerable<YnabRecord> convertedRecords)
        {
            var innerWriter = new StringWriter();

            var csvWriter = new CsvWriter(innerWriter, _csvWriterConfiguration);
            csvWriter.WriteRecords(convertedRecords);

            return innerWriter.ToString();
        }

        private class InputCsvMap : ClassMap<RabobankRecord>
        {
            public InputCsvMap()
            {
                AutoMap();
                Map(rabo => rabo.NaamTegenpartij).Name("Naam tegenpartij");
                Map(rabo => rabo.Omschrijving).Name("Omschrijving-1");
            }
        }
    }
}