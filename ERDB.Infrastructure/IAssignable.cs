using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERDBArch.Infrastructure.PhoneBook
{
    /// <summary>
    /// Filling object
    /// </summary>
    public interface IAssignable
    {
        /// <summary>
        /// Fills object with this object values
        /// </summary>
        /// <param name="o">Object to fill</param>
        void AssignTo(object o);
    }
}
