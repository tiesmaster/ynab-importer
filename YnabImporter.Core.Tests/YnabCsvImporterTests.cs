using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace YnabImporter.Core.Tests
{
    public class YnabCsvImporterTests
    {
        [Fact]
        public void SingleRabobankTransaction_CanBeConvertedToYnab()
        {
            // arrange
            var rabobankCsv =
@"""IBAN/BBAN"",""Munt"",""BIC"",""Volgnr"",""Datum"",""Rentedatum"",""Bedrag"",""Saldo na trn"",""Tegenrekening IBAN/BBAN"",""Naam tegenpartij"",""Naam uiteindelijke partij"",""Naam initi�rende partij"",""BIC tegenpartij"",""Code"",""Batch ID"",""Transactiereferentie"",""Machtigingskenmerk"",""Incassant ID"",""Betalingskenmerk"",""Omschrijving-1"",""Omschrijving-2"",""Omschrijving-3"",""Reden retour"",""Oorspr bedrag"",""Oorspr munt"",""Koers""
""NL12RABO1234567890"",""EUR"",""RABONL2U"",""000000000000000001"",""2018-05-01"",""2018-05-01"",""+100,00"",""+100,00"","""",""WERKGEVER"","""","""","""",""ei"","""","""","""","""","""",""Loon maand mei"","" "","""","""","""","""",""""
""NL12RABO1234567890"",""EUR"",""RABONL2U"",""000000000000001234"",""2018-05-04"",""2018-05-04"",""-12,34"",""+7,66"","""",""ALBERT HEIJN 1234 ROTTERDAM"","""","""","""",""bc"","""","""","""","""","""",""Betaalautomaat 14:22 pasnr. 123"","" "","""","""","""","""",""""
";
            // act
            var resultingYnabCsv = YnabCsvImporter.FromRabobank(rabobankCsv);

            // assert
            var expectedYnabCsv =
@"""Date"",""Payee"",""Memo"",""Outflow"",""Inflow""
""2018-05-01"",""WERKGEVER"",""Loon maand mei"","""",""100,00""
""2018-05-04"",""ALBERT HEIJN 1234 ROTTERDAM"",""Betaalautomaat 14:22 pasnr. 123"",""12,34"",""""
";
            Assert.Equal(expectedYnabCsv, resultingYnabCsv);
        }
    }
}