using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ERDBArch.Mock.PhoneBook
{
    /// <summary>
    /// Person mock class
    /// </summary>
    public class Person : IComparable, ICloneable, ERDBArch.Infrastructure.PhoneBook.IAssignable, ERDBArch.Infrastructure.PhoneBook.IValidatable 
    {
        private const int MAX_NAME = 30;

        /// <summary>
        /// Validation function for Person
        /// </summary>
        /// <param name="p">Person object</param>
        public static bool Validate(Person o)
        {
            Person p = o as Person;
            if(p==null ||  p.isNull)
                return false;
            return p.ID > 0;
        }

        public static Person Parse(string str)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns nullable Person object
        /// </summary>
        public static Person Null
        {
            get
            {
                return new Person() { m_isNull = true };
            }
        }

        /// <summary>
        /// Gets nullable Person object
        /// </summary>
        public bool isNull { get{return m_isNull;}}

        /// <summary>
        /// Gets or sets Person ID
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
        /// Gets or sets first name
        /// </summary>
        public string FName
        {
            get
            {
                return m_fName;
            }
            set
            {
                if (m_isNull)
                    throw new InvalidOperationException("Object is nullable");
                if (value.Length > MAX_NAME)
                    m_fName = value.Substring(0, MAX_NAME);
                else
                    m_fName = value;
            }
        }

        /// <summary>
        /// Gets or sets Last name
        /// </summary>
        public string LName
        {
            get
            {
                return m_lName;
            }
            set
            {
                if (m_isNull)
                    throw new InvalidOperationException("Object is nullable");
                if (value.Length > MAX_NAME)
                    m_lName = value.Substring(0, MAX_NAME);
                else
                    m_lName = value;
            }
        }

        /// <summary>
        /// Validation function for Person
        /// </summary>
        /// <param name="p">Person object</param>
        public bool Validate(object o)
        {
            return Person.Validate(o as Person);
        }

        /// <summary>
        /// Reads this Person object from binary stream
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
                if (reader.ReadInt32() > 0)
                    m_lName = reader.ReadString();
                if (reader.ReadInt32() > 0)
                    m_fName = reader.ReadString();
            }
        }

        /// <summary>
        /// Writes this Person object to binary stream
        /// </summary>
        /// <param name="outStream">Output stream</param>
        public void Write(Stream outStream)
        {
            using (BinaryWriter writer = new BinaryWriter(outStream))
            {
                writer.Write(m_ID == 0 ? 0 : 1);
                writer.Write(m_ID);
                writer.Write(m_lName.Length * 2);
                if (m_lName.Length > 0)
                    writer.Write(m_lName);

                writer.Write(m_fName.Length * 2);
                if (m_fName.Length > 0)
                    writer.Write(m_fName);
            }
        }

        /// <summary>
        /// Compares objects
        /// </summary>
        /// <param name="obj">Object compare with</param>
        /// <returns>-1:this is less;1 this is greater;0-equals</returns>
        public int CompareTo(object obj)
        {
            Person p = obj as Person;
            if (p.m_isNull | this.m_isNull)
                return -1;
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
            if(m_isNull)
                throw new InvalidOperationException("Object is nullable");
            Person p = new Person();
            AssignTo(p);
            return p;
        }

        /// <summary>
        /// Fills object with this object values
        /// </summary>
        /// <param name="o">Object to fill</param>
        public void AssignTo(object o)
        {
            if(o != null && o is Person)
            {
                Person p = o as Person;
                p.ID = this.ID;
                p.FName = this.FName;
                p.LName = this.LName;
                p.m_isNull = this.m_isNull;
            }
        }

        
        private string m_lName = string.Empty;
        private string m_fName = string.Empty;
        private bool m_isNull = false;
        private int m_ID = 0;

    }
}
