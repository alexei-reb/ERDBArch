using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERDBArch.Infrastructure.PhoneBook
{
    /// <summary>
    /// Validation
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Determines whethe ibject is valid
        /// </summary>
        /// <param name="o">Object to validate</param>
        /// <returns>Validation result</returns>
        bool Validate(object o);
    }
}
