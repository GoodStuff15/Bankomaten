namespace Bankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Properties
            bool running = true;
            bool loggedin = false;

            // Containers

            string[] users = new string[] { "Göran", "Gunnar", "Gustav" };
            int[] pincodes = new int[] { 123, 666, 420 };

            // Welcome Greeting

            Console.WriteLine("Välkommen till Gustavs Bank-O-Matic!\n" +
                "Klicka på valfri knapp för att logga in!");
            Console.ReadKey(true);
            Console.Clear();

            // Calling the Login method, which takes users and pincodes as arguments.
            // It returns true if login is correct

            loggedin = LogIn(users, pincodes);
            Console.Clear();
            // if the login was correct, this while loop will run until we tell exit the program or log out.

            while (running && loggedin)
            {
                Console.WriteLine("Logged in!");
                Console.ReadKey();
                // Plats för övriga metoder
                running = false;
            }

            Console.WriteLine("Good bye :)");
            Console.ReadKey();
           
            

            

        }

        
    }
}
