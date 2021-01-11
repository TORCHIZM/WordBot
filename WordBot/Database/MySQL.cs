using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Torchizm_Bot.Database
{
    public static class MySQL
    {
        private const string ConnectionString = @"server=localhost;userid=root;database=wordbot";

        public static List<T> Select<T>(string query)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            using var cmd = new MySqlCommand(query, connection);
            using var dataReader = cmd.ExecuteReader();

            var list = DataReaderMapToList<T>(dataReader);

            connection.Close();

            return list;
        }

        public static void Execute(string query)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            using var cmd = new MySqlCommand(query, connection);
            using var dataReader = cmd.ExecuteReader();

            connection.Close();
        }

        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();

                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                        prop.SetValue(obj, dr[prop.Name], null);

                list.Add(obj);
            }
            return list;
        }
    }
}