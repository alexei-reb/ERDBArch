using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERDBArch.Modules.PhoneBook.BLL
{
    /// <summary>
    /// Convertor class for Address mock
    /// </summary>
    public class AddressConvertor : Infrastructure.PhoneBook.IMockConvertor
    {
        /// <summary>
        /// Makes Address object from dictionary content
        /// </summary>
        /// <param name="dic">Dictionary with values for fields</param>
        /// <returns>New Address object</returns>
        public object ToObject(Dictionary<string, object> dic)
        {
            Mock.PhoneBook.Address p = new Mock.PhoneBook.Address();
            ConvertDictionary converter = dic as ConvertDictionary;

            int id = 0;
            string buf;
            if (!converter.TryGet<int>("phone_id", out id))
                throw new FormatException("ID is incorrect or missing");
            
            p.ID = id;
            if (!converter.TryGet<int>("personid", out id))
                throw new FormatException("PersonID is incorrect or missing");
            
            p.PersonID = id;
            if (!converter.TryGet<string>("addressvalue", out buf))
                throw new FormatException("AddressValue is incorrect or missing");
            
            p.AddressValue = buf;

            return p;
        }

        /// <summary>
        /// Fills dictionary with object content
        /// </summary>
        /// <param name="obj">Address with values for dictionary</param>
        /// <returns>Filled dictionary</returns>
        public Dictionary<string, object> ToDictionary(object obj)
        {
            Mock.PhoneBook.Address p = obj as Mock.PhoneBook.Address;
            if (Mock.PhoneBook.Address.Validate(p))
            {
                Dictionary<string, object> res = new Dictionary<string, object>();
                res.Add("phone_id", p.ID);
                res.Add("personid", p.PersonID);
                res.Add("addressvalue", p.AddressValue);
                return res;
            }
            else throw new ArgumentException("Object is not valid");
        }
    }
}
