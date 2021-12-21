using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicReport.Common
{
    public static class Helper
    {
        public const string QueryRelationShipString = @"SELECT
                                                            tp.name as TableFK,
                                                            cp.name as FK,
                                                            tr.name as TablePK,
                                                            cr.name as PK,
	                                                        CASE WHEN UNIQUE_KEY.COLUMN_NAME IS NULL THEN 0 ELSE 1 END AS IsUnique
                                                        FROM 
                                                            sys.foreign_keys fk
                                                        INNER JOIN 
                                                            sys.tables tp ON fk.parent_object_id = tp.object_id
                                                        INNER JOIN 
                                                            sys.tables tr ON fk.referenced_object_id = tr.object_id
                                                        INNER JOIN 
                                                            sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
                                                        INNER JOIN 
                                                            sys.columns cp ON fkc.parent_column_id = cp.column_id AND fkc.parent_object_id = cp.object_id
                                                        INNER JOIN 
                                                            sys.columns cr ON fkc.referenced_column_id = cr.column_id AND fkc.referenced_object_id = cr.object_id

                                                        LEFT JOIN (SELECT tc.TABLE_NAME,cu.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc 
			                                                        inner join INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE cu 
			                                                        on cu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME 
			                                                        where 
				                                                        tc.CONSTRAINT_TYPE = 'UNIQUE') as UNIQUE_KEY

                                                        ON cp.name = UNIQUE_KEY.COLUMN_NAME AND tp.name = UNIQUE_KEY.TABLE_NAME 

                                                        ORDER BY
                                                            tp.name, cp.column_id";
        public const string QueryTableString = @"SELECT TABLE_NAME AS TableName,COLUMNS.COLUMN_NAME as ColumnName,DATA_TYPE as Type
                                                    FROM(SELECT Name FROM SYS.TABLES WHERE IS_MS_SHIPPED= 0 AND NOT NAME LIKE 'SYS%') AS TABLENAME
                                                    INNER JOIN(SELECT COLUMN_NAME, TABLE_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS) AS COLUMNS
                                                    ON TABLENAME.name = COLUMNS.TABLE_NAME";
    }
}
