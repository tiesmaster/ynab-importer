using Microsoft.AspNetCore.Mvc;

using YnabImporter.Core;

namespace YnabImporter.Api.Controllers
{
    [Route("api/[controller]")]
    public class YnabImporterController : Controller
    {
        [HttpPost]
        public string Post([FromBody]string rabobankCsvText)
        {
            return YnabCsvImporter.FromRabobank(rabobankCsvText);
        }
    }
}