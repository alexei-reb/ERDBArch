using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERDBArch.Modules.PhoneBook.BLL
{
    /// <summary>
    /// Provides methods to fill DB entities with mocks values
    /// </summary>
    public static class EntityAssigner
    {
        /// <summary>
        /// Fills Person entity
        /// </summary>
        /// <param name="p">Mock to be copied</param>
        /// <exception cref="ArgumentException">Throw if object is not valid</exception>
        /// <param name="res">Entity to be filled</param>
        public static void AssignPersonEntity(Mock.PhoneBook.Person p, DB.Person res)
        {
            if (Mock.PhoneBook.Person.Validate(p))
            {
                res.lname = p.LName;
                res.fname = p.FName;
            }
            else
                throw new ArgumentException("Object is not valid");
        }

        /// <summary>
        /// Fills Phone entity
        /// </summary>
        /// <param name="p">Mock to be copied</param>
        /// <exception cref="ArgumentException">Throw if object is not valid</exception>
        /// <param name="res">Entity to be filled</param>
        public static void AssignPhoneEntity(Mock.PhoneBook.Phone p, DB.Phone res)
        {
            if (Mock.PhoneBook.Phone.Validate(p))
            {
                res.personid = p.PersonID;
                res.phonevalue = p.PhoneValue;
            }
            else
                throw new ArgumentException("Object is not valid");
        }

        /// <summary>
        /// Fills Address entity
        /// </summary>
        /// <param name="p">Mock to be copied</param>
        /// <exception cref="ArgumentException">Throw if object is not valid</exception>
        /// <param name="res">Entity to be filled</param>
        public static void AssignAddressEntity(Mock.PhoneBook.Address p, DB.Address res)
        {
            if (Mock.PhoneBook.Address.Validate(p))
            {
                res.personid = p.PersonID;
                res.addressvalue = p.AddressValue;
            }
            else
                throw new ArgumentException("Object is not valid");
        }

        
    }
}
