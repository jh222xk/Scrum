using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Medlemsregister
{
    class MemberDirectory
    {
        // Field
        private string _path;

        // Constructor
        public MemberDirectory(string path)
        {
            Path = path;
        }

        // Property
        public string Path {
            get
            {
                return _path;
            }
            set
            {
                if (value == null || String.IsNullOrWhiteSpace(value))
                {
                    throw new ApplicationException("Path is missing!");
                }
                _path = value;
            }
        }

        // Methods
        public List<Member> Load()
        {
            // Create a member list
            List<Member> memberList = new List<Member>();

            // Encoding
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding(1252);

            using (StreamReader reader = new StreamReader(Path, enc))
            {
                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    // Remove all the semicolons.
                    string[] members = line.Split(';');

                    if (members.Length != 4)
                    {
                        throw new ApplicationException("Wrong formatted file!");
                    }

                    // Creates a member object and initialize it with userid, firstname, lastname and phone number.
                    Member member = new Member();
                    member.UserID = int.Parse(members[0]);
                    member.FirstName = members[1];
                    member.LastName = members[2];
                    member.PhoneNum = members[3];

                    // Add it to the list.
                    memberList.Add(member);
                }
            }

            // Just write out for now...
            foreach (var item in memberList)
            {
                Console.WriteLine("{0}, {1}, {2}, {3}", item.UserID, item.FirstName, item.LastName, item.PhoneNum);
            }

            return memberList;
        }
    }
}
