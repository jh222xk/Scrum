using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlemsregister
{
    class MemberView
    {
        // Method to render the list of recipes.
        public void Render(List<Member> members)
        {
            // Iterate through the recipes list.
            foreach (var member in members)
            {
                Render(member);
            }
        }

        // Method to render a recipe.
        public void Render(Member member)
        {
            Console.WriteLine();
            Console.WriteLine(" AnvändarID: {0}", member.UserID);
            Console.WriteLine(" Namn: {0} {1}", member.FirstName, member.LastName);
            Console.WriteLine(" Telefonnummer: {0}", member.PhoneNum);
            Console.WriteLine("\n ═══════════════════════════════════════════\n");
        }
    }
}
