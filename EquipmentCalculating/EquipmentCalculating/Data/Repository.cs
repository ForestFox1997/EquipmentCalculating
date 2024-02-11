using EquipmentCalculating.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace EquipmentCalculating.Data
{
    public class Repository
    {
        private string _dbPath = Path.Combine(AppContext.BaseDirectory, "catalog.db");

        private bool DatabaseIsExist => File.Exists(_dbPath);

        private readonly string _connectionString;

        public Repository()
        {
            _connectionString = $"Data Source={_dbPath}";

            if (!DatabaseIsExist)
            {
                InitializeDatabase();
            }
        }

        public IList<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();

            using (var dbConnection = new SQLiteConnection(_connectionString))
            {
                dbConnection.Open();

                SQLiteCommand command = dbConnection.CreateCommand();
                command.CommandText = "SELECT * FROM Categories";

                SQLiteDataReader reader = command.ExecuteReader();
                for (int itemsReceived = 0; reader.Read(); itemsReceived++)
                {
                    categories.Add(new Category
                    {
                        Id = Convert.ToInt64(reader["Id"]),
                        Name = reader.IsDBNull(1)
                            ? default : (string)reader["Name"],
                    });

                    itemsReceived++;
                }
            }

            return categories;
        }

        public Category AddCategory(Category category)
        {
            using (var dbConnection = new SQLiteConnection(_connectionString))
            {
                dbConnection.Open();

                SQLiteCommand command = dbConnection.CreateCommand();
                command.CommandText = $"INSERT INTO Categories (Name) VALUES ('{category.Name}') RETURNING Id";

                var id = command.ExecuteScalar();

                category.Id = Convert.ToInt32(id);
            }

            return category;
        }

        public void RemoveCategory(Category category)
        {
            using (var dbConnection = new SQLiteConnection(_connectionString))
            {
                dbConnection.Open();

                SQLiteCommand command = dbConnection.CreateCommand();
                command.CommandText = $"DELETE FROM Categories WHERE Id = {category.Id}";

                _ = command.ExecuteNonQuery();
            }
        }

        public IList<Equipment> GetEquipments()
        {
            List<Equipment> equpments = new List<Equipment>();

            using (var dbConnection = new SQLiteConnection(_connectionString))
            {
                dbConnection.Open();

                SQLiteCommand command = dbConnection.CreateCommand();

                List<Location> locations = new List<Location>();

                command.CommandText = "SELECT * FROM Locations";

                SQLiteDataReader reader = command.ExecuteReader();
                for (int itemsReceived = 0; reader.Read(); itemsReceived++)
                {
                    locations.Add(new Location
                    {
                        Id = Convert.ToInt64(reader["Id"]),
                        IsWarehouse = reader.IsDBNull(1) ? default : Convert.ToBoolean((long)reader[1]),
                        Number = reader.IsDBNull(2) ? default : (string)reader[2],
                        Name = reader.IsDBNull(3) ? default : (string)reader[3],
                        IsLegalEntity = reader.IsDBNull(4) ? default : Convert.ToBoolean((long)reader[4])
                    });
                }

                command.Reset();
                command.CommandText = "SELECT * FROM equipment";

                reader = command.ExecuteReader();
                for (int itemsReceived = 0; reader.Read(); itemsReceived++)
                {
                    var equipment = new Equipment();
                    equipment.Id = Convert.ToInt64(reader["Id"]);
                    equipment.SerialNumber = reader.IsDBNull(1)
                            ? default : (string)reader[1];
                    equipment.Name = reader.IsDBNull(2)
                            ? default : (string)reader[2];
                    equipment.Category = new Category
                    {
                        Id = reader.IsDBNull(3)
                            ? default : (long)reader[3]
                    };
                    equipment.GoodCondition = reader.IsDBNull(4)
                            ? default : Convert.ToBoolean((long)reader[4]);
                    equipment.Location = locations.Find(l => l.Id == equipment.Id);
                    equpments.Add(equipment);

                    itemsReceived++;
                }
            }

            return equpments;
        }

        public void SaveEquipment(Equipment equipment)
        {
            if (equipment == null)
            {
                return;
            }

            using (var dbConnection = new SQLiteConnection(_connectionString))
            {
                dbConnection.Open();

                SQLiteCommand command = dbConnection.CreateCommand();

                if (equipment.Location == null)
                {
                    equipment.Location = new Location();
                    command.CommandText = $@"
    INSERT INTO Locations (IsWarehouse, Number, Name, IsLegalEntity) VALUES 
    ({Convert.ToInt64(equipment.Location.IsWarehouse)},
    '{equipment.Location.Number}',
    '{equipment.Location.Name}',
    {Convert.ToInt64(equipment.Location.IsLegalEntity)}) RETURNING Id";
                    equipment.Location.Id = (long)command.ExecuteScalar();
                }
                else
                {
                    command.CommandText = $@"
    UPDATE Locations SET 
        IsWarehouse = {Convert.ToInt64(equipment.Location.IsWarehouse)},
        Number = '{equipment.Location.Number}',
        Name = '{equipment.Location.Name}',
        IsLegalEntity = {Convert.ToInt64(equipment.Location.IsLegalEntity)}
    WHERE Id = {equipment.Location.Id}";
                    _ = command.ExecuteNonQuery();
                }

                if (equipment.Id == null)
                {
                    var category = equipment.Category == null ? "NULL" : equipment.Category.Id.ToString();
                    command.CommandText = $@"
    INSERT INTO Equipment (SerialNumber, Name, Category, GoodCondition, Location) VALUES (
        '{equipment.SerialNumber}',
        '{equipment.Name}',
        {category},
        {Convert.ToInt64(equipment.GoodCondition)},
        {equipment.Location.Id}) RETURNING Id";
                    equipment.Id = (long)command.ExecuteScalar();
                }
                else
                {
                    var category = equipment.Category == null ? "NULL" : equipment.Category.Id.ToString();
                    command.CommandText = $@"
    UPDATE Equipment SET SerialNumber = '{equipment.SerialNumber}',
        Name = '{equipment.Name}',
        Category = {category},
        GoodCondition = '{Convert.ToInt64(equipment.GoodCondition)}',
        Location = {equipment.Location.Id}
    WHERE Id = {equipment.Id}";
                    _ = command.ExecuteNonQuery();
                }

            }
        }

        public void RemoveEquipment(Equipment equipment)
        {
            using (var dbConnection = new SQLiteConnection(_connectionString))
            {
                dbConnection.Open();

                SQLiteCommand command = dbConnection.CreateCommand();
                command.CommandText = $"DELETE FROM Equipment WHERE Id = {equipment.Id}";

                _ = command.ExecuteNonQuery();
            }
        }

        private void InitializeDatabase()
        {
            File.Create(_dbPath).Close();

            using (var dbConnection = new SQLiteConnection(_connectionString))
            {
                dbConnection.Open();

                SQLiteCommand command = dbConnection.CreateCommand();
                command.CommandText = @"
    CREATE TABLE Categories (
        Id   INTEGER PRIMARY KEY AUTOINCREMENT,
        Name TEXT
    )
    STRICT;

    CREATE TABLE Equipment (
        Id            INTEGER PRIMARY KEY,
        SerialNumber  TEXT,
        Name          TEXT,
        Category      ANY  REFERENCES Categories (Id),
        GoodCondition INTEGER,
        Location      INTEGER 
    )
    STRICT;

    CREATE TABLE Locations (
        Id            INTEGER PRIMARY KEY AUTOINCREMENT,
        IsWarehouse   INTEGER,
        Number        TEXT,
        Name          TEXT,
        IsLegalEntity INTEGER
    );
";

                _ = command.ExecuteNonQuery();
            }
        }
    }
}
