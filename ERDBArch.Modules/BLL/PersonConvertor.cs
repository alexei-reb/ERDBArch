using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERDBArch.Modules.PhoneBook.BLL
{
    /// <summary>
    /// Convertor class for Person mock
    /// </summary>
    public class PersonConvertor : Infrastructure.PhoneBook.IMockConvertor
    {

        /// <summary>
        /// Makes Person object from dictionary content
        /// </summary>
        /// <param name="dic">Dictionary with values for fields</param>
        /// <returns>New Person object</returns>
        public object ToObject(Dictionary<string, object> dic)
        {
            Mock.PhoneBook.Person p = new Mock.PhoneBook.Person();
            ConvertDictionary converter=dic as ConvertDictionary;
            
            int id = 0;
            string buf;

            if (!converter.TryGet<int>("person_id", out id))
                throw new FormatException("ID is incorrect or missing");

            p.ID = id;
            if (!converter.TryGet<string>("lname", out buf))
                throw new FormatException("LName is incorrect or missing");

            p.LName = buf;
            if (!converter.TryGet<string>("fname", out buf))
                throw new FormatException("FName is incorrect or missing");

            p.LName = buf;


            return p;
        }

        /// <summary>
        /// Fills dictionary with object content
        /// </summary>
        /// <param name="obj">Person with values for dictionary</param>
        /// <returns>Filled dictionary</returns>
        public Dictionary<string, object> ToDictionary(object obj)
        {
            Mock.PhoneBook.Person p = obj as Mock.PhoneBook.Person;
            if (Mock.PhoneBook.Person.Validate(p))
            {
                Dictionary<string, object> res = new Dictionary<string, object>();
                res.Add("person_id", p.ID);
                res.Add("lname", p.LName);
                res.Add("fname", p.FName);
                return res;
            }
            else
                throw new ArgumentException("Object is not valid");
        }

         
    }
}
