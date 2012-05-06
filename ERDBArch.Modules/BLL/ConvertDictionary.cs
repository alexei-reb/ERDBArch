using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERDBArch.Modules.PhoneBook.BLL
{
    /// <summary>
    /// Expands Dictionary for TryGet
    /// </summary>
    public class ConvertDictionary:Dictionary<string, object>
    {
        /// <summary>
        /// Trys to get value from dictionary and convert to type
        /// </summary>
        /// <typeparam name="T">Type to convert value</typeparam>
        /// <param name="name">Dictionary key value</param>
        /// <param name="ent">Converted value</param>
        /// <returns>true for sucsess; false for failure</returns>
        public bool TryGet<T>(string name, out T ent)
        {
            if (!this.ContainsKey(name) || !(this[name] is T))
            {
                ent = default(T);
                return false;
            }
            
            ent = (T)this[name];
            return true;
        }
    }
}
