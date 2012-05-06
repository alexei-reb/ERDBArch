using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ERDBArch.Mock.PhoneBook
{
    /// <summary>
    /// Address mock class
    /// </summary>
    public class Address : IComparable, ICloneable, ERDBArch.Infrastructure.PhoneBook.IAssignable, ERDBArch.Infrastructure.PhoneBook.IValidatable
    {
        private const int MAX_ADDRESS_LENGTH = 50;

        /// <summary>
        /// Validation for Address
        /// </summary>
        /// <param name="p">Address object</param>
        /// <returns></returns>
        public static bool Validate(Address p)
        {
            if (p == null || p.m_isNull)
                return true;
            return p.m_ID > 0;
        }

        public static Address Parse(string str)
        {
            throw new NotImplementedException();
        }

        public static Address Null
        {
            get
            {
                return new Address() { m_isNull = true };
            }
        }
        /// <summary>
        /// Gets nullable Address object
        /// </summary>
        public bool isNull { get { return m_isNull; } }

        /// <summary>
        /// Gets or sets Address ID
        /// </summary>
        public int ID
        {
            get
            {
                return m_ID;
            }
            set
            {
                if (m_isNull)
                    throw new InvalidOperationException("Object is nullable");
                m_isNull = false;
                m_ID = value;
            }
        }

        /// <summary>
        /// Gets or sets Person ID
        /// </summary>
        public int PersonID
        {
            get
            {
                return m_personID;
            }
            set
            {
                if (m_isNull)
                    throw new InvalidOperationException("Object is nullable");
                m_personID = value;
            }
        }

        /// <summary>
        /// Gets or sets Address
        /// </summary>
        public string AddressValue
        {
            get
            {
                return m_address;
            }
            set
            {
                if (m_isNull)
                    throw new InvalidOperationException("Object is nullable");
                if (value.Length > MAX_ADDRESS_LENGTH)
                    m_address = value.Substring(0, MAX_ADDRESS_LENGTH);
                else
                    m_address = value;
            }
        }

        /// <summary>
        /// Validation function for Address
        /// </summary>
        /// <param name="p">Address object</param>
        public bool Validate(object o)
        {
            return Address.Validate(o as Address);
        }

        /// <summary>
        /// Reads this Address object from binary stream
        /// </summary>
        /// <param name="inStream">Input stream</param>
        public void Read(Stream inStream)
        {
            using (BinaryReader reader = new BinaryReader(inStream))
            {
                if (reader.ReadInt32() != 0)
                    m_ID = reader.ReadInt32();
                else
                    reader.ReadInt32();

                if (reader.ReadInt32() != 0)
                    m_personID = reader.ReadInt32();
                else
                    reader.ReadInt32();

                if (reader.ReadInt32() > 0)
                    m_address = reader.ReadString();
            }
        }

        /// <summary>
        /// Writes this Address object to binary stream
        /// </summary>
        /// <param name="outStream">Output stream</param>
        public void Write(Stream outStream)
        {
            using (BinaryWriter writer = new BinaryWriter(outStream))
            {
                writer.Write(m_ID == 0 ? 0 : 1);
                writer.Write(m_ID);
                writer.Write(m_personID == 0 ? 0 : 1);
                writer.Write(m_personID);
                writer.Write(m_address.Length * 2);
                if (m_address.Length > 0)
                    writer.Write(m_address);
            }
        }

        /// <summary>
        /// Compares objects
        /// </summary>
        /// <param name="obj">Object compare with</param>
        /// <returns>-1:this is less;1 this is greater;0-equals</returns>
        public int CompareTo(object obj)
        {
            Address p = obj as Address;
            if (p.m_isNull && this.m_isNull)
                return -1;
            return this.ID.CompareTo(p.ID);
        }

        /// <summary>
        /// Clone this object
        /// </summary>
        /// <returns>Copy of this</returns>
        public object Clone()
        {
            if (m_isNull)
                throw new InvalidOperationException("Object is nullable");

            Address p = new Address();
            AssignTo(p);
            return p;
        }

        /// <summary>
        /// Fills object with this object values
        /// </summary>
        /// <param name="o">Object to fill</param>
        public void AssignTo(object o)
        {
            if (o != null && o is Address)
            {
                Address p = o as Address;
                p.ID = this.ID;
                p.PersonID = this.PersonID;
                p.AddressValue = this.AddressValue;
            }
        }

        private int m_ID = 0;
        private string m_address = string.Empty;
        private int m_personID = 0;
        private bool m_isNull = false;
    }
}
