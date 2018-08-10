
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Data.SQLite;

namespace BreakFastWPF.Models
{
    public class SQLiteHelper
    {
        private SQLiteConnection sqlConn;
        //private static string sqliteFile = @"C:\Users\IEUser\AppData\Local\database.db";
        private static string sqliteFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/database.db";  // set folder for database
        private static string sqlitePw = "1234567"; // set password for database

        //Constructor
        public SQLiteHelper()
        {
            // check if database file exist when not create with password
            if (!File.Exists(sqliteFile))
            {
                sqlConn = new SQLiteConnection("Data Source=" + sqliteFile + ";Version=3;Page Size=1024;");
                sqlConn.SetPassword(sqlitePw);
            }
            sqlConn = new SQLiteConnection("Data Source=" + sqliteFile + ";Version=3;Page Size=1024;Password=" + sqlitePw); // connect to database

            // create tables, when not exist
            string query =
                "CREATE TABLE IF NOT EXISTS user (id INTEGER PRIMARY KEY AUTOINCREMENT, 'login' TEXT, 'firstname' TEXT, 'lastname' TEXT, 'persid' TEXT, 'password' Text, 'role' INTEGER, 'first' INTEGER, 'active' INTEGER);" +
                "CREATE TABLE IF NOT EXISTS userRigths (id INTEGER PRIMARY KEY, 'lwe' INTEGER, 'lwa' INTEGER, 'lwb' INTEGER, 'lwi' INTEGER, 'mwe' INTEGER, 'mwa' INTEGER, 'mwb' INTEGER, 'mwi' INTEGER, 'vacc' INTEGER, 'vadr' INTEGER);" +
                "CREATE TABLE IF NOT EXISTS menu (MenuId INTEGER PRIMARY KEY, 'MenuType' INTEGER, 'Style' INTEGER, 'Title' TEXT, 'Price' INTEGER, 'Discount' INTEGER, 'ImageUri' TEXT, 'mwb' INTEGER, 'Desp' TEXT);" +
                "CREATE TABLE IF NOT EXISTS admin ('password' Text);" +
                "INSERT INTO user('login','firstname','lastname') VALUES('chennet','Patrick','Shih')";
            queryNon(query);
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + sqliteFile + ";Version=3;Page Size=1024;Password=" + sqlitePw))
            {
                conn.Open();
                SQLiteCommand command = new SQLiteCommand("Select * from user", conn);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                    Console.WriteLine(reader["id"]);

                reader.Close();
            }
        }

        // check if admin password exist
        public bool CheckAdmin()
        {
            sqlConn.Open();
            var command = sqlConn.CreateCommand();
            command.CommandText = "SELECT * FROM admin";
            SQLiteDataReader reader = command.ExecuteReader();
            bool rows = reader.HasRows;
            sqlConn.Close();
            return rows;
        }

        // methode for querys without response
        private void queryNon(string query)
        {
            sqlConn.Open();
            var command = sqlConn.CreateCommand();
            command.CommandText = query;
            command.ExecuteNonQuery();
            sqlConn.Close();
        }

    }
}
