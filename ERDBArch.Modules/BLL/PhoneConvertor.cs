using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERDBArch.Modules.PhoneBook.BLL
{
    /// <summary>
    /// Convertor class for Phone mock
    /// </summary>
    public class PhoneConvertor : Infrastructure.PhoneBook.IMockConvertor
    {

        /// <summary>
        /// Makes Phone object from dictionary content
        /// </summary>
        /// <param name="dic">Dictionary with values for fields</param>
        /// <returns>New Phone object</returns>
        public object ToObject(Dictionary<string, object> dic)
        {
            Mock.PhoneBook.Phone p = new Mock.PhoneBook.Phone();
            ConvertDictionary converter = dic as ConvertDictionary;

            int id = 0;
            string buf;
            if (!converter.TryGet<int>("phone_id", out id))
                throw new FormatException("ID is incorrect or missing");

            p.ID = id;
            if (!converter.TryGet<int>("personid", out id))
                throw new FormatException("PersonID is incorrect or missing");

            p.PersonID = id;
            if (!converter.TryGet<string>("phonevalue", out buf))
                throw new FormatException("phonevalue is incorrect or missing");

            p.PhoneValue = buf;

            return p;
        }

        /// <summary>
        /// Fills dictionary with object content
        /// </summary>
        /// <param name="obj">Phone object with values for dictionary</param>
        /// <returns>Filled dictionary</returns>
        public Dictionary<string, object> ToDictionary(object obj)
        {
            Mock.PhoneBook.Phone p = obj as Mock.PhoneBook.Phone;
            if (Mock.PhoneBook.Phone.Validate(p))
            {
                Dictionary<string, object> res = new Dictionary<string, object>();
                res.Add("phone_id", p.ID);
                res.Add("personid", p.PersonID);
                res.Add("phonevalue", p.PhoneValue);
                return res;
            }
            else throw new ArgumentException("Object is not valid");
        }
    }
}
