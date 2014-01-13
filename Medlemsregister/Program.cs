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

            // Really see so the file is correctly loaded.
            if (members != null)
            {
                // Just iterate until the user want's to quit the program (0).
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

        // Private static method for loading the list of members.
        private static List<Member> LoadMembers()
        {
            MemberView mView = new MemberView();

            // Try to open a file named "members.txt" and the return the list so we can use that list
            // for all the other methods.
            try
            {
                MemberDirectory repo = new MemberDirectory("members.txt");
                List<Member> members = repo.Load();
                return members;
            }
            // Else catch the exception and return NULL as well as a error message will be presented.
            catch (Exception)
            {
                // Display a header from the RenderHeader method.
                Console.Clear();
                mView.RenderHeader(" FEL! Ett fel inträffade då medlemsregistret lästes in.", bgcolor: ConsoleColor.Red);

                // And return null.
                return null;
            }
        }

        // Method to save members
        private static void SaveMembers(List<Member> members)
        {
            MemberView mView = new MemberView();

            // Check if there's any members otherwise display a warning message.
            if (members != null && members.Any())
            {
                // Try to save it otherwise display an error.
                try
                {
                    MemberDirectory repo = new MemberDirectory("members.txt");
                    repo.Save(members);

                    // Display a header from the RenderHeader method.
                    Console.Clear();
                    mView.RenderHeader("      Medlemmarna har sparats     ", bgcolor: ConsoleColor.DarkGreen);
                }
                catch (Exception)
                {
                    // Display a header from the RenderHeader method.
                    Console.Clear();
                    mView.RenderHeader(" FEL! Ett fel inträffade då medlemmarna skulle sparas.", bgcolor: ConsoleColor.Red);
                }
            }
            else
            {
                // Display a header from the RenderHeader method.
                Console.Clear();
                mView.RenderHeader("  Det finns inga medlemmar att spara ", bgcolor: ConsoleColor.Yellow, fgcolor: ConsoleColor.Black);
            }
        }

        // Private static method that returns a string value.
        private static string ReadString(string prompt)
        {
            // Iterate until the users pass a valid value.
            while (true)
            {
                Console.Write(prompt);
                string inputValue = Console.ReadLine();

                // Check if it's really a string.
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

            // Iterate until the users pass a valid value.
            while (true)
            {
                Console.Write(prompt);
                inputValue = Console.ReadLine();

                // Just check if what the user types is a integer but return a string.
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
                MemberView mView = new MemberView();

                // Display a header from the RenderHeader method.
                Console.Clear();
                mView.RenderHeader("          Lägg till medlem        ");
                Console.WriteLine();

                // Call the methods for getting validated data.
                firstName = ReadString(" Förnamn: ");
                lastName = ReadString(" Efternamn: ");
                phoneNum = ReadNum(" Telefonnummer: ");

                // Output a "Are you sure prompt?" so the user can think two times.
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("\n Är du säker på att du vill lägga till en medlem med förnamn '{0}', ", firstName);
                Console.Write("\n efternamn '{0}' samt telefonnummer '{1}' [j/n]: ", lastName, phoneNum);
                Console.ResetColor();

                // Read the key...
                ConsoleKeyInfo inputKey = Console.ReadKey();

                // ... and check if the key is either small or large "j".
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

                    // Save the member to the file...
                    SaveMembers(members);

                    // ... and return it.
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
            MemberView mView = new MemberView();

            // Check if there's any members otherwise display a warning message.
            if (members != null && members.Any())
            {
                // Get the member we want using the GetMember method.
                Member m = GetMember("Välj medlem att ändra", members);

                // If m is null just break out of the method.
                if (m == null)
                {
                    return;
                }

                // Display a header from the RenderHeader method.
                Console.Clear();
                header = String.Format("      Ändra medlem, {0} {1}     ", m.FirstName, m.LastName);
                mView.RenderHeader(header);

                // Display a list of options for the user.
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

                    // Get the choice that the user made.
                    bool choice = int.TryParse(Console.ReadLine(), out index) && index >= 0 && index <= 4;


                    if (choice)
                    {
                        // Display a header from the RenderHeader method.
                        Console.Clear();
                        header = String.Format("      Ändrar medlem, {0} {1}     ", m.FirstName, m.LastName);
                        mView.RenderHeader(header);
                        Console.WriteLine();

                        if (index == 0)
	                    {
		                    exit = true;
                            continue;
	                    }
                        else if (index == 1)
	                    {
                            // Validate the firstName.
		                    firstName = ReadString(" Förnamn: ");

                            // Output a "Are you sure prompt?" so the user can think two times.
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("\n Är du säker på att du vill ändra '{0} {1}'s förnamn till '{2}'? [j/n]: ", m.FirstName, m.LastName, firstName);
                            Console.ResetColor();

                            // Read the key...
                            ConsoleKeyInfo inputKey = Console.ReadKey();

                            // ... and check if the key is either small or large "j".
                            if (inputKey.KeyChar == 'j' || inputKey.KeyChar == 'J')
                            {
                                // Set the FirstName to what the user specified.
                                m.FirstName = firstName;

                                // Save it to file.
                                SaveMembers(members);

                                // Display a header from the RenderHeader method.
                                Console.Clear();
                                mView.RenderHeader(" Medlem har ändrats", bgcolor: ConsoleColor.DarkGreen);

                                // After changes have been made just exit from this.
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
                            // Validate the lastName.
		                    lastName = ReadString(" Efternamn: ");

                            // Output a "Are you sure prompt?" so the user can think two times.
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("\n Är du säker på att du vill ändra '{0} {1}'s efternamn till '{2}'? [j/n]: ", m.FirstName, m.LastName, lastName);
                            Console.ResetColor();

                            // Read the key...
                            ConsoleKeyInfo inputKey = Console.ReadKey();

                            // ... and check if the key is either small or large "j".
                            if (inputKey.KeyChar == 'j' || inputKey.KeyChar == 'J')
                            {
                                // Set the LastName to what the user specified.
                                m.LastName = lastName;

                                // Save it to file.
                                SaveMembers(members);

                                // Display a header from the RenderHeader method.
                                Console.Clear();
                                mView.RenderHeader("         Medlem har ändrats       ", bgcolor: ConsoleColor.DarkGreen);

                                // After changes have been made just exit from this.
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
                            // Validate the phoneNum.
		                    phoneNum = ReadNum(" Telefonnummer: ");

                            // Output a "Are you sure prompt?" so the user can think two times.
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("\n Är du säker på att du vill ändra '{0} {1}'s telefonnummer till '{2}'? [j/n]: ", m.FirstName, m.LastName, phoneNum);
                            Console.ResetColor();

                            // Read the key...
                            ConsoleKeyInfo inputKey = Console.ReadKey();

                            // ... and check if the key is either small or large "j".
                            if (inputKey.KeyChar == 'j' || inputKey.KeyChar == 'J')
                            {
                                // Set the PhoneNum to what the user specified.
                                m.PhoneNum = phoneNum;

                                // Save it to file.
                                SaveMembers(members);

                                // Display a header from the RenderHeader method.
                                Console.Clear();
                                mView.RenderHeader("         Medlem har ändrats       ", bgcolor: ConsoleColor.DarkGreen);

                                // After changes have been made just exit from this.
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
                            // Validate all the input values.
		                    firstName = ReadString(" Förnamn: ");
                            lastName = ReadString(" Efternamn: ");
                            phoneNum = ReadNum(" Telefonnummer: ");

                            // Output a "Are you sure prompt?" so the user can think two times.
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("\n Är du säker på att du vill ändra '{0} {1}'s förnamn till '{2}', ", m.FirstName, m.LastName, firstName);
                            Console.Write("\n efternamn till '{0}' samt telefonnummer till '{1}'? [j/n]: ", lastName, phoneNum);
                            Console.ResetColor();

                            // Read the key...
                            ConsoleKeyInfo inputKey = Console.ReadKey();

                            // ... and check if the key is either small or large "j".
                            if (inputKey.KeyChar == 'j' || inputKey.KeyChar == 'J')
                            {
                                // Set all the values to what the user specified.
                                m.FirstName = firstName;
                                m.LastName = lastName;
                                m.PhoneNum = phoneNum;

                                // Save it to file.
                                SaveMembers(members);

                                // Display a header from the RenderHeader method.
                                Console.Clear();
                                mView.RenderHeader("         Medlem har ändrats       ", bgcolor: ConsoleColor.DarkGreen);
                                // After changes have been made just exit from this.

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
                // Display a header from the RenderHeader method.
                Console.Clear();
                mView.RenderHeader(" Det finns inga medlemmar att ändra", bgcolor: ConsoleColor.Yellow, fgcolor: ConsoleColor.Black);
            }
        }
        // Method to delete a member from the list of members.
        private static void DeleteMember(List<Member> members)
        {
            string header;
            MemberView mView = new MemberView();

            // Check if there's any members otherwise display a warning message.
            if (members != null && members.Any())
            {
                // Display a header from the RenderHeader method.
                Console.Clear();
                mView.RenderHeader("      Välj medlem att ta bort     ");

                // Get the member we want using the GetMember method.
                Member m = GetMember("Välj medlem att ta bort", members);

                // If m is null just break out of the method.
                if (m == null)
                {
                    return;
                }

                // Check so the user is really sure that's this is the right recipe to
                // if it is; delete it. else; just return to the menu again.
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("\n Är du säker på att du vill ta bort medlemmen: '{0} {1}' [j/n]: ", m.FirstName, m.LastName);
                Console.ResetColor();

                // Read the key...
                ConsoleKeyInfo inputKey = Console.ReadKey();

                // ... and check if the key is either small or large "j".
                if (inputKey.KeyChar == 'j' || inputKey.KeyChar == 'J')
                {
                    // Try to remove it otherwise display an error.
                    try
                    {
                        // Just remove it.
                        members.Remove(m);

                        // Save the file.
                        SaveMembers(members);

                        // Display a header from the RenderHeader method.
                        Console.Clear();
                        header = String.Format("      Medlemmen, {0} {1} har tagits bort    ", m.FirstName, m.LastName);
                        mView.RenderHeader(header, bgcolor: ConsoleColor.DarkGreen);
                    }
                    catch (Exception)
                    {
                        // Display a header from the RenderHeader method.
                        Console.Clear();
                        mView.RenderHeader(" FEL! Ett fel inträffade då medlemmen skulle tas bort.", bgcolor: ConsoleColor.Red);
                    }

                }
            }
            else
            {
                // Display a header from the RenderHeader method.
                Console.Clear();
                mView.RenderHeader(" Det finns inga medlemmar att ta bort", bgcolor: ConsoleColor.Yellow, fgcolor: ConsoleColor.Black);
            }
        }


        // Method to present an indexed list of all members name
        // and let's the user choose from these names and then returns it.
        private static Member GetMember(string header, List<Member> members)
        {
            int index;

            MemberView mView = new MemberView();

            do
            {
                // Display a header from the RenderHeader method.
                Console.Clear();
                mView.RenderHeader(header);

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

                // Get the choice that the user made.
                bool choice = int.TryParse(Console.ReadLine(), out index) && index >= 0 && index <= members.Count;

                if (choice)
                {
                    // If the index is greater than zero return the member we choosed.
                    if (index > 0)
                    {
                        return members[index - 1];
                    }
                    // Else return null.
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
            MemberView mView = new MemberView();

            // Check if there's any members otherwise display a warning message.
            if (members != null && members.Any())
            {
                if (!viewAll)
                {
                    Console.Clear();
                    mView.RenderHeader("       Välj medlem att visa       ");

                    // Get the member we want using the GetMember method.
                    Member m = GetMember("Välj medlem att visa", members);

                    // If m is null just break out of the method.
                    if (m == null)
                    {
                        return;
                    }

                    // Display a header from the RenderHeader method.
                    Console.Clear();
                    header = String.Format("      Visar medlem, {0} {1}      ", m.FirstName, m.LastName);
                    mView.RenderHeader(header);

                    // And render the user.
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
                // Display a header from the RenderHeader method.
                Console.Clear();
                mView.RenderHeader(" Det finns inga medlemmar att visa", bgcolor: ConsoleColor.Yellow, fgcolor: ConsoleColor.Black);
            }

        }

        // Private static method for displaying the Menu and read the choice.
        private static int GetMenuChoice()
        {
            int index;

            MemberView mView = new MemberView();

            do
            {
                // Output everything.
                mView.RenderHeader("          Medlemsregister         ");
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

                // Check the choice and return it.
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

        // Private static method for pressing any key to continue.
        private static void ContinueOnKeyPressed()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write("\n   Tryck tangent för att fortsätta   ");
            Console.ResetColor();
            
            // Hide the cursor.
            Console.CursorVisible = false;

            // Read the key.
            Console.ReadKey(true);
            Console.Clear();

            // Show the cursor.
            Console.CursorVisible = true;
        }
    }
}
