using System;

namespace JsonTableAttribute
{
    public class TableNameAttribute : Attribute
    {
        public string Name { get; set; }

        public TableNameAttribute(string tableName)
        {
            this.Name = tableName;
        }
        
    }

    public class InnerTableNameAttribute : Attribute
    {
        public string Name { get; set; }

        public InnerTableNameAttribute(string innerTableName)
        {
            this.Name = innerTableName;
        }
    }
}