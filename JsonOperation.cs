using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JsonTableAttribute;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NestedReflection {
    public class JsonOperation {
        private readonly string Tables = "Tables";
        private readonly string innerTables = "innerTables";

        public T GetDataFromJson<T> (string jsonContent) where T : new () {
            JObject json = new JObject ();
            JObject jsonData = JObject.Parse (jsonContent);

            foreach (PropertyInfo property in typeof (T).GetProperties ()) {
                Type listType = property.PropertyType.GetGenericArguments ()?.ToList ().FirstOrDefault ();

                if (listType != null) {
                    string nameOfTable = GetTableName (property);
                    if (nameOfTable != null) {
                        JArray tableArray = GetTableJson (listType, jsonData.GetValue (nameof (Tables)) as JArray, nameOfTable);
                        json.Add (new JProperty (property.Name, tableArray));

                    } else {
                        continue;
                    }
                } else {
                    json.Add (new JProperty (property.Name, jsonData.GetValue (property.Name)));
                }
            }
            return json.ToObject<T> ();
        }

        public JArray GetTableJson (Type table, JArray array, string tableName) {
            List<JsonTable> tables = array.ToObject<List<JsonTable>> ();
            JsonTable desireTable = tables.FirstOrDefault (t => t.tableName.ToLower ().Equals (tableName.ToLower ()));

            if (desireTable != null) {
                Dictionary<string, string> innerTableMap = new Dictionary<string, string> ();
                foreach (PropertyInfo property in table.GetProperties ()) {
                    string innerTableName = GetInnerTableName (property);
                    if (innerTableName != null) {
                        innerTableMap.Add (property.Name, innerTableName);
                    } else {
                        continue;
                    }
                }

                if (innerTableMap.Count () > 0) {

                    JArray tableArray = new JArray ();
                    foreach (JObject json in JArray.Parse (JsonConvert.SerializeObject (desireTable.values))) {
                        List<JsonTable> innertableList = JArray.Parse (json.GetValue (nameof (innerTables)).ToString ()).ToObject<List<JsonTable>> ();
                        foreach (var key in innerTableMap.Keys) {
                            json[key] = GetInnerTable (innertableList, innerTableMap.GetValueOrDefault (key));
                        }

                        tableArray.Add (json);
                    }

                    return tableArray;
                } else {
                    var values = JsonConvert.SerializeObject (desireTable.values);
                    return JArray.Parse (values);
                }

            } else {
                return new JArray ();
            }
        }

        private JArray GetInnerTable (List<JsonTable> innerTables, string innerTableName) {
            JsonTable table = innerTables.FirstOrDefault (t => t.tableName.ToLower ().Equals (innerTableName.ToLower ()));

            if (table != null) {
                return JArray.Parse (JsonConvert.SerializeObject (table.values));
            }

            return new JArray ();
        }

        private string GetTableName (PropertyInfo type) {
            Attribute[] attributes = Attribute.GetCustomAttributes (type);

            if (attributes.Count () > 0) {
                foreach (Attribute attribute in attributes) {
                    if (attribute is TableNameAttribute) {
                        TableNameAttribute table = (TableNameAttribute) attribute;
                        return table.Name;
                    }
                }
                return null;
            } else {
                return null;
            }
        }

        private string GetInnerTableName (PropertyInfo type) {
            Attribute[] attributes = Attribute.GetCustomAttributes (type);

            if (attributes.Count () > 0) {
                foreach (Attribute attribute in attributes) {
                    if (attribute is InnerTableNameAttribute) {
                        InnerTableNameAttribute table = (InnerTableNameAttribute) attribute;
                        return table.Name;
                    }
                }
                return null;
            } else {
                return null;
            }
        }
    }
}