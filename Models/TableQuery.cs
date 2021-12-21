using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicReport.Models
{
    public class TableQuery
    {
        public string Field { get; set; }
        public string Table { get; set; }
        public string Sort { get; set; }
        public bool? Show { get; set; }
        public string Criteria { get; set; }
        public string Or { get; set; }
        public string JoinOn { get; set; }
        public string GetSelectField()
        {
            return $"{Table}.{Field}";
        }
    }
}
