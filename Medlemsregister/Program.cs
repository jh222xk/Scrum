using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlemsregister
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            List<Member> members = null;

            // Just iterate until the user want's to quit the program (0).
            do
            {
                // Just a bunch of placeholders here at the moment...
                switch (GetMenuChoice())
                {
                    case 0:
                        exit = true;
                        continue;
                    case 1:
                        // Load member directory
                        members = LoadMembers();
                        break;
                    case 2:
                        // Save members
                        SaveMembers(members);
                        break;
                    case 3:
                        // Add member
                        CreateMember(members);
                        Console.WriteLine("");
                        break;
                    case 4:
                        // Edit member
                        Console.WriteLine("");
                        break;
                    case 5:
                        // Delete member
                        Console.WriteLine("");
                        break;
                    case 6:
                        // List on member
                        ViewMember(members);
                        break;
                    case 7:
                        // List all members
                        ViewMember(members, true);
                        break;
                }

                ContinueOnKeyPressed();
            } while (!exit);
        }

        private static List<Member> LoadMembers()
        {
            try
            {
                MemberDirectory repo = new MemberDirectory("members.txt");
                List<Member> members = repo.Load();
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" ╔═══════════════════════════════════╗ ");
                Console.WriteLine(" ║      Medlemmarna har lästs in     ║ ");
                Console.WriteLine(" ╚═══════════════════════════════════╝ ");
                Console.BackgroundColor = ConsoleColor.Black;
                return members;
            }
            catch (Exception)
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" ╔═══════════════════════════════════╗ ");
                Console.WriteLine(" ║ FEL! Ett fel inträffade då        ║ ");
                Console.WriteLine(" ║      medlemmarna lästes in.       ║ ");
                Console.WriteLine(" ╚═══════════════════════════════════╝ ");
                Console.BackgroundColor = ConsoleColor.Black;
                return null;
            }
        }

        // Method to save members
        private static void SaveMembers(List<Member> members)
        {
            // Check if there's any members otherwise display a warning message.
            if (members != null && members.Any())
            {
                // Try to save it otherwise display an error.
                try
                {
                    MemberDirectory repo = new MemberDirectory("members.txt");
                    repo.Save(members);

                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" ╔═══════════════════════════════════╗ ");
                    Console.WriteLine(" ║      Medlemmarna har sparats      ║ ");
                    Console.WriteLine(" ╚═══════════════════════════════════╝ ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" ╔═══════════════════════════════════╗ ");
                    Console.WriteLine(" ║ FEL! Ett fel inträffade då        ║ ");
                    Console.WriteLine(" ║      medlemmarna skulle sparas.   ║ ");
                    Console.WriteLine(" ╚═══════════════════════════════════╝ ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
            else
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(" ╔══════════════════════════════════════╗ ");
                Console.WriteLine(" ║  Det finns inga medlemmar att spara  ║ ");
                Console.WriteLine(" ╚══════════════════════════════════════╝ ");
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        // Private static method that returns a string value.
        private static string ReadString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string inputValue = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(inputValue))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nFEL! Ange en sträng.\n");
                    Console.ResetColor();
                }
                else
                {
                    return inputValue;
                }
            }
        }

        // Private static method that returns a string value.
        private static string ReadNum(string prompt)
        {
            int number;
            string inputValue;

            while (true)
            {
                Console.Write(prompt);
                inputValue = Console.ReadLine();

                if ((int.TryParse(inputValue, out number) && number >= 0))
                {
                    return inputValue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nFEL! Ange ett nummer.\n");
                    Console.ResetColor();
                }
            }
        }

        // Private static method for creating a member.
        private static Member CreateMember(List<Member> members)
        {
            int userID;
            string firstName;
            string lastName;
            string phoneNum;

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" ╔═══════════════════════════════════╗ ");
            Console.WriteLine(" ║          Lägg till medlem         ║ ");
            Console.WriteLine(" ╚═══════════════════════════════════╝ ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();

            firstName = ReadString(" Förnamn: ");
            lastName = ReadString(" Efternamn: ");
            phoneNum = ReadNum(" Telefonnummer: ");

            if (members.Count < 1)
            {
                userID = 1;
            }
            else
            {
                userID = members[members.Count - 1].UserID + 1;
            }

            // Create a new member.
            Member newMember = new Member(userID, firstName, lastName, phoneNum);

            // Add it to the list.
            members.Add(newMember);

            return newMember;
        }

        // Method to present an indexed list of all members name
        // and let's the user choose from these names and then returns it.
        private static Member GetMember(string header, List<Member> members)
        {
            int index;

            do
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("\n 0. Avbryt.");
                Console.WriteLine("\n -----------------------------------\n");

                // Iterate through the list and display the members name.
                for (int item = 0; item < members.Count; item++)
                {
                    Console.WriteLine(" {0}. {1} {2} ", item + 1, members[item].FirstName, members[item].LastName);
                }
                Console.WriteLine("\n ═══════════════════════════════════ \n");
                Console.Write(" Välj medlem [1-{0}]: ", members.Count);
                Console.ResetColor();
                bool choice = int.TryParse(Console.ReadLine(), out index) && index >= 0 && index <= members.Count;

                if (choice)
                {
                    if (index > 0)
                    {
                        return members[index - 1];
                    }
                    else
                    {
                        return null;
                    }

                }

                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n FEL! Ange ett nummer mellan 1 och {0}.\n", members.Count);
                ContinueOnKeyPressed();
            } while (true);
        }

        // Method to view a member.
        private static void ViewMember(List<Member> members, bool viewAll = false)
        {
            // Check if there's any members otherwise display a warning message.
            if (members != null && members.Any())
            {
                MemberView mView = new MemberView();

                if (!viewAll)
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" ╔═══════════════════════════════════╗ ");
                    Console.WriteLine(" ║       Välj medlem att visa        ║ ");
                    Console.WriteLine(" ╚═══════════════════════════════════╝ ");
                    Console.BackgroundColor = ConsoleColor.Black;

                    // Get the member we want...
                    Member m = GetMember("Välj medlem att visa", members);

                    if (m == null)
                    {
                        return;
                    }
                    // ... and render it.
                    Console.Clear();
                    mView.Render(m);
                }
                else
                {
                    // Renders all the members.
                    Console.Clear();
                    mView.Render(members);
                }
            }
            else
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(" ╔═══════════════════════════════════╗ ");
                Console.WriteLine(" ║ Det finns inga medlemmar att visa ║ ");
                Console.WriteLine(" ╚═══════════════════════════════════╝ ");
                Console.BackgroundColor = ConsoleColor.Black;
            }

        }

        private static int GetMenuChoice()
        {
            int index;

            do
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" ╔═══════════════════════════════════╗ ");
                Console.WriteLine(" ║           Medlemsregister         ║ ");
                Console.WriteLine(" ╚═══════════════════════════════════╝ ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("\n - Arkiv -----------------------------------\n");
                Console.WriteLine(" 0. Avsluta.");
                Console.WriteLine(" 1. Öppna medlemsregister.");
                Console.WriteLine(" 2. Spara medlemmar.");
                Console.WriteLine("\n - Redigera --------------------------------\n");
                Console.WriteLine(" 3. Lägg till medlem.");
                Console.WriteLine(" 4. Ändra medlem.");
                Console.WriteLine(" 5. Ta bort medlem.");
                Console.WriteLine("\n - Lista -----------------------------------\n");
                Console.WriteLine(" 6. Lista en medlem.");
                Console.WriteLine(" 7. Lista alla medlemmar.");
                Console.WriteLine("\n ═══════════════════════════════════════════\n");
                Console.Write(" Ange menyval [0-7]: ");
                Console.ResetColor();

                if (int.TryParse(Console.ReadLine(), out index) && index >= 0 && index <= 7)
                {
                    return index;
                }

                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n FEL! Ange ett nummer mellan 0 och 7.\n");
                ContinueOnKeyPressed();
            } while (true);
        }

        private static void ContinueOnKeyPressed()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write("\n   Tryck tangent för att fortsätta   ");
            Console.ResetColor();
            Console.CursorVisible = false;
            Console.ReadKey(true);
            Console.Clear();
            Console.CursorVisible = true;
        }
    }
}
