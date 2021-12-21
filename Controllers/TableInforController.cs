using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicReport.Models;
using DynamicReport.Services.Implements;
using DynamicReport.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DynamicReport.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TableInforController : ControllerBase
    {
        private readonly ITableResponsitory _TableResponsitory;
        public TableInforController(ITableResponsitory tableResponsitory)
        {
            _TableResponsitory = tableResponsitory;
        }

        public async Task<IActionResult> GetTableInfosAsync()
        {
            var Tmps = await _TableResponsitory.GetTableModelViewsAsync();
            return Ok(Tmps);

        }
        [HttpPost]
        public async Task<IActionResult> GetStringQueryAsync(IList<TableQuery> tableQueries)
        {
            return Ok(await _TableResponsitory.GenerateSqlSelectAsync(tableQueries));
        }

        [HttpPost]
        public async Task<IActionResult> RenderReportAsync(IList<TableQuery> tableQueries)
        {
            var query = await _TableResponsitory.GenerateSqlSelectAsync(tableQueries);
            var dataset = await _TableResponsitory.GetDataQueryAsync(query);
            CustomXtraReport customXtraReport = new CustomXtraReport();
            return Ok(customXtraReport.CreatePdf(dataset));
        }
    }
}
