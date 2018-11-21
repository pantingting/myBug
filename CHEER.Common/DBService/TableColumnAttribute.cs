using System;

namespace CHEER.Common.DBService
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class TableAttribute : Attribute
    {
        public string TableName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class ColumnAttribute : Attribute
    {
        public string ColumnName { get; set; } = null;
        public bool IsPrimaryKey { get; set; } = false;
        public bool CanInsert { get; set; } = true;
        public bool CanUpdate { get; set; } = true;
    }
}
