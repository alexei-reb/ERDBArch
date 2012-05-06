using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Data.EntityClient;
using System.Data.Common;
using System.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace ERDBArch.Test.PhoneBook
{
    [TestFixture]
    class Class1
    {
        [Test]
        public void Test1()
        {

            string MyConString = "SERVER=jenkins.stvad.org;" +
                "DATABASE=lexaDB;" +
                "UID=lexa.re;" +
                "PASSWORD=21nbvcxz;";
            MySqlConnection connection = new MySqlConnection(MyConString);
            MySqlCommand command = connection.CreateCommand();
            MySqlDataReader Reader;
            command.CommandText = "select * from Person";
            connection.Open();
            Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                string thisrow = "";
                for (int i = 0; i < Reader.FieldCount; i++)
                    thisrow += Reader.GetValue(i).ToString() + ",";
                Debug.WriteLine(thisrow);
            }
            connection.Close();
        }
    }
}
