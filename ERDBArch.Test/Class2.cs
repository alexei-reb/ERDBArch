using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Data.EntityClient;
using System.Data.Common;
using System.Data;

namespace ERDBArch.Test.PhoneBook
{
    [TestFixture]
    class Class2
    {
        [Test]
        public void Test1()
        {
            string SELECT_PERSONS_ALL = "SELECT p.id as person_id, p.fname, p.lname, ph.id as phone_id , " +
                "ph.phonevalue as phonevalue, a.id as address_id , a.addressvalue as addressvalue " +
                "FROM Entities.People as p INNER JOIN Entities.Addresses as a ON a.personid = p.id " +
                "INNER JOIN Entities.Phones as ph ON ph.personid = p.id";
            
            EntityConnection m_connection = new EntityConnection("name=Entities");
            List<Mock.PhoneBook.Phone> list = new List<Mock.PhoneBook.Phone>();

            m_connection.Open();
            Infrastructure.PhoneBook.IMockConvertor convertor = new ERDBArch.Modules.PhoneBook.BLL.PersonConvertor();

            using (EntityCommand cmd = new EntityCommand(SELECT_PERSONS_ALL, m_connection))
            {
                using (DbDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                {
                    while (reader.Read())
                    {
                        System.Diagnostics.Debug.WriteLine(reader["person_id"]);
   
                    }
                }
            }
            m_connection.Close();
            
        }
    }
}
