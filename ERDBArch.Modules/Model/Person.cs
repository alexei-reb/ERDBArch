using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ERDBArch.Modules.PhoneBook.Model
{
    /// <summary>
    /// Presents a facade for mocks
    /// </summary>
    class Person : System.ComponentModel.IDataErrorInfo
    {

        private const string REGEX_NAME = "^\\w*$";
        private const string REGEX_PHONE = "^\\+?\\d?\\d?[(]?\\d?[ -]?\\d{2}[)]?[ -]?\\d{3}([ -]?\\d{2}){2}$";

        /// <summary>
        /// Creates a new Person
        /// </summary>
        public Person()
        {
            m_personMock = new Mock.PhoneBook.Person();
            m_phoneMock = new Mock.PhoneBook.Phone();
            m_addressMock = new Mock.PhoneBook.Address();
        }

        /// <summary>
        /// Creates a Person from filled mocks
        /// </summary>
        /// <param name="person">Person mock</param>
        /// <param name="phone">Phone mock</param>
        /// <param name="address">Address mock</param>
        internal Person(Mock.PhoneBook.Person person, Mock.PhoneBook.Phone phone, Mock.PhoneBook.Address address)
        {
            person.AssignTo(m_personMock);
            phone.AssignTo(m_phoneMock);
            address.AssignTo(m_addressMock);

            if (person.isNull)
                person = new Mock.PhoneBook.Person();
            if (phone.isNull)
                phone = new Mock.PhoneBook.Phone();
            if (address.isNull)
                address = new Mock.PhoneBook.Address();
        }

        #region Implicit
        /// <summary>
        /// Implicit convertion to Person mock
        /// </summary>
        /// <param name="p">Person to convert</param>
        /// <returns></returns>
        public static implicit operator Mock.PhoneBook.Person(Person p)
        {
            return p.m_personMock;
        }

        /// <summary>
        /// Implicit convertion to Phone mock
        /// </summary>
        /// <param name="p">Person to convert</param>
        /// <returns></returns>
        public static implicit operator Mock.PhoneBook.Phone(Person p)
        {
            return p.m_phoneMock;
        }

        /// <summary>
        /// Implicit convertion to Address mock
        /// </summary>
        /// <param name="p">Person to convert</param>
        /// <returns></returns>
        public static implicit operator Mock.PhoneBook.Address(Person p)
        {
            return p.m_addressMock;
        }

        #endregion

        #region Props
        /// <summary>
        /// Gets or sets Person ID
        /// </summary>
        public int ID
        {
            get
            {
                return m_personMock.ID;
            }
            set
            {
                if (value < 1)
                {
                    if (!errors.ContainsKey("ID"))
                        errors.Add("ID", "ID can not be less 0");
                }
                else
                {
                    errors.Remove("ID");
                }
                m_personMock.ID = value;
            }
        }

        /// <summary>
        /// Gets or sets last name
        /// </summary>
        public string LName
        {
            get
            {
                return m_personMock.LName;
            }
            set
            {
                if (!Regex.IsMatch(value, REGEX_NAME))
                {
                    if (!errors.ContainsKey("LName"))
                        errors.Add("LName", "LName is not valid");
                }
                else
                {
                    errors.Remove("LName");
                }
                m_personMock.LName = value;
            }
        }

        /// <summary>
        /// Gets or sets first name
        /// </summary>
        public string FName
        {
            get
            {
                return m_personMock.FName;
            }
            set
            {
                if (!Regex.IsMatch(value, REGEX_NAME))
                {
                    if (!errors.ContainsKey("FName"))
                        errors.Add("FName", "FName is not valid");
                }
                else
                {
                    errors.Remove("FName");
                }
                m_personMock.FName = value;
            }
        }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        public string Phone
        {
            get
            {
                return m_phoneMock.PhoneValue;
            }
            set
            {
                if (!Regex.IsMatch(value, REGEX_PHONE))
                    errors.Add("Phone", "Phone is not valid");
                m_phoneMock.PhoneValue = value;
            }
        }

        /// <summary>
        /// Gets or sets address
        /// </summary>
        public string Address
        {
            get
            {
                return m_addressMock.AddressValue;
            }
            set 
            {
                m_addressMock.AddressValue = value;
            }
        }

        #endregion

        #region IDataErrorInfo
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets erros for custom property
        /// </summary>
        /// <param name="columnName">Property to search for errors</param>
        /// <returns>Error message</returns>
        public string this[string columnName]
        {
            get
            {
                return (!errors.ContainsKey(columnName) ? null :
                  String.Join(Environment.NewLine, errors[columnName]));
            }
        }

        #endregion

        /// <summary>
        /// Determines, whether object is valid
        /// </summary>
        /// <returns>Validation result</returns>
        public bool Validate()
        {
            if (errors.Count == 0)
                return true;
            return false;
        }

        private Mock.PhoneBook.Person m_personMock = new Mock.PhoneBook.Person();
        private Mock.PhoneBook.Phone m_phoneMock = new Mock.PhoneBook.Phone();
        private Mock.PhoneBook.Address m_addressMock = new Mock.PhoneBook.Address();
        private Dictionary<string, string> errors = new Dictionary<string, string>();

    }
}
