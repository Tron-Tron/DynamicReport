using DynamicReport.Services.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicReport.Models;
using System.Data;

namespace DynamicReport.Services.Interfaces
{
    public interface ITableResponsitory
    {
        Task<IList<TableModelView>> GetTableModelViewsAsync();
        // public Task<string> GetStringQueryAsync(IList<TableQuery> tableQueries);
        Task<string> GenerateSqlSelectAsync(IList<TableQuery> tableQueries);
         Task<DataSet> GetDataQueryAsync(string Query);
    }
}
