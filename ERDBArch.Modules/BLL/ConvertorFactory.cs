using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERDBArch.Modules.PhoneBook.BLL
{
    /// <summary>
    /// Class to get instanse of convertor
    /// </summary>
    public static class ConvertorFactory
    {
        /// <summary>
        /// Gets instance of Convertor for custom Mock
        /// </summary>
        /// <param name="t">Mock type</param>
        /// <returns>Custom convertor instance</returns>
        public static Infrastructure.PhoneBook.IMockConvertor GetInstance(Type t)
        {
            
            if (t == typeof(Mock.PhoneBook.Person))
                return new PersonConvertor();

            if (t == typeof(Mock.PhoneBook.Address))
                return new AddressConvertor();

            if (t == typeof(Mock.PhoneBook.Phone))
                return new PhoneConvertor();

            throw new NotImplementedException("Type not supported");
        }
    }
}
