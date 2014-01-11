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
            List<Member> members = LoadMembers();

            // Just iterate until the user want's to quit the program (0).
            if (members != null)
            {
                do
                {
                    switch (GetMenuChoice())
                    {
                        case 0:
                            exit = true;
                            continue;
                        case 1:
                            // Add member
                            CreateMember(members);
                            break;
                        case 2:
                            // Edit member
                            EditMember(members);
                            break;
                        case 3:
                            // Delete member
                            DeleteMember(members);
                            break;
                        case 4:
                            // List one member
                            ViewMember(members);
                            break;
                        case 5:
                            // List all members
                            ViewMember(members, true);
                            break;
                    }

                    ContinueOnKeyPressed();
                } while (!exit);
            }
        }

        private static List<Member> LoadMembers()
        {
            try
            {
                MemberDirectory repo = new MemberDirectory("members.txt");
                List<Member> members = repo.Load();
                return members;
            }
            catch (Exception)
            {
                Console.Clear();
                MemberView.RenderHeader(" FEL! Ett fel inträffade då medlemsregistret lästes in.", bgcolor: ConsoleColor.Red);
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
                    MemberView.RenderHeader("      Medlemmarna har sparats     ", bgcolor: ConsoleColor.DarkGreen);
                }
                catch (Exception)
                {
                    Console.Clear();
                    MemberView.RenderHeader(" FEL! Ett fel inträffade då medlemmarna skulle sparas.", bgcolor: ConsoleColor.Red);
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
            if (members != null)
            {
                int userID;
                string firstName;
                string lastName;
                string phoneNum;

                Console.Clear();
                MemberView.RenderHeader("          Lägg till medlem        ");
                Console.WriteLine();

                firstName = ReadString(" Förnamn: ");
                lastName = ReadString(" Efternamn: ");
                phoneNum = ReadNum(" Telefonnummer: ");

                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("\n Är du säker på att du vill lägga till en medlem med förnamn '{0}', ", firstName);
                Console.Write("\n efternamn '{0}' samt telefonnummer '{1}' [j/n]: ", lastName, phoneNum);
                Console.ResetColor();
                ConsoleKeyInfo inputKey = Console.ReadKey();

                if (inputKey.KeyChar == 'j' || inputKey.KeyChar == 'J')
                {
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

                    SaveMembers(members);

                    return newMember;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            
        }

        // Private static method for editing a member.
        private static void EditMember(List<Member> members)
        {
            int index;
            string firstName;
            string lastName;
            string phoneNum;
            string header;
            bool exit = false;

            // Check if there's any members otherwise display a warning message.
            if (members != null && members.Any())
            {
                Console.Clear();
                MemberView.RenderHeader("       Välj medlem att ändra      ");

                Member m = GetMember("Välj medlem att ändra", members);

                if (m == null)
                {
                    return;
                }

                Console.Clear();
                header = String.Format("      Ändra medlem, {0} {1}     ", m.FirstName, m.LastName);
                MemberView.RenderHeader(header);

                do
                {
                    Console.WriteLine("\n - Arkiv -----------------------------------\n");
                    Console.WriteLine(" 0. Avbryt.");
                    Console.WriteLine("\n - Redigera --------------------------------\n");
                    Console.WriteLine(" 1. Ändra förnamn.");
                    Console.WriteLine(" 2. Ändra efternamn.");
                    Console.WriteLine(" 3. Ändra Telefonnummer.");
                    Console.WriteLine(" 4. Ändra allt.");
                    Console.WriteLine("\n ═══════════════════════════════════════════\n");
                    Console.Write(" Ange menyval [0-4]: ");
                    Console.ResetColor();

                    bool choice = int.TryParse(Console.ReadLine(), out index) && index >= 0 && index <= 4;

                    if (choice)
                    {
                        Console.Clear();
                        header = String.Format("      Ändrar medlem, {0} {1}     ", m.FirstName, m.LastName);
                        MemberView.RenderHeader(header);
                        Console.WriteLine();

                        if (index == 0)
	                    {
		                    exit = true;
                            continue;
	                    }
                        else if (index == 1)
	                    {
		                    firstName = ReadString(" Förnamn: ");
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("\n Är du säker på att du vill ändra '{0} {1}'s förnamn till '{2}'? [j/n]: ", m.FirstName, m.LastName, firstName);
                            Console.ResetColor();
                            ConsoleKeyInfo inputKey = Console.ReadKey();

                            if (inputKey.KeyChar == 'j' || inputKey.KeyChar == 'J')
                            {
                                m.FirstName = firstName;
                                SaveMembers(members);
                                Console.Clear();
                                MemberView.RenderHeader(" Medlem har ändrats", bgcolor: ConsoleColor.DarkGreen);
                                exit = true;
                                continue;
                            }
                            else
                            {
                                exit = true;
                                continue;
                            }
	                    }
                        else if (index == 2)
	                    {
		                    lastName = ReadString(" Efternamn: ");
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("\n Är du säker på att du vill ändra '{0} {1}'s efternamn till '{2}'? [j/n]: ", m.FirstName, m.LastName, lastName);
                            Console.ResetColor();
                            ConsoleKeyInfo inputKey = Console.ReadKey();

                            if (inputKey.KeyChar == 'j' || inputKey.KeyChar == 'J')
                            {
                                m.LastName = lastName;
                                SaveMembers(members);
                                Console.Clear();
                                MemberView.RenderHeader("         Medlem har ändrats       ", bgcolor: ConsoleColor.DarkGreen);
                                exit = true;
                                continue;
                            }
                            else
                            {
                                exit = true;
                                continue;
                            }
	                    }
                        else if (index == 3)
	                    {
		                    phoneNum = ReadNum(" Telefonnummer: ");
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("\n Är du säker på att du vill ändra '{0} {1}'s telefonnummer till '{2}'? [j/n]: ", m.FirstName, m.LastName, phoneNum);
                            Console.ResetColor();
                            ConsoleKeyInfo inputKey = Console.ReadKey();

                            if (inputKey.KeyChar == 'j' || inputKey.KeyChar == 'J')
                            {
                                m.PhoneNum = phoneNum;
                                SaveMembers(members);
                                Console.Clear();
                                MemberView.RenderHeader("         Medlem har ändrats       ", bgcolor: ConsoleColor.DarkGreen);
                                exit = true;
                                continue;
                            }
                            else
                            {
                                exit = true;
                                continue;
                            }
	                    }
                        else if (index == 4)
	                    {
		                    firstName = ReadString(" Förnamn: ");
                            lastName = ReadString(" Efternamn: ");
                            phoneNum = ReadNum(" Telefonnummer: ");
                            
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("\n Är du säker på att du vill ändra '{0} {1}'s förnamn till '{2}', ", m.FirstName, m.LastName, firstName);
                            Console.Write("\n efternamn till '{0}' samt telefonnummer till '{1}'? [j/n]: ", lastName, phoneNum);
                            Console.ResetColor();
                            ConsoleKeyInfo inputKey = Console.ReadKey();

                            if (inputKey.KeyChar == 'j' || inputKey.KeyChar == 'J')
                            {
                                m.FirstName = firstName;
                                m.LastName = lastName;
                                m.PhoneNum = phoneNum;
                                SaveMembers(members);
                                Console.Clear();
                                MemberView.RenderHeader("         Medlem har ändrats       ", bgcolor: ConsoleColor.DarkGreen);
                                exit = true;
                                continue;
                            }
                            else
                            {
                                exit = true;
                                continue;
                            }
	                    }

                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("\n FEL! Ange ett nummer mellan 0 och 6.\n");
                    }
                    ContinueOnKeyPressed();
                } while (!exit);
            }
            else
            {
                Console.Clear();
                MemberView.RenderHeader(" Det finns inga medlemmar att ändra", bgcolor: ConsoleColor.Yellow, fgcolor: ConsoleColor.Black);
            }
        }
        // Method to delete a recipe from the list of recipes.
        private static void DeleteMember(List<Member> members)
        {
            // Check if there's any recipes otherwise display a warning message.
            if (members != null && members.Any())
            {
                Console.Clear();
                MemberView.RenderHeader("      Välj medlem att ta bort     ");

                Member r = GetMember("Välj medlem att ta bort", members);

                if (r == null)
                {
                    return;
                }
            }
            else
            {
                Console.Clear();
                MemberView.RenderHeader(" Det finns inga medlemmar att ta bort", bgcolor: ConsoleColor.Yellow, fgcolor: ConsoleColor.Black);
            }
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
                Console.Write(" Välj medlem [1-{0}, avbryt (0)]: ", members.Count);
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
            string header;

            // Check if there's any members otherwise display a warning message.
            if (members != null && members.Any())
            {
                MemberView mView = new MemberView();

                if (!viewAll)
                {
                    Console.Clear();
                    MemberView.RenderHeader("       Välj medlem att visa       ");

                    // Get the member we want...
                    Member m = GetMember("Välj medlem att visa", members);

                    if (m == null)
                    {
                        return;
                    }
                    // ... and render it.
                    Console.Clear();
                    header = String.Format("      Visar medlem, {0} {1}      ", m.FirstName, m.LastName);
                    MemberView.RenderHeader(header);
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
                MemberView.RenderHeader(" Det finns inga medlemmar att visa", bgcolor: ConsoleColor.Yellow, fgcolor: ConsoleColor.Black);
            }

        }

        private static int GetMenuChoice()
        {
            int index;

            do
            {
                //Console.Clear();
                MemberView.RenderHeader("          Medlemsregister         ");
                Console.WriteLine("\n - Arkiv ------------------------------\n");
                Console.WriteLine(" 0. Avsluta.");
                Console.WriteLine(" 1. Lägg till medlem.");
                Console.WriteLine("\n - Redigera ---------------------------\n");
                Console.WriteLine(" 2. Ändra medlem.");
                Console.WriteLine(" 3. Ta bort medlem.");
                Console.WriteLine("\n - Lista ------------------------------\n");
                Console.WriteLine(" 4. Lista en medlem.");
                Console.WriteLine(" 5. Lista alla medlemmar.");
                Console.WriteLine();
                MemberView.HorizontalLine(39);
                Console.WriteLine();
                Console.Write(" Ange menyval [0-5]: ");
                Console.ResetColor();

                if (int.TryParse(Console.ReadLine(), out index) && index >= 0 && index <= 5)
                {
                    return index;
                }

                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n FEL! Ange ett nummer mellan 0 och 5.\n");
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
