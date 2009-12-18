﻿using System;
using System.Data;

namespace Schema.Metadata
{
    public class DbRelation : DbObject
    {
        public DbRelation(IDbSchemaProvider provider) : base(provider) { }

        public DbRelation(IDbSchemaProvider provider, DataRow RelationRow) : this(provider)
        {
            if (RelationRow["PK_TABLE_CATALOG"] != DBNull.Value)
                this.PkTableCatalog = RelationRow["PK_TABLE_CATALOG"].ToString();
            if (RelationRow["PK_TABLE_SCHEMA"] != DBNull.Value)
                this.PkTableSchema = RelationRow["PK_TABLE_SCHEMA"].ToString();
            this.PkTableName = RelationRow["PK_TABLE_NAME"].ToString();
            this.PkColumnName = RelationRow["PK_COLUMN_NAME"].ToString();

            if (RelationRow["FK_TABLE_CATALOG"] != DBNull.Value)
                this.FkTableCatalog = RelationRow["FK_TABLE_CATALOG"].ToString();
            if (RelationRow["FK_TABLE_SCHEMA"] != DBNull.Value)
                this.FkTableSchema = RelationRow["FK_TABLE_SCHEMA"].ToString();
            this.FkTableName = RelationRow["FK_TABLE_NAME"].ToString();
            this.FkColumnName = RelationRow["FK_COLUMN_NAME"].ToString();
            this.FkName = RelationRow.Field<string>("FK_NAME");
        }

        public string PkTableCatalog { get; set; }
        public string PkTableSchema { get; set; }
        public string PkTableName { get; set; }
        public string PkColumnName { get; set; }
        
        public string FkTableCatalog { get; set; }
        public string FkTableSchema { get; set; }
        public string FkTableName { get; set; }
        public string FkColumnName { get; set; }

        public string FkName { get; set; }
    }
}
