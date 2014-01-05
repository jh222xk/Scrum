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
                        members = LoadMembers();
                        break;
                    case 2:
                        Console.WriteLine("");
                        break;
                    case 3:
                        Console.WriteLine("");
                        break;
                    case 4:
                        Console.WriteLine("");
                        break;
                    case 5:
                        Console.WriteLine("");
                        break;
                    case 6:
                        Console.WriteLine("");
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
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" ╔═══════════════════════════════════╗ ");
                Console.WriteLine(" ║      Medlemmarna har lästs in     ║ ");
                Console.WriteLine(" ╚═══════════════════════════════════╝ ");
                Console.BackgroundColor = ConsoleColor.Black;
                return repo.Load();
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
                Console.WriteLine(" 3. Ändra medlem.");
                Console.WriteLine(" 4. Ta bort medlem.");
                Console.WriteLine("\n - Lista ------------------------------------\n");
                Console.WriteLine(" 5. Lista en medlem.");
                Console.WriteLine(" 6. Lista alla medlemmar.");
                Console.WriteLine("\n ═══════════════════════════════════════════\n");
                Console.Write(" Ange menyval [0-6]: ");
                Console.ResetColor();

                if (int.TryParse(Console.ReadLine(), out index) && index >= 0 && index <= 6)
                {
                    return index;
                }

                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n FEL! Ange ett nummer mellan 0 och 6.\n");
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
