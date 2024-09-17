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

        public static bool LogIn(string[] users, int[] pins)
        {
            Console.WriteLine("Vänligen fyll i användarnamn och PIN för att logga in: ");

            // Properties

            int pinEntry;

            // Maximum number of PIN entries is 3
            int pinTries = 0;
            int maxPinTries = 3;

            // Two bools that are used to control while loops below
            bool userCorrect = false;
            bool pinCorrect = false;

            while (!userCorrect)
            {
                Console.WriteLine("Användarnamn: ");
                string userName = Console.ReadLine();

                // if statement checks if the entered userName exists in the "database"
                // (the username array we passed to the method)
                // If it doesen't exist: It continues until user enters a correct user name.

                if (users.Contains(userName))
                {

                    userCorrect = true;

                    // User enters a pin code
                    // This while loops runs a number of times equal to number of max tries.
                    // After that, the method returns false.

                    while (!pinCorrect && pinTries < maxPinTries)
                    {

                        Console.WriteLine("Lösenord: ");
                        pinEntry = Convert.ToInt32(Console.ReadLine());

                        // If statement checks if the pin entered matches the user name
                        // In this case by looking if they have the same index in their respective arrays
                        // If true, the method returns true and the user is logged in!
                        // If false, pinTries is incremented, and the user is notified of
                        // the number of tries they have left.

                        if (Array.IndexOf(users, userName) == Array.IndexOf(pins, pinEntry))
                        {
                            pinCorrect = true;
                            return true;
                        }
                        else
                        {
                            pinTries++;

                            Console.WriteLine($"Sorry! Wrong PIN. {maxPinTries - pinTries} tries left.");
                        }

                    }


                }
                else
                {
                    Console.WriteLine("Den användaren finns inte, försök igen!");
                }

            }

            return false;
        }
    }
}
