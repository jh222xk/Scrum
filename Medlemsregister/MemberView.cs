using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlemsregister
{
    class MemberView
    {
        // Method to render the list of members.
        public void Render(List<Member> members)
        {
            MemberView mView = new MemberView();

            mView.RenderHeader("          Visar medlemmar         ");

            // Iterate through the members list.
            foreach (var member in members)
            {
                Render(member);
            }
        }

        // Method to render a member.
        public void Render(Member member)
        {
            Console.WriteLine();
            Console.WriteLine(" AnvändarID: {0}", member.UserID);
            Console.WriteLine(" Namn: {0} {1}", member.FirstName, member.LastName);
            Console.WriteLine(" Telefonnummer: {0}", member.PhoneNum);
            Console.WriteLine();
            MemberView.HorizontalLine(39);
            Console.WriteLine();
        }

        // Method to render a horizontal line.
        public static void HorizontalLine(int length)
        {
            for (int i = 0; i < length; i++)
            {
                Console.Write("═");
            }
        }

        // Method to render the header with a text, background and foreground color.
        public void RenderHeader(string header, ConsoleColor bgcolor = ConsoleColor.DarkCyan, ConsoleColor fgcolor = ConsoleColor.White)
        {
            Console.BackgroundColor = bgcolor;
            Console.ForegroundColor = fgcolor;
            Console.Write(" ╔");
            HorizontalLine(header.Length + 1);
            Console.Write("╗ ");
            Console.Write("\n{0}{1}{0} ", " ║", header);
            Console.Write("\n ╚");
            HorizontalLine(header.Length + 1);
            Console.Write("╝ \n");
            Console.ResetColor();
        }
    }
}
