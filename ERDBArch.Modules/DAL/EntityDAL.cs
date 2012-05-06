using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERDBArch.Modules.PhoneBook.BLL
{
    /// <summary>
    /// Provides CRUD for model object with Entity framework
    /// </summary>
    class EntityDAL
    {
        /// <summary>
        /// Gets all persons from DB
        /// </summary>
        /// <returns>List of all Persons</returns>
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

                res.Add(new Model.Person(item, p,
                    a));
            }
            return res;
        }

        /// <summary>
        /// Inserts Person to DB
        /// </summary>
        /// <exception cref="ArgumentException">Throw if object is null or not valid</exception>
        /// <param name="person">Person to insert</param>
        public void Insert(Model.Person person)
        {
            if (person != null && person.Validate())
            {
                InsertPerson((Mock.PhoneBook.Person)person);
                InsertPhone((Mock.PhoneBook.Phone)person);
                InsertAddress((Mock.PhoneBook.Address)person);
            }
            else
                throw new ArgumentException("Object is not valid");
        }

        /// <summary>
        /// Updates Person in DB
        /// </summary>
        /// <exception cref="ArgumentException">Throw if object is null or not valid</exception>
        /// <param name="person">Person, whis is currently in DB, to update</param>
        public void Update(Model.Person person)
        {
            if (person != null && person.Validate())
            {
                UpdatePerson((Mock.PhoneBook.Person)person);
                UpdatePhone((Mock.PhoneBook.Phone)person);
                UpdateAddress((Mock.PhoneBook.Address)person);
            }
            else
                throw new ArgumentException("Object is not valid");
        }

        /// <summary>
        /// Deletes Person from DB
        /// </summary>
        /// <exception cref="ArgumentException">Throw if object is null</exception>
        /// <param name="person">Person to delete</param>
        public void Delete(Model.Person person)
        {
            if (person != null && person.Validate())
            {
                DeletePerson((Mock.PhoneBook.Person)person);
                DeletePhone((Mock.PhoneBook.Phone)person);
                DeleteAddress((Mock.PhoneBook.Address)person);
            }
            else
                throw new ArgumentException("Object is not valid");
        }

        #region Delete

        /// <summary>
        /// Deletes 1 record from Person table with specified value
        /// </summary>
        /// <param name="p">Person record to be deleted</param>
        protected void DeletePerson(Mock.PhoneBook.Person p)
        {
            m_entities.DeleteObject((from v in m_entities.People where v.id == p.ID select v).First());
            m_entities.SaveChanges();
        }

        /// <summary>
        /// Deletes 1 record from Phone table with specified value
        /// </summary>
        /// <param name="p">Phone record to be deleted</param>
        protected void DeletePhone(Mock.PhoneBook.Phone p)
        {
            m_entities.DeleteObject((from v in m_entities.Phones where v.id == p.ID select v).First());
            m_entities.SaveChanges();
        }

        /// <summary>
        /// Deletes 1 record from Address table with specified value
        /// </summary>
        /// <param name="p">Address record to be deleted</param>
        protected void DeleteAddress(Mock.PhoneBook.Address p)
        {
            m_entities.DeleteObject((from v in m_entities.Addresses where v.id == p.ID select v).First());
            m_entities.SaveChanges();
        }

        #endregion

        #region Update
        /// <summary>
        /// Updates record in DB with spesified value
        /// </summary>
        /// <param name="p">Person to be updated</param>
        protected void UpdatePerson(Mock.PhoneBook.Person p)
        {
            var t = (from v in m_entities.People where v.id == p.ID select v).First();
            BLL.EntityAssigner.AssignPersonEntity(p, t);
            m_entities.Refresh(System.Data.Objects.RefreshMode.ClientWins, t);
            m_entities.SaveChanges();
        }

        /// <summary>
        /// Updates record in DB with spesified value
        /// </summary>
        /// <param name="p">Phone to be updated</param>
        protected void UpdatePhone(Mock.PhoneBook.Phone p)
        {
            var t = (from v in m_entities.Phones where v.id == p.ID select v).First();
            BLL.EntityAssigner.AssignPhoneEntity(p, t);
            m_entities.Refresh(System.Data.Objects.RefreshMode.ClientWins, t);
            m_entities.SaveChanges();
        }

        /// <summary>
        /// Updates record in DB with spesified value
        /// </summary>
        /// <param name="p">Address to be updated</param>
        protected void UpdateAddress(Mock.PhoneBook.Address p)
        {
            var t = (from v in m_entities.Addresses where v.id == p.ID select v).First();
            BLL.EntityAssigner.AssignAddressEntity(p, t);
            m_entities.Refresh(System.Data.Objects.RefreshMode.ClientWins, t);
            m_entities.SaveChanges();
        }
        #endregion

        #region Insert
        /// <summary>
        /// Inserts new Person into Person table
        /// </summary>
        /// <param name="p">Person to be added</param>
        protected void InsertPerson(Mock.PhoneBook.Person p)
        {
            DB.Person e_person = Modules.PhoneBook.BLL.EntityConvertor.PersonMockToEntity(p);
            m_entities.AddToPeople(e_person);
            m_entities.SaveChanges();
        }

        /// <summary>
        /// Inserts new Phone into Phone table
        /// </summary>
        /// <param name="p">Phone to be added</param>
        protected void InsertPhone(Mock.PhoneBook.Phone p)
        {
            DB.Phone e_phone = Modules.PhoneBook.BLL.EntityConvertor.PhoneMockToEntity(p);
            m_entities.AddToPhones(e_phone);
            m_entities.SaveChanges();
        }

        /// <summary>
        /// Inserts new Address into Address table
        /// </summary>
        /// <param name="p">Address to be added</param>
        protected void InsertAddress(Mock.PhoneBook.Address p)
        {
            DB.Address e_address = Modules.PhoneBook.BLL.EntityConvertor.AddressMockToEntity(p);
            m_entities.AddToAddresses(e_address);
            m_entities.SaveChanges();
        }

        #endregion

        #region Select
        /// <summary>
        /// Sectes all Persons from person table into List
        /// </summary>
        /// <returns>List of Person mocks</returns>
        protected List<Mock.PhoneBook.Person> GetPeople()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            List<Mock.PhoneBook.Person> list = new List<Mock.PhoneBook.Person>();
            (from p in m_entities.People select p).ToList().ForEach(x => list.Add
                (new Mock.PhoneBook.Person() { ID = x.id, LName = x.lname, FName=x.fname }));

            return list;
        }

        /// <summary>
        /// Sectes all Phones from person table into List
        /// </summary>
        /// <returns>List of Phone mocks</returns>
        protected List<Mock.PhoneBook.Phone> GetPhones()
        {
            List<Mock.PhoneBook.Phone> list = new List<Mock.PhoneBook.Phone>();

            (from p in m_entities.Phones select p).ToList().ForEach(x => list.Add
                (new Mock.PhoneBook.Phone() { ID = x.id, PersonID = (int)x.personid, PhoneValue = x.phonevalue }));

            return list;
        }

        /// <summary>
        /// Sectes all Addresses from person table into List
        /// </summary>
        /// <returns>List of Address mocks</returns>
        protected List<Mock.PhoneBook.Address> GetAddresses()
        {
            List<Mock.PhoneBook.Address> list = new List<Mock.PhoneBook.Address>();

            (from p in m_entities.Addresses select p).ToList().ForEach(x => list.Add
                (new Mock.PhoneBook.Address() { ID = x.id, PersonID = (int)x.personid, AddressValue = x.addressvalue }));

            return list;
        }
        #endregion

        private ERDBArch.DB.Entities m_entities = new DB.Entities();
    }
}
