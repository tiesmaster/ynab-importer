﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CsvHelper;
using CsvHelper.Configuration;

namespace YnabImporter.Core
{

    public class YnabRecord
    {
        public string Date { get; set; }
        public string Payee { get; set; }
        public string Memo { get; set; }
        public string Outflow { get; set; }
        public string Inflow { get; set; }
    }
}