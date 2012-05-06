using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERDBArch.Modules.PhoneBook.BLL
{
    /// <summary>
    /// Provides methods for convertation mocks to Entities and back
    /// </summary>
    public static class EntityConvertor
    {
        #region EntityToMock
        /// <summary>
        /// Converts Entity to Person mock
        /// </summary>
        /// <param name="p">Entity to be copied</param>
        /// <returns>Person mock</returns>
        public static Mock.PhoneBook.Person EntityToPersonMock(DB.Person p)
        {
            return new Mock.PhoneBook.Person() { ID = p.id, LName = p.lname, FName = p.fname };
        }

        /// <summary>
        /// Converts Entity to Phone mock
        /// </summary>
        /// <param name="p">Entity to be copied</param>
        /// <returns>Phone mock</returns>
        public static Mock.PhoneBook.Phone EntityToPhoneMock(DB.Phone p)
        {
            return new Mock.PhoneBook.Phone() { ID = p.id, PersonID = (int)p.personid, PhoneValue = p.phonevalue };
        }

        /// <summary>
        /// Converts Entity to Address mock
        /// </summary>
        /// <param name="p">Entity to be copied</param>
        /// <returns>Address mock</returns>
        public static Mock.PhoneBook.Address EntityToAddressMock(DB.Address p)
        {
            return new Mock.PhoneBook.Address() { ID = p.id, PersonID = (int)p.personid, AddressValue = p.addressvalue };
        }

        #endregion

        #region MockToEntity
        /// <summary>
        /// Converts Person mock to Entity
        /// </summary>
        /// <param name="p">Person mock to be copied</param>
        /// <exception cref="ArgumentException">Throw if object is not valid</exception>
        /// <returns>Person Entity</returns>
        public static DB.Person PersonMockToEntity(Mock.PhoneBook.Person p)
        {
            if (Mock.PhoneBook.Person.Validate(p))
            {
                return new DB.Person() { id = p.ID, fname = p.FName, lname = p.LName };
            }
                throw new ArgumentException("Object is not valid");
        }

        /// <summary>
        /// Converts Phone mock to Entity
        /// </summary>
        /// <param name="p">Phone mock to be copied</param>
        /// <exception cref="ArgumentException">Throw if object is not valid</exception>
        /// <returns>Phone Entity</returns>
        public static DB.Phone PhoneMockToEntity(Mock.PhoneBook.Phone p)
        {
            if (Mock.PhoneBook.Phone.Validate(p))
            {
                return new DB.Phone() { id = p.ID, personid=p.PersonID, phonevalue=p.PhoneValue };
            }
            throw new ArgumentException("Object is not valid");
        }

        /// <summary>
        /// Converts Address mock to Entity
        /// </summary>
        /// <param name="p">Address mock to be copied</param>
        /// <exception cref="ArgumentException">Throw if object is not valid</exception>
        /// <returns>Address Entity</returns>
        public static DB.Address AddressMockToEntity(Mock.PhoneBook.Address p)
        {
            if (Mock.PhoneBook.Address.Validate(p))
            {
                return new DB.Address() { id = p.ID, personid = p.PersonID, addressvalue = p.AddressValue };
            }
            throw new ArgumentException("Object is not valid");
        }
        #endregion

    }
}
