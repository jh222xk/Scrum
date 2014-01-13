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
        // Method to read the file.
        public List<Member> Load()
        {
            // Create a member list
            List<Member> memberList = new List<Member>();

            // Encoding
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding(1252);

            using (StreamReader reader = new StreamReader(Path, enc))
            {
                string line = null;

                // Go through the register.
                while ((line = reader.ReadLine()) != null)
                {
                    // If there's a empty line in the file just continue.
                    if (line == String.Empty)
                    {
                        continue;
                    }

                    // Remove all the semicolons.
                    string[] members = line.Split(';');

                    // Check if it's four parts e.g. 1;Nisse;Hult;0701234567
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

            // Sort it by UserID...
            memberList.Sort();

            // ... and return the list.
            return memberList;
        }

        // Method for saving to the file.
        public void Save(List<Member> members)
        {
            using (StreamWriter writer = new StreamWriter(Path, false, System.Text.Encoding.UTF8))
            {
                // Iterate through the list of members.
                foreach (var member in members)
                {
                    // Write everything to the file.
                    writer.WriteLine("{0};{1};{2};{3}", member.UserID, member.FirstName, member.LastName, member.PhoneNum);
                }
            }
        }
    }
}
