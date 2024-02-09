using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EquipmentCalculating.Data
{
    public static class Repository
    {
        // TODO !!! Дописать

        private static string _dbPath = Path.Combine(AppContext.BaseDirectory, "catalog.db");

        private static bool DatabaseIsExist => File.Exists(_dbPath);

        private static readonly string _connectionString = $"Data Source={_dbPath}";

        public static object Get()
        {
            if (DatabaseIsExist is false)
            {
                InitializeDatabase();
            }

            throw new NotImplementedException();
        }

        public static void Set()
        {

        }

        private static void InitializeDatabase()
        {
            File.Create(_dbPath).Close();

            using (var dbConnection = new SqliteConnection(_connectionString))
            {
                dbConnection.Open();

                SqliteCommand command = dbConnection.CreateCommand();
                command.CommandText =
                    @"CREATE TABLE Equipments (
                      Id                INTEGER PRIMARY KEY AUTOINCREMENT,
                      SerialNumber              TEXT,
                      Name              TEXT,
                      Category             TEXT,
                      GoodCondition          TEXT,
                      Caliber           TEXT,
                      Location         TEXT,
                  );";

                _ = command.ExecuteNonQuery();
            }



        }


    }
}
