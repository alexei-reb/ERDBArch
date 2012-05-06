using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ERDBArch.Mock.PhoneBook
{
    /// <summary>
    /// Phone mock class
    /// </summary>
    public class Phone : IComparable, ICloneable, ERDBArch.Infrastructure.PhoneBook.IAssignable, ERDBArch.Infrastructure.PhoneBook.IValidatable
    {
        private const int MAX_PHONE_LENGTH = 50;

        /// <summary>
        /// Validation function for phone
        /// </summary>
        /// <param name="p">Phone object</param>
        public static bool Validate(Phone p)
        {
            if (p == null || p.m_isNull)
                return true;
            return p.m_ID > 0;
        }


        public static Phone Parse(string str)
        {
            throw new NotImplementedException();
        }


        public static Phone Null
        {
            get
            {
                return new Phone() { m_isNull = true };
            }
        }
        /// <summary>
        /// Gets nullable Phone object
        /// </summary>
        public bool isNull { get { return m_isNull; } }

        /// <summary>
        /// Gets or sets Phone ID
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
        /// Gets or sets Phone number
        /// </summary>
        public string PhoneValue
        {
            get
            {
                return m_phone;
            }
            set
            {
                if (m_isNull)
                    throw new InvalidOperationException("Object is nullable");
                if (value.Length > MAX_PHONE_LENGTH)
                    m_phone = value.Substring(0, MAX_PHONE_LENGTH);
                else
                    m_phone = value;
            }
        }

        /// <summary>
        /// Validation function for phone
        /// </summary>
        /// <param name="p">Phone object</param>
        public bool Validate(object o)
        {
            return Phone.Validate(o as Phone);
        }

        /// <summary>
        /// Reads this Phone object from binary stream
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
                    m_phone = reader.ReadString();
            }
        }

        /// <summary>
        /// Writes this Phone object to binary stream
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
                writer.Write(m_phone.Length * 2);
                if (m_phone.Length > 0)
                    writer.Write(m_phone);
            }
        }

        /// <summary>
        /// Compares objects
        /// </summary>
        /// <param name="obj">Object compare with</param>
        /// <returns>-1:this is less;1 this is greater;0-equals</returns>
        public int CompareTo(object obj)
        {
            Phone p = obj as Phone;
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

            Phone p = new Phone();
            AssignTo(p);
            return p;
        }

        /// <summary>
        /// Fills object with this object values
        /// </summary>
        /// <param name="o">Object to fill</param>
        public void AssignTo(object o)
        {
            if (o != null && o is Phone)
            {
                Phone p = o as Phone;
                p.ID = this.ID;
                p.PersonID = this.PersonID;
                p.PhoneValue = this.PhoneValue;
            }
        }



        private int m_ID = 0;
        private string m_phone = string.Empty;
        private int m_personID = 0;
        private bool m_isNull = true;
        
    }
}
