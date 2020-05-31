using System.Data;
using System;
using System.Text;
using System.Collections.Generic;
using Unity.Pandora.Database.Mobile.Repository.Config;
using Unity.Pandora.Database.Mobile.Repository.Enum;

namespace Unity.Pandora.Database.Mobile.Repository.Helper
{
    public static class SqliteHelper
    {
        public static string GetString(this IDataReader reader, string name)
        {
            return GetFieldValue<String>(reader, name, (string)null);
        }

        public static T GetFieldValue<T>(this IDataReader reader, string fieldName, T defaultvalue = default(T))
        {
            try
            {
                var value = reader[fieldName];
                if (value == DBNull.Value || value == null)
                    return defaultvalue;
                return (T)value;
            }
            catch (Exception e)
            {
                LoggerHelper.LogConsole("Error reading databasefield " + fieldName + "| " + e.Message);
                //SimpleLog.Error("Error reading databasefield " + fieldName + "| ", e);
            }
            return defaultvalue;
        }

        public static string CreatedTableFromEntity(IEntity entity)
        {

            string tablename = entity.GetTableName();

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("CREATE TABLE IF NOT EXISTS '" + tablename + "' ");

            stringBuilder.Append(" ('Id' INTEGER PRIMARY KEY ");


            foreach (KeyValuePair<string, object> entry in entity.GetDictionary())
            {
                if (!"Id".Equals(entry.Key))
                {

                    stringBuilder.Append(" , ");

                    if (typeof(string).IsInstanceOfType(entry.Value))
                    {
                        stringBuilder.Append(" '" + entry.Key + "' " + SqLiteDataType.TEXT.ToString() + " NOT NULL");
                    }

                    if (typeof(float).IsInstanceOfType(entry.Value))
                    {
                        stringBuilder.Append(" '" + entry.Key + "' " + SqLiteDataType.REAL.ToString() + " NOT NULL");
                    }

                    if (typeof(int).IsInstanceOfType(entry.Value))
                    {
                        stringBuilder.Append(" '" + entry.Key + "' " + SqLiteDataType.INTEGER.ToString() + " NOT NULL");
                    }
                }
            }
            stringBuilder.Append(" );");

            return stringBuilder.ToString();
        }

        public static string DeleteEntity(int id, IEntity entity)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("delete from " + entity.GetTableName());

            builder.Append(" where id = " + id);

            return builder.ToString();
        }

        public static string SelectAllEntity(string tableName, Dictionary<string, object> dictionary)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("select ");

            foreach (KeyValuePair<string, object> entry in dictionary)
            {
                stringBuilder.Append(entry.Key + ",");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            stringBuilder.Append(" FROM " + tableName);

            return stringBuilder.ToString();
        }

        public static string SelectWithCondiction(IEntity entity, Dictionary<ConditionKey, string> dictionary)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("select ");

            foreach (KeyValuePair<string, object> entry in entity.GetDictionary())
            {
                stringBuilder.Append(entry.Key + ",");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            stringBuilder.Append(" FROM " + entity.GetTableName());

            //find Where 

            foreach (KeyValuePair<ConditionKey, string> entry in dictionary)
            {
                if (entry.Key.Equals(ConditionKey.WHERE))
                {
                    stringBuilder.Append(entry.Value);
                }
            }

            //find OrderBy
            foreach (KeyValuePair<ConditionKey, string> entry in dictionary)
            {
                if (entry.Key.Equals(ConditionKey.OREDER_BY))
                {
                    stringBuilder.Append(entry.Value);
                }
            }

            //find Limit
            foreach (KeyValuePair<ConditionKey, string> entry in dictionary)
            {

                if (entry.Key.Equals(ConditionKey.LIMIT))
                {
                    stringBuilder.Append(entry.Value);
                }
            }
            stringBuilder.Append(";");
            return stringBuilder.ToString();
        }

        public static string UpdateEntity(IEntity entity)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("UPDATE " + entity.GetTableName() + " ");
            stringBuilder.Append("SET ");

            Dictionary<string, object> objectData = entity.GetDictionary();

            foreach (KeyValuePair<string, object> entry in objectData)
            {
                if (!"Id".Equals(entry.Key))
                {
                    if (typeof(string).IsInstanceOfType(entry.Value))
                    {
                        stringBuilder.Append(entry.Key + " = '" + (string)entry.Value + "' ,");
                    }

                    if (typeof(float).IsInstanceOfType(entry.Value) || typeof(int).IsInstanceOfType(entry.Value))
                    {
                        stringBuilder.Append(entry.Key + " =" + entry.Value + " ,");
                    }
                }
            }

            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            foreach (KeyValuePair<string, object> entry in objectData)
            {
                if ("Id".Equals(entry.Key))
                {
                    stringBuilder.Append(" WHERE ID =" + entry.Value + ";");
                }

            }

            return stringBuilder.ToString();
        }

        public static string SelectEntity(string tableName, Dictionary<string, object> objectData, int id)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("select ");

            foreach (KeyValuePair<string, object> entry in objectData)
            {
                stringBuilder.Append(entry.Key + ",");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            stringBuilder.Append(" FROM " + tableName);

            stringBuilder.Append(" WHERE ID =" + id + ";");

            return stringBuilder.ToString();
        }

        public static string InsertEntity(IEntity entity)
        {
            string tablename = entity.GetTableName();

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Insert into " + tablename + " ( ");

            foreach (KeyValuePair<string, object> entry in entity.GetDictionary())
            {
                if (!"Id".Equals(entry.Key))
                {
                    stringBuilder.Append(entry.Key + ",");
                }
            }

            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(") VALUES ( ");

            foreach (KeyValuePair<string, object> entry in entity.GetDictionary())
            {
                if (!"Id".Equals(entry.Key))
                {
                    if (typeof(string).IsInstanceOfType(entry.Value))
                    {
                        stringBuilder.Append(" '" + (string)entry.Value + "',");
                    }

                    if (typeof(float).IsInstanceOfType(entry.Value) || typeof(int).IsInstanceOfType(entry.Value))
                    {
                        stringBuilder.Append(entry.Value + ",");
                    }
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            stringBuilder.Append(" );");

            return stringBuilder.ToString();
        }

        public static string InsertEntityById(IEntity entity)
        {
            string tablename = entity.GetTableName();

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Insert into " + tablename + " ( ");

            foreach (KeyValuePair<string, object> entry in entity.GetDictionary())
            {
                stringBuilder.Append(entry.Key + ",");
            }

            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(") VALUES ( ");

            foreach (KeyValuePair<string, object> entry in entity.GetDictionary())
            {

                if (typeof(string).IsInstanceOfType(entry.Value))
                {
                    stringBuilder.Append(" '" + (string)entry.Value + "',");
                }

                if (typeof(float).IsInstanceOfType(entry.Value) || typeof(int).IsInstanceOfType(entry.Value))
                {
                    stringBuilder.Append(entry.Value + ",");
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            stringBuilder.Append(" );");

            return stringBuilder.ToString();
        }

    }
}