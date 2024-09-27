using System.Globalization;

namespace Bankomaten
{
    internal class Program
    {
        // Properties

        static bool running = true;
        static bool loggedin = false;

        static int user1_ID = 0;
        static int user2_ID = 1;
        static int user3_ID = 2;
        static int user4_ID = 3;
        static int user5_ID = 4;

        static int activeUserID;

        static string[] accountTypes = new string[] { "Lönekonto", "Sparkonto", "Investeringssparkonto", "Aktiekonto", "Magic the Gathering-konto" };

        // Containers

        static int[] userIDs = new int[] { 0, 1, 2, 3, 4, 5 };
        static string[] users = new string[] { "Göran", "Gunnar", "Gustav", "Glenn", "Garbodor" };
        static int[] pincodes = new int[] { 123, 666, 420, 808, 000 };

        static int[] userAccountCount = new int[5] { 1, 2, 3, 4, 5 };

        static double[][] userSaldos =
            [

            [0,0,0,0,0],
            [0,0,0,0,0],
            [0,0,0,0,0],
            [0,0,0,0,0],
            [0,0,0,0,0]

            ];



        static void Main(string[] args)
        {

            Console.Title = "Gustavs Bank-O-Matic";
            GenerateAccounts();




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

        static void GenerateAccounts()
        {

            Random r = new Random();
            for(int i = 0; i < users.Length; i++ )
            {

                for(int j = 0; j < userAccountCount[i]; j++)
                {
                    userSaldos[i][j] = (double)r.Next(1, 1000001);
                }

            }

            // Displaying for testing

/*            for (int i = 0; i < users.Length; i++)
            {
                Console.WriteLine($"User: {users[i]}");
                Console.WriteLine($"Pin: {pincodes[i]}");
                Console.WriteLine($"User accounts: {userAccountCount[i]}");

                for (int j = 0; j < userAccountCount[i]; j++)
                {
                    Console.WriteLine($"Account: {accountTypes[j]} Money: {userSaldos[i][j]}");
                }

            }*/
        }

        static void ViewAccounts(int id)
        {
            Console.Clear();
            ConsoleKeyInfo cki;
            while(true)
            {

            
            string divider = "-------------------------|--------------------";
            Console.WriteLine("Dina konton:\n");
            Console.WriteLine($"{"Konto", -25}|{"Saldo", 20}");
            Console.WriteLine(divider);

            for(int i = 0; i < userAccountCount[id]; i++)
            {
                Console.WriteLine($"{accountTypes[i], -25}|{userSaldos[id][i].ToString("C", CultureInfo.CurrentCulture),20}");
                Console.WriteLine(divider);
            }
            Console.WriteLine("\n Klicka enter för att komma tillbaka till huvudmenyn!\n");
                cki = Console.ReadKey(true);

                if(cki.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\nDu måste trycka på enter brorsan! Prova igen!\n");
                }
            }


        }

        static bool LogIn(string[] users, int[] pins)
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
                            activeUserID = Array.IndexOf(users, userName); 
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

        static void MainMenu()
        {
            
            ConsoleKeyInfo cki;
            bool menuOn = true;

            while(menuOn)
            {
            Console.Clear();

            Console.WriteLine("--- MENY ----\n");
            
            Console.WriteLine("1. Se dina konton och saldo");
            Console.WriteLine("2. Överföring mellan konton");
            Console.WriteLine("3. Ta ut pengar");
            Console.WriteLine("4. Logga ut");
            Console.WriteLine("\n Gör ditt val genom att trycka på motsvarande siffra på tangentbordet!\n");

            cki = Console.ReadKey(true);

            char choice = cki.KeyChar;

            switch(choice)
            {
                case '1':
                        ViewAccounts(activeUserID);
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


        static bool ExitProgram()
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
