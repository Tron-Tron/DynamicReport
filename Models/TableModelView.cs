using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicReport.Models
{
    public class RelationShipInfoView
    {
        public string TableFK { get; set; }
        public string FK { get; set; }
        public string TablePK { get; set; }
        public string PK { get; set; }
        public bool IsUnique { get; set; }
        public string Relationship { get; set; }
    }
    public class TableModelView
    {
        public string Name { get; set; }
        public IList<string> Fields { get; set; }
        public IList<string> Types { get; set; }
        public IList<string> RelationShips { get; set; }
        public IList<RelationShipInfoView> RelationShipInfos { get; set; }
 

    }
}
