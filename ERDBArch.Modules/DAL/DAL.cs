using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace ERDBArch.Modules.PhoneBook.BLL
{
    /// <summary>
    /// Data base entry point
    /// </summary>
    class DAL
    {
        private const string SELECT = "select * from {0}";
        private const string INSERT = "INSERT INTO {0} ({1}) VALUES ({2})";
        private const string SELECT_WHERE = "select * from {0} where PersonID={1}";

        /// <summary>
        /// Connects to SQL server
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        public void Connect(string connectionString)
        {
            m_connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Gets all Persons form DB
        /// </summary>
        /// <returns>List of Person</returns>
        public List<Model.Person> GetPersons()
        {
            List<Model.Person> res = new List<Model.Person>();
            List<Mock.PhoneBook.Person> persons = GetMocks<Mock.PhoneBook.Person>(m_trans);
            
            foreach (var item in persons)
            {
                res.Add(new Model.Person(item, GetPhone(item.ID, m_trans), GetAddress(item.ID, m_trans)));
            }
            return res;
        }

        /// <summary>
        /// Inserts new Person to DB
        /// </summary>
        /// <param name="person">Person to insert</param>
        public void AddPerson(Model.Person person)
        {
            if (person!=null && person.Validate())
            {
                Insert(new PersonConvertor(), (Mock.PhoneBook.Person)person, m_trans);
                Insert(new PhoneConvertor(), (Mock.PhoneBook.Phone)person, m_trans);
                Insert(new AddressConvertor(), (Mock.PhoneBook.Address)person, m_trans);
            }
            else
                throw new ArgumentException();
        }

        /// <summary>
        /// Gets Mock collection of custom type
        /// </summary>
        /// <typeparam name="T">Type of mock</typeparam>
        /// <param name="trans">Transaction parameter</param>
        /// <returns>List of mock</returns>
        protected List<T> GetMocks<T>(System.Data.Common.DbTransaction trans) where T : class, new()
        {
            List<T> res = null;
            m_connection.Open();
            SqlCommand command = new SqlCommand(string.Format(SELECT, typeof(T).Name), m_connection, trans as SqlTransaction);
            SqlDataReader reader = command.ExecuteReader();
            Infrastructure.PhoneBook.IMockConvertor convertor = ConvertorFactory.GetInstance(typeof(T));
            while (reader.Read())
            {
                
                res.Add(convertor.ToObject(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue)) as T);
            }
            reader.Close();
            m_connection.Close();
            return res;
        }


        /// <summary>
        /// Gets Phone mock object for by PersonID
        /// </summary>
        /// <param name="PersonID">PersonID to search for</param>
        /// <param name="trans">Transaction parameter</param>
        /// <returns>Phone mock</returns>
        protected Mock.PhoneBook.Phone GetPhone(int PersonID, System.Data.Common.DbTransaction trans)
        {
            Mock.PhoneBook.Phone res = null;
            m_connection.Open();

            SqlCommand command = new SqlCommand(string.Format(SELECT_WHERE, "Phone", PersonID), m_connection, trans as SqlTransaction);
            SqlDataReader reader = command.ExecuteReader();
            PhoneConvertor convertor = new PhoneConvertor();
            if (reader.HasRows)
                res = convertor.ToObject(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue)) as Mock.PhoneBook.Phone;
            else
                res = Mock.PhoneBook.Phone.Null;

            m_connection.Close();
            return res;
        }

        /// <summary>
        /// Gets Address mock object for by PersonID
        /// </summary>
        /// <param name="PersonID">PersonID to search for</param>
        /// <param name="trans">Transaction parameter</param>
        /// <returns>Address mock</returns>
        protected Mock.PhoneBook.Address GetAddress(int PersonID, System.Data.Common.DbTransaction trans)
        {
            Mock.PhoneBook.Address res = null;
            m_connection.Open();

            SqlCommand command = new SqlCommand(string.Format(SELECT_WHERE, "Address", PersonID), m_connection);
            SqlDataReader reader = command.ExecuteReader();
            PhoneConvertor convertor = new PhoneConvertor();
            if (reader.HasRows)
                res = convertor.ToObject(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue)) as Mock.PhoneBook.Address;
            else
                res = Mock.PhoneBook.Address.Null;

            m_connection.Close();
            return res;
        }

        /// <summary>
        /// Inserts mock to DB
        /// </summary>
        /// <param name="convertor">Convertor for custom mock type</param>
        /// <param name="insertObj">Object to insert</param>
        /// <param name="trans">Transaction parameter</param>
        protected void Insert(Infrastructure.PhoneBook.IMockConvertor convertor, object insertObj, System.Data.Common.DbTransaction trans)
        {
            if (insertObj != null && convertor!=null)
            {
                m_connection.Open();
                Dictionary<string, object> dic = convertor.ToDictionary(insertObj);
                string pars = String.Join(",", dic.Keys);
                string vals = String.Join(",", dic.Values);

                SqlCommand command = new SqlCommand(string.Format(INSERT, insertObj.GetType().Name, pars, vals), m_connection);

                if (command.ExecuteNonQuery() == 0)
                    throw new ArgumentException();
                m_connection.Close();
            }
            else
                throw new ArgumentNullException("Object to insert is null");
        }


        SqlConnection m_connection = null;
        SqlTransaction m_trans = null;
    }
}
