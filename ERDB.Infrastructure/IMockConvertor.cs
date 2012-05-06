using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ERDBArch.Infrastructure.PhoneBook
{
    /// <summary>
    /// Convertation from mock to dictionary and back
    /// </summary>
    public interface IMockConvertor
    {
        /// <summary>
        /// Makes object from dictionary content
        /// </summary>
        /// <param name="dic">Dictionary with values for fields</param>
        /// <returns>New filled object</returns>
        object ToObject(Dictionary<string, object> dic);

        /// <summary>
        /// Fills dictionary with object content
        /// </summary>
        /// <param name="obj">Object with values for dictionary</param>
        /// <returns>Filled dictionary</returns>
        Dictionary<string, object> ToDictionary(object obj);
    }
}
