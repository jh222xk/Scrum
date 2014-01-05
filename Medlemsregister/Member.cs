using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlemsregister
{
    class Member : IComparable, IComparable<Member>
    {
        // Fields
        private int _userID;
        private string _firstName;
        private string _lastName;
        private string _phoneNum;

        public int UserID
        {
            get
            {
                return _userID;
            }
            set
            {
                _userID = value;
            }
        }

        public string FirstName {
            get
            {
                return _firstName;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ApplicationException("Firstname is missing!");
                }
                _firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ApplicationException("Lastname is missing!");
                }
                _lastName = value;
            }
        }

        public string PhoneNum
        {
            get
            {
                return _phoneNum;
            }
            set
            {
                _phoneNum = value;
            }
        }

        // Constructors
        public Member()
        {
            // Empty!
        }

        public Member(int userID, string firstName, string lastName, string phoneNum)
        {
            UserID = userID;
            FirstName = firstName;
            LastName = lastName;
            PhoneNum = phoneNum;
        }

        public int CompareTo(object obj)
        {
            return this.CompareTo(obj);
        }

        public int CompareTo(Member other)
        {
            return this.FirstName.CompareTo(other.FirstName);
        }


    }
}
