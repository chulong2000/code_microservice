using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
namespace GHM.Infrastructure.Helpers
{
    public static class ClassHelper
    {
        public static string NameSpace<T>() where T : class
        {
            return typeof(T).Namespace;
        }

        public static string ClassName<T>() where T : class
        {
            return typeof(T).Name;
        }

        public static List<string> GetPropertiesName<T>() where T : class
        {
            return typeof(T).GetProperties().Select(x => x.Name).ToList();
        }

        public static List<string> GetPropertiesNameAsKey<T>() where T : class
        {
            var nameSpace = NameSpace<T>();
            var className = ClassName<T>();
            return typeof(T).GetProperties().Select(x => $"{nameSpace}.{className}.{x.Name}").ToList();
        }

        public static string GetPropertyNameAsKey<T>(string propertyName) where T : class
        {
            var nameSpace = NameSpace<T>();
            var className = ClassName<T>();
            return $"{nameSpace}.{className}.{propertyName}";
        }
        public static string GetPropertyGroupIdAsKey<T>() where T : class
        {
            var nameSpace = NameSpace<T>();
            var className = ClassName<T>();
            return $"{nameSpace}.{className}.{className}";
        }
        public static string GetDisplayName<T>(string propertyName)
        {
            var properties = typeof(T).GetProperties();
            var propertyInfo = properties.FirstOrDefault(x => x.Name == propertyName);
            if (propertyInfo == null) return null;

            var attributes = propertyInfo.GetCustomAttributes(true);
            if (attributes.FirstOrDefault() is DisplayNameAttribute displayNameAttribute) return displayNameAttribute.DisplayName;
            return null;
        }

        public static DataTable ToDataTable(IEnumerable<dynamic> items)
        {
            if (items == null) return null;
            var data = items.ToArray();
            if (data.Length == 0) return null;

            var dt = new DataTable();
            foreach (var pair in ((IDictionary<string, object>)data[0]))
            {
                dt.Columns.Add(pair.Key, (pair.Value ?? string.Empty).GetType());
            }
            foreach (var d in data)
            {
                dt.Rows.Add(((IDictionary<string, object>)d).Values.ToArray());
            }
            return dt;
        }

        //Convert List<T> sang một DataTable
        public static DataTable ConvertListOjbectToDataTable<T>(List<T> objectClass, string table_name = "Table")
        {
            DataTable dt = new DataTable();
            try
            {
                dt.TableName = table_name;

                foreach (PropertyInfo property in objectClass[0].GetType().GetProperties())
                {
                    dt.Columns.Add(new DataColumn(property.Name, property.PropertyType));
                }

                foreach (var vehicle in objectClass)
                {
                    DataRow newRow = dt.NewRow();
                    foreach (PropertyInfo property in vehicle.GetType().GetProperties())
                    {
                        newRow[property.Name] = vehicle.GetType().GetProperty(property.Name).GetValue(vehicle, null);
                    }
                    dt.Rows.Add(newRow);
                }
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        //Convert DataTable to List<T>
        public static List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();
            var properties = typeof(T).GetProperties();
            return dt.Rows.OfType<DataRow>().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                    {
                        PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                        pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], pI.PropertyType));
                    }
                }
                return objT;
            }).ToList();
        }

        //Convert một DataRow => Object T
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

    }
}
