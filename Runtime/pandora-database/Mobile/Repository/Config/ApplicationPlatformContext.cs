using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Unity.Pandora.Database.Mobile.Repository.Helper;
using UnityEngine;

namespace Unity.Pandora.Database.Mobile.Repository.Config
{
    public class ApplicationPlatformContext<TObject> where TObject : IEntity
    {
        private SqliteConnection connection;

        private string url;

        /// <summary>
        /// 
        /// Description: Inizializate the sqlLite context database fom desktop and mobile device
        /// 
        /// </summary>
        internal void Initialized()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                url = "URI = file:" + Application.persistentDataPath + "/" + AppDatabase.Instance.DbName + ";Version=3;Pooling=True;Max Pool Size=100;";

            }
            else
            {
                url = "URI = file:" + Application.persistentDataPath + "/" + AppDatabase.Instance.DbName + ";Version=3;Pooling=True;Max Pool Size=100;";
            }
        }

        public void ExecuteInsert(string sqlInsert)
        {
            LoggerHelper.LogConsole(sqlInsert);
            using (connection = new SqliteConnection(url))
            {
                connection.Open();
                IDbCommand dbcmd = this.connection.CreateCommand();
                dbcmd.CommandText = sqlInsert;
                IDataReader reader = dbcmd.ExecuteReader();
                connection.Close();
            }
        }

        [Obsolete("ExecuteSelect is deprecated, please contruct entity and use method ExecuteSelectEntity.")]
        public string ExecuteSelect(string sqlSelect, string[] columns)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (connection = new SqliteConnection(url))
            {
                connection.Open();
                IDataReader reader = null;
                IDbCommand dbcmd = this.connection.CreateCommand();
                dbcmd.CommandText = sqlSelect;
                reader = dbcmd.ExecuteReader();
                while (reader.Read())
                {
                    foreach (string column in columns)
                    {
                        var value = SqliteHelper.GetString(reader, column);
                        stringBuilder.Append(column + " : " + value + "\n");
                    }

                }
                connection.Close();
            }
            return stringBuilder.ToString();
        }

        public List<string> ExecuteSelectAllEntity(string select)
        {
            List<string> listEntity = new List<string>();
            using (connection = new SqliteConnection(url))
            {
                connection.Open();
                IDataReader reader = null;
                IDbCommand dbcmd = this.connection.CreateCommand();
                dbcmd.CommandText = select;
                reader = dbcmd.ExecuteReader();
                Dictionary<string, object> dictionary;
                while (reader.Read())
                {
                    dictionary = new Dictionary<string, object>();
                    for (int index = 0; index < reader.FieldCount; index++)
                    {

                        var columnName = reader.GetName(index);
                        var sqlType = reader.GetDataTypeName(index);
                        if (SqLiteDataType.TEXT.ToString().Equals(sqlType))
                        {

                            var value = SqliteHelper.GetString(reader, columnName);
                            dictionary[columnName] = value;
                        }

                        if (SqLiteDataType.INTEGER.ToString().Equals(sqlType))
                        {
                            int value = reader.GetInt32(index);
                            dictionary[columnName] = value;
                        }

                        if (SqLiteDataType.REAL.ToString().Equals(sqlType))
                        {
                            var value = reader.GetFloat(index);
                            dictionary[columnName] = value;
                        }
                    }
                    listEntity.Add(JsonHelper.ParserDictionary(dictionary));

                }
                connection.Close();
            }
            return listEntity;
        }

        public Dictionary<string, object> ExecuteSelectEntity(string sqlSelect, Dictionary<string, object> objectData)
        {
            using (connection = new SqliteConnection(url))
            {
                connection.Open();
                IDataReader reader = null;
                IDbCommand dbcmd = connection.CreateCommand();
                dbcmd.CommandText = sqlSelect;

                try
                {
                    reader = dbcmd.ExecuteReader();

                    var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                    while (reader.Read())
                    {

                        for (int index = 0; index < reader.FieldCount; index++)
                        {

                            var columnName = reader.GetName(index);
                            var sqlType = reader.GetDataTypeName(index);
                            if (SqLiteDataType.TEXT.ToString().Equals(sqlType))
                            {
                                var value = reader.GetString(index);
                                objectData[columnName] = value;
                            }

                            if (SqLiteDataType.INTEGER.ToString().Equals(sqlType))
                            {
                                int value = reader.GetInt32(index);
                                objectData[columnName] = value;
                            }

                            if (SqLiteDataType.REAL.ToString().Equals(sqlType))
                            {
                                var value = reader.GetFloat(index);
                                objectData[columnName] = value;
                            }
                        }

                    }
                }
                catch (Exception exeption)
                {
                    LoggerHelper.LogConsole("error SqlLite");
                    LoggerHelper.LogConsole("Specific: " + exeption.Message);
                }
                connection.Close();

            }
            return objectData;
        }

        public void CreationTable(string sqlStatement)
        {
            LoggerHelper.LogConsole(sqlStatement);
            using (connection = new SqliteConnection(url))
            {
                connection.Open();
                SqliteCommand command = this.connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sqlStatement;
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }

}
 
