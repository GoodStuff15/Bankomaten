namespace Bankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Properties

            bool running = true;
            bool loggedin = false;
            Console.Title = "Gustavs Bank-O-Matic";

            // Containers

            string[] users = new string[] { "Göran", "Gunnar", "Gustav", "Glenn", "Garbodor" };
            int[] pincodes = new int[] { 123, 666, 420, 808, 000 };


            // Welcome Greeting
            while(running)
            { 
            Console.WriteLine("Välkommen till Gustavs Bank-O-Matic!\n" +
                "Klicka på valfri knapp för att logga in!");
            Console.ReadKey(true);
            Console.Clear();

            // Calling the Login method, which takes users and pincodes as arguments.
            // It returns true if login is correct

            loggedin = LogIn(users, pincodes);
            Console.Clear();

            // if the login was correct, this while loop will run until we log out.

            while (loggedin)
            {
                Console.WriteLine("Login successful!");
                Console.ReadKey();

                MainMenu();

                loggedin = false;
                running = ExitProgram();
            }

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

        public static void MainMenu()
        {

            ConsoleKeyInfo cki;
            bool menuOn = true;

            while(menuOn)
            {

            Console.WriteLine("--- MENY ----\n");
            
            Console.WriteLine("1. Se dina konton och saldo");
            Console.WriteLine("2. Överföring mellan konton");
            Console.WriteLine("3. Ta ut pengar");
            Console.WriteLine("4. Logga ut");
            Console.WriteLine("\n Välj genom att trycka på motsvarande siffra på tangentbordet!");
            cki = Console.ReadKey(true);

            char choice = cki.KeyChar;

            switch(choice)
            {
                case '1':
                        // Se konto och saldo-funktion
                    break;
                case '2':
                        // Överföringsfunktion
                    break;
                case '3':
                        // Ta ut pengar-funktion
                    break;
                case '4':
                        menuOn = false;
                    break;
                default:
                    Console.WriteLine("\nThat menu item does not exist! Try again:\n");
                    break;

            }

            }
        }


        public static bool ExitProgram()
        {
            ConsoleKeyInfo yn;

            while (true)
            {
                Console.WriteLine("Do you want to exit the program? Press Y for yes or N for no:");

            yn = Console.ReadKey(true);
            char yOrN = yn.KeyChar;
            yOrN = Char.ToUpper(yOrN);
       
            switch (yOrN)
            {
                case 'Y':
                        Console.Clear();
                        return false;
                    
                case 'N':
                        Console.Clear();
                        return true;
                   
                default:
                    Console.WriteLine("Invalid button press, try again!");
                        Console.Clear();
                    break;
            }
            }
        }

    }
}
