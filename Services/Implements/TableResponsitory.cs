using DynamicReport.Common;
using DynamicReport.Models;
using DynamicReport.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicReport.Services.Implements
{
    public class TableResponsitory : ITableResponsitory
    {
        private const string ConnectionStringFormat = @"Data Source={0};Initial Catalog={1};User ID={2};Password={3}";
        private const string ServerName = @"DESKTOP-B4HQDSN\TRONTRON";
        private const string DataBaseName = "TN_CSDLPT";
        private const string UserName = "sa";
        private const string PassWord = "sa";

        public string GetConnectionString()
        {
            return string.Format(ConnectionStringFormat, ServerName, DataBaseName, UserName, PassWord);
        }

        public SqlConnection GetConnect()
        {
            var connectString = GetConnectionString();
            return new SqlConnection(connectString);
        }

      
        private class TableInfoView
        {
            public string TableName { get; set; }
            public string ColumnName { get; set; }
            public string Type { get; set; }
        }
        public async Task<DataSet> GetDataQueryAsync(string Query)
        {
            using var conn = GetConnect();
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                return null;
            }

            using var cmd = conn.CreateCommand();
            cmd.CommandText = Query;
            using var reader = await cmd.ExecuteReaderAsync();

            //Create a new DataSet.
            DataSet dsReports = new DataSet();
            dsReports.Tables.Add("Report");

            //Load DataReader into the DataTable.
            dsReports.Tables[0].Load(reader);
            return dsReports;
        }
        public async Task<IList<TableModelView>> GetTableModelViewsAsync()
        {
            using var conn = GetConnect();
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {

            }
            var listNameTable = (List<TableModelView>)null;
 
            //--------------- Get Fields
            var listTableInfo = new List<TableInfoView>();

            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = Helper.QueryTableString;              
              
                using var reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    listTableInfo.Add(new TableInfoView
                    {
                        TableName = reader[nameof(TableInfoView.TableName)].ToString(),
                        ColumnName = reader[nameof(TableInfoView.ColumnName)].ToString(),
                        Type = reader[nameof(TableInfoView.Type)].ToString(),
                    });
                }


            }
            var listRelationInfo = new List<RelationShipInfoView>();

            {
                using var cmd = conn.CreateCommand();

                cmd.CommandText = Helper.QueryRelationShipString;

                using var reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {

                    var FKtxt = reader[nameof(RelationShipInfoView.FK)].ToString();
                    var IsUniquetxt = Convert.ToBoolean(reader[nameof(RelationShipInfoView.IsUnique)]);
                    var Pktxt = reader[nameof(RelationShipInfoView.PK)].ToString();
                    var TableFKtxt = reader[nameof(RelationShipInfoView.TableFK)].ToString();
                    var TablePKtxt = reader[nameof(RelationShipInfoView.TablePK)].ToString();
                    listRelationInfo.Add(new RelationShipInfoView
                    {
                        FK = FKtxt,
                        IsUnique = IsUniquetxt,
                        PK = Pktxt,
                        TableFK = TableFKtxt,
                        TablePK = TablePKtxt,
                        Relationship = $"{TablePKtxt}({Pktxt}),1-{(IsUniquetxt ? '1' : 'n')},{TableFKtxt}({FKtxt})"
                    });
                   /* var FKtxt1 = reader[nameof(RelationShipInfoView.PK)].ToString();
                    var IsUniquetxt1 = Convert.ToBoolean(reader[nameof(RelationShipInfoView.IsUnique)]);
                    var Pktxt1 = reader[nameof(RelationShipInfoView.FK)].ToString();
                    var TableFKtxt1 = reader[nameof(RelationShipInfoView.TablePK)].ToString();
                    var TablePKtxt1 = reader[nameof(RelationShipInfoView.TableFK)].ToString();
                    listRelationInfo.Add(new RelationShipInfoView
                    {
                        FK = FKtxt1,
                        IsUnique = IsUniquetxt1,
                        PK = Pktxt1,
                        TableFK = TableFKtxt1,
                        TablePK = reader[nameof(RelationShipInfoView.TableFK)].ToString()
                    });*/
                }


            }
       
            listNameTable = (from info in listTableInfo.GroupBy(x => x.TableName)
                             join relation in listRelationInfo.GroupBy(x => x.TablePK)
                             on info.Key equals relation.Key
                             into tmpRelations
                             from tmprelation in tmpRelations.DefaultIfEmpty()
                                
                             select new TableModelView
                             {
                                 Name = info.Key,
                                 Fields = info.Select(x => x.ColumnName).ToList(),
                                 Types = info.Select(x => x.Type).ToList(),
                                 RelationShips = tmprelation?.Select(x => $"{x.TablePK}({x.PK}),1-{(x.IsUnique ? '1' : 'n')},{x.TableFK}({x.FK})").ToList(),
                                 RelationShipInfos = tmprelation?.ToList() ?? new List<RelationShipInfoView>()


                             }).ToList();

            return listNameTable;
           
        }

        private string GetSelect(IList<TableQuery> tableQueries)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in tableQueries.Where(x => x.Show ?? false))
            {
                stringBuilder.AppendFormat(",{0} ", item.GetSelectField());
            }
            return stringBuilder.Length > 0 ? stringBuilder.Remove(0, 1).Insert(0, "Select ").ToString() : "";
        }

/*        private string GetCondition(IList<TableQuery> tableQueries)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var item in tableQueries.Where(x => !string.IsNullOrEmpty(x.Criteria) || !string.IsNullOrEmpty(x.Or)))
            {
                if (!string.IsNullOrEmpty(item.Criteria))
                {
                    stringBuilder.AppendFormat("{0}{1} and ", item.GetSelectField(), item.Criteria.Replace("\"", "\'"));
                }

                if (!string.IsNullOrEmpty(item.Or))
                {
                    stringBuilder.AppendFormat("{0}{1} or  ", item.GetSelectField(), item.Or.Replace("\"", "\'"));
                }
            }

            return stringBuilder.Length > 4 ? stringBuilder.Remove(stringBuilder.Length - 4, 4).Insert(0, "where ").ToString() : "";
        }*/
        private string GetCondition(IList<TableQuery> tableQueries)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var item in tableQueries.Where(x => !string.IsNullOrEmpty(x.Criteria) || !string.IsNullOrEmpty(x.Or)))
            {
                if (!string.IsNullOrEmpty(item.Criteria))
                {
                    stringBuilder.AppendFormat("and {0}{1}  ", item.GetSelectField(), item.Criteria.Replace("\"", "\'"));
                 
                }

                if (!string.IsNullOrEmpty(item.Or))
                {
                    stringBuilder.AppendFormat(" or {0}{1}  ", item.GetSelectField(), item.Or.Replace("\"", "\'"));
              
                }
            }

            return stringBuilder.Length > 4 ? stringBuilder.Remove(0, 4).Insert(0, "where ").ToString() : "";
        }
        private string GetSort(IList<TableQuery> tableQueries)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var item in tableQueries.Where(x => string.Compare(x.Sort, "ASC", true) == 0 || string.Compare(x.Sort, "DESC", true) == 0))
            {

                stringBuilder.AppendFormat(",{0} {1} ", item.GetSelectField(), item.Sort);
            }

            return stringBuilder.Length > 0 ? stringBuilder.Remove(0, 1).Insert(0, "order by ").ToString() : "";
        }

        private string GetJoin(IList<TableQuery> tableQueries)
        {
            var first = tableQueries.First();
            var GroupTable = tableQueries.Where(x=>x.Table != first.Table).GroupBy(x => x.Table).ToList();
            if (GroupTable.Count < 1) return "";

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var group in GroupTable)
            {
                stringBuilder.AppendLine(group.First().JoinOn);
            }
            return stringBuilder.Length > 0 ? stringBuilder.ToString() : "";
        }

        public Task<string> GenerateSqlSelectAsync(IList<TableQuery> tableQueries)
        {
            if (tableQueries.Count < 1)
            {
                return Task.FromResult("");
            }
            StringBuilder stringBuilder = new StringBuilder();
            var first = tableQueries.First();
            var sqlSelect = stringBuilder
                                        .AppendFormat(" {0} from {1} {2} {3} {4};",
                                                                    GetSelect(tableQueries),
                                                                    first.Table,
                                                                    GetJoin(tableQueries),
                                                                    GetCondition(tableQueries),
                                                                    GetSort(tableQueries))
                                        .ToString();

            return Task.FromResult(sqlSelect);
        }
 
    }
   
}
