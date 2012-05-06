using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace ERDBArch.Modules.PhoneBook.BLL
{
    public class DALEntitySQL
    {
        class DAL
        {

            private const string SELECT_PERSONS_ALL = 
                "SELECT p.id as person_id, p.lname, p.fname FROM Entities.People as p";
            
            private const string SELECT_PHONES_ALL = 
                "SELECT p.id as phone_id, p.personid, p.phonevalue FROM Entities.Phones as p";
            
            private const string SELECT_ADDRESSES_ALL =
                "SELECT p.id as address_id, p.personid, p.addressvalue FROM Entities.Addresses as p";

            private const string SELECT_PERSONS_ALL_JOIN = 
                "SELECT p.id as person_id, p.fname, p.lname, ph.id as phone_id , " + 
                "ph.phonevalue as phonevalue, a.id as address_id , a.addressvalue as addressvalue " +
                "FROM Entities.People as p INNER JOIN Entities.Addresses as a ON a.personid = p.id " +
                "INNER JOIN Entities.Phones as ph ON ph.personid = p.id";



            private const string INSERT_PERSON = 
                "INSERT INTO Person (lname, fname) values (@lname, @fname)";
            
            private const string INSERT_PHONE = 
                "INSERT INTO Phone (personid, phonevalue) values (@personid, @phonevalue) ";
            
            private const string INSERT_ADDRESS = 
                "INSERT INTO Address (personid, addressvalue) values (@personid, @addressvalue) ";



            private const string UPDATE_PERSON = 
                "UPDATE Person SET lname=@lname, fname=@fname";

            private const string UPDATE_PHONE = 
                "UPDATE Phone SET personid=@personid, phonevalue=@phonevalue";

            private const string UPDATE_ADDRESS = 
                "UPDATE Address SET personid=@personid, addressvalue=@addressvalue";


            private const string DELETE_PERSON = "DELETE Person WHERE id=@id";
            private const string DELETE_PHONE = "DELETE Phone WHERE id=@id";
            private const string DELETE_ADDRESS = "DELETE Address WHERE id=@id";


            /// <summary>
            /// Connects to SQL server
            /// </summary>
            /// <param name="connectionString">Connection string</param>
            public void Connect(string connectionString)
            {
                m_connection = new EntityConnection("name=Entities");
                m_sqlconnection = new SqlConnection(connectionString);
            }

            /// <summary>
            /// Gets all Persons form DB
            /// </summary>
            /// <returns>List of Person</returns>
            public List<Model.Person> GetPersons()
            {
                List<Model.Person> res = new List<Model.Person>();
                List<Mock.PhoneBook.Person> people = GetPeople();
                List<Mock.PhoneBook.Phone> phones = GetPhones();
                List<Mock.PhoneBook.Address> addresses = GetAddresses();

                foreach (var item in people)
                {
                    Mock.PhoneBook.Phone p = phones.Find(x => x.PersonID == item.ID);
                    if (p == null)
                        p = Mock.PhoneBook.Phone.Null;

                    Mock.PhoneBook.Address a = addresses.Find(x => x.PersonID == item.ID);
                    if (a == null)
                        a = Mock.PhoneBook.Address.Null;

                    res.Add(new Model.Person(item, p, a));
                }
                return res;
            }

            /// <summary>
            /// Gets all Persons form DB with JOIN
            /// </summary>
            /// <returns>List of Person</returns>
            public List<Model.Person> GetPersonsJoin()
            {
                List<Model.Person> res = new List<Model.Person>();

                m_connection.Open();
                Infrastructure.PhoneBook.IMockConvertor convertor = new BLL.PersonConvertor();

                using (EntityCommand cmd = new EntityCommand(SELECT_PERSONS_ALL, m_connection))
                {
                    using (DbDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> dic = Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue);
                            Mock.PhoneBook.Person person = new BLL.PersonConvertor().ToObject(dic) as Mock.PhoneBook.Person;
                            if (person.ID == 0)
                                person = Mock.PhoneBook.Person.Null;

                            Mock.PhoneBook.Phone phone = new BLL.PhoneConvertor().ToObject(dic) as Mock.PhoneBook.Phone;
                            if (phone.ID == 0)
                                person = Mock.PhoneBook.Person.Null;

                            Mock.PhoneBook.Address address = new BLL.AddressConvertor().ToObject(dic) as Mock.PhoneBook.Address;
                            if (address.ID == 0)
                                person = Mock.PhoneBook.Person.Null;

                            res.Add(new Model.Person(person, phone, address));
                        }
                    }
                }
                m_connection.Close();

                return res;
            }


            #region Select
            /// <summary>
            /// Sectes all Persons from person table into List
            /// </summary>
            /// <returns>List of Person mocks</returns>
            protected List<Mock.PhoneBook.Person> GetPeople()
            {
                List<Mock.PhoneBook.Person> list = new List<Mock.PhoneBook.Person>();

                m_connection.Open();
                Infrastructure.PhoneBook.IMockConvertor convertor = new BLL.PersonConvertor();
            
                using (EntityCommand cmd = new EntityCommand(SELECT_PERSONS_ALL, m_connection))
                {
                    using (DbDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        while (reader.Read())
                        {

                            list.Add(convertor.ToObject(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue)) as Mock.PhoneBook.Person);
                        }
                    }
                }
                m_connection.Close();
                return list;
            }

            /// <summary>
            /// Sectes all Phones from person table into List
            /// </summary>
            /// <returns>List of Phone mocks</returns>
            protected List<Mock.PhoneBook.Phone> GetPhones()
            {
                List<Mock.PhoneBook.Phone> list = new List<Mock.PhoneBook.Phone>();

                m_connection.Open();
                Infrastructure.PhoneBook.IMockConvertor convertor = new BLL.PhoneConvertor();

                using (EntityCommand cmd = new EntityCommand(SELECT_PHONES_ALL, m_connection))
                {
                    using (DbDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        while (reader.Read())
                        {
                            list.Add(convertor.ToObject(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue)) as Mock.PhoneBook.Phone);
                        }
                    }
                }
                m_connection.Close();
                return list;
            }

            /// <summary>
            /// Sectes all Addresses from person table into List
            /// </summary>
            /// <returns>List of Address mocks</returns>
            protected List<Mock.PhoneBook.Address> GetAddresses()
            {
                List<Mock.PhoneBook.Address> list = new List<Mock.PhoneBook.Address>();

                m_connection.Open();
                Infrastructure.PhoneBook.IMockConvertor convertor = new BLL.AddressConvertor();

                using (EntityCommand cmd = new EntityCommand(SELECT_ADDRESSES_ALL, m_connection))
                {
                    using (DbDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        while (reader.Read())
                        {
                            list.Add(convertor.ToObject(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue)) as Mock.PhoneBook.Address);
                        }
                    }
                }
                m_connection.Close();
                return list;
            }
            #endregion

            #region Insert
            /// <summary>
            /// Inserts new Person into Person table
            /// </summary>
            /// <param name="p">Person to be added</param>
            protected void InsertPerson(Mock.PhoneBook.Person p)
            {
                if (p != null)
                {
                    m_sqlconnection.Open();
                    Dictionary<string, object> dic = new BLL.PersonConvertor().ToDictionary(p);
                    SqlCommand command = new SqlCommand(INSERT_PERSON, m_sqlconnection);

                    dic.Where(m=>m.Key!="person_id").ToList().ForEach
                        (o=>command.Parameters.AddWithValue("@" + o.Key, o.Value));

                    if (command.ExecuteNonQuery() == 0)
                        throw new ArgumentException();
                    m_sqlconnection.Close();
                }
                else
                    throw new ArgumentNullException("Object to insert is null");
                
            }

            /// <summary>
            /// Inserts new Phone into Phone table
            /// </summary>
            /// <param name="p">Phone to be added</param>
            protected void InsertPhone(Mock.PhoneBook.Phone p)
            {
                if (p != null)
                {
                    m_sqlconnection.Open();
                    Dictionary<string, object> dic = new BLL.PhoneConvertor().ToDictionary(p);
                    SqlCommand command = new SqlCommand(INSERT_PHONE, m_sqlconnection);

                    dic.Where(m => m.Key != "phone_id").ToList().ForEach
                        (o => command.Parameters.AddWithValue("@" + o.Key, o.Value));

                    if (command.ExecuteNonQuery() == 0)
                        throw new ArgumentException();
                    m_sqlconnection.Close();
                }
                else
                    throw new ArgumentNullException("Object to insert is null");
            }

            /// <summary>
            /// Inserts new Address into Address table
            /// </summary>
            /// <param name="p">Address to be added</param>
            protected void InsertAddress(Mock.PhoneBook.Address p)
            {
                if (p != null)
                {
                    m_sqlconnection.Open();
                    Dictionary<string, object> dic = new BLL.AddressConvertor().ToDictionary(p);
                    SqlCommand command = new SqlCommand(INSERT_ADDRESS, m_sqlconnection);

                    dic.Where(m => m.Key != "address_id").ToList().ForEach
                        (o => command.Parameters.AddWithValue("@" + o.Key, o.Value));

                    if (command.ExecuteNonQuery() == 0)
                        throw new ArgumentException();
                    m_sqlconnection.Close();
                }
                else
                    throw new ArgumentNullException("Object to insert is null");
            }

            #endregion

            #region Update
            /// <summary>
            /// Updates record in DB with spesified value
            /// </summary>
            /// <param name="p">Person to be updated</param>
            protected void UpdatePerson(Mock.PhoneBook.Person p)
            {
                if(p != null)
                {
                    m_sqlconnection.Open();
                    Dictionary<string, object> dic = new BLL.PersonConvertor().ToDictionary(p);
                    SqlCommand command = new SqlCommand(UPDATE_PERSON, m_sqlconnection);

                    dic.Where(m => m.Key != "person_id").ToList().ForEach(o => command.Parameters.AddWithValue("@" + o.Key, o.Value));

                    if (command.ExecuteNonQuery() == 0)
                        throw new ArgumentException();
                    m_sqlconnection.Close();
                }
                else
                    throw new ArgumentNullException("Object to insert is null");
                
            }

            /// <summary>
            /// Updates record in DB with spesified value
            /// </summary>
            /// <param name="p">Phone to be updated</param>
            protected void UpdatePhone(Mock.PhoneBook.Phone p)
            {
                if (p != null)
                {
                    m_sqlconnection.Open();
                    Dictionary<string, object> dic = new BLL.PhoneConvertor().ToDictionary(p);
                    SqlCommand command = new SqlCommand(UPDATE_PHONE, m_sqlconnection);

                    dic.Where(m => m.Key != "phone_id").ToList().ForEach(o => command.Parameters.AddWithValue("@" + o.Key, o.Value));

                    if (command.ExecuteNonQuery() == 0)
                        throw new ArgumentException();
                    m_sqlconnection.Close();
                }
                else
                    throw new ArgumentNullException("Object to insert is null");
                
            }

            /// <summary>
            /// Updates record in DB with spesified value
            /// </summary>
            /// <param name="p">Address to be updated</param>
            protected void UpdateAddress(Mock.PhoneBook.Address p)
            {
                if (p != null)
                {
                    m_sqlconnection.Open();
                    Dictionary<string, object> dic = new BLL.AddressConvertor().ToDictionary(p);
                    SqlCommand command = new SqlCommand(UPDATE_ADDRESS, m_sqlconnection);

                    dic.Where(m => m.Key != "address_id").ToList().ForEach(o => command.Parameters.AddWithValue("@" + o.Key, o.Value));

                    if (command.ExecuteNonQuery() == 0)
                        throw new ArgumentException();
                    m_sqlconnection.Close();
                }
                else
                    throw new ArgumentNullException("Object to insert is null");
            }
            #endregion

            #region Delete

            /// <summary>
            /// Deletes 1 record from Person table with specified value
            /// </summary>
            /// <param name="p">Person record to be deleted</param>
            protected void DeletePerson(Mock.PhoneBook.Person p)
            {
                if (p != null)
                {
                    m_sqlconnection.Open();
                    SqlCommand command = new SqlCommand(DELETE_PERSON, m_sqlconnection);

                   command.Parameters.AddWithValue("@id", p.ID);

                    if (command.ExecuteNonQuery() == 0)
                        throw new ArgumentException();
                    m_sqlconnection.Close();
                }
                else
                    throw new ArgumentNullException("Object to insert is null");
            }

            /// <summary>
            /// Deletes 1 record from Phone table with specified value
            /// </summary>
            /// <param name="p">Phone record to be deleted</param>
            protected void DeletePhone(Mock.PhoneBook.Phone p)
            {
                if (p != null)
                {
                    m_sqlconnection.Open();
                    SqlCommand command = new SqlCommand(DELETE_PHONE, m_sqlconnection);

                    command.Parameters.AddWithValue("@id", p.ID);

                    if (command.ExecuteNonQuery() == 0)
                        throw new ArgumentException();
                    m_sqlconnection.Close();
                }
                else
                    throw new ArgumentNullException("Object to insert is null");
            }

            /// <summary>
            /// Deletes 1 record from Address table with specified value
            /// </summary>
            /// <param name="p">Address record to be deleted</param>
            protected void DeleteAddress(Mock.PhoneBook.Address p)
            {
                if (p != null)
                {
                    m_sqlconnection.Open();
                    SqlCommand command = new SqlCommand(DELETE_ADDRESS, m_sqlconnection);

                    command.Parameters.AddWithValue("@id", p.ID);

                    if (command.ExecuteNonQuery() == 0)
                        throw new ArgumentException();
                    m_sqlconnection.Close();
                }
                else
                    throw new ArgumentNullException("Object to insert is null");
            }
            #endregion

            private EntityConnection m_connection = null;
            private SqlConnection m_sqlconnection = null;
            
        }
    }
}
