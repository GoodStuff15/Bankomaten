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
            // Setting up
            Console.Title = "Gustavs Bank-O-Matic";
            GenerateAccounts();

            // Welcome Greeting
            while(running)
            { 
            Console.WriteLine("Välkommen till Gustavs Bank-O-Matic!\n" +
                "Klicka på valfri knapp för att logga in!");
            Console.ReadKey(true);
            Console.Clear();

            // Calling the Login function, which takes 'users' and 'pincodes' arrays as arguments.
            // It returns true if login is correct
            loggedin = LogIn(users, pincodes);
            Console.Clear();

            // if the login was correct, this while loop will run until we log out.

            while (loggedin)
            {
                Console.WriteLine("Login successful!");
                Console.ReadKey();

                MainMenu();

                    // if we choose to log out from the main menu,
                    // loggedin status is set to false, and the program
                    // asks if we want to exit. If we don't, we return
                    // to the login screen.

                loggedin = false;
                running = ExitProgram();
            }

            }
            Console.WriteLine("Good bye :)");
            Console.ReadKey();
           
            

            

        }

        // Puts a random amount of money in the accounts the respective users have access to.
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


        }

        static void WithdrawFunds(int id)
        {
            bool withdrawing = true;
            int from;
            int amount = 0;
            ConsoleKeyInfo cki;


            while(withdrawing)
            {
                AccountDisplay(id);
                Console.WriteLine("\nVälj ett konto att ta ut pengar ifrån (tryck på motsvarande siffra på tangentbordet): ");
                cki = Console.ReadKey(true);
                from = Convert.ToInt32(cki.KeyChar.ToString());

                if (userAccountCount[id] > from)
                {
                    Console.WriteLine($"Du valde {accountTypes[from]}\n");
                    Console.WriteLine("Skriv in summa: ");
                    amount = int.Parse(Console.ReadLine());

                    if (amount > userSaldos[id][from])
                    {
                        Console.WriteLine("\nDet finns inte tillräckligt med pengar på kontot! ");

                        Console.WriteLine("\n Klicka enter för att komma tillbaka till huvudmenyn,\n" +
                            "eller på valfri knapp för att prova igen.");

                        cki = Console.ReadKey(true);

                        withdrawing = ReturnToMain(cki);
                    }
                    else
                    {
                        // Removes the amount from the sending account
                        // Then prints new balance, and the amount withdrawn

                        userSaldos[id][from] -= amount;

                        Console.WriteLine($"Nytt saldo på {accountTypes[from]}: {userSaldos[id][from].ToString("C", CultureInfo.CurrentCulture)}");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine($"Ditt uttag: {amount.ToString("C", CultureInfo.CurrentCulture)}");
                        Console.ResetColor();

                        Console.WriteLine("\n Klicka enter för att komma tillbaka till huvudmenyn,\n" +
                            "eller valfri knapp för att göra ett nytt uttag.");
                        cki = Console.ReadKey(true);

                        withdrawing = ReturnToMain(cki);
                        

                    }


                }

            }
        }
        static void TransferFunds(int id)
        {

            // Properties used in this function

            bool transfering = true;
            int from;
            int to;
            int amount = 0;
            ConsoleKeyInfo cki;

            

            while (transfering)
            {
                // Viewing accounts (useful for us short term memory-challenged)
                AccountDisplay(id);

                Console.WriteLine("\nVälj ett konto att föra över pengar ifrån (tryck på motsvarande siffra på tangentbordet): ");
                cki = Console.ReadKey(true);
                from = Convert.ToInt32(cki.KeyChar.ToString());

                // Checks if the from account exists
                // Restarts the function if not

                if (userAccountCount[id] > from)
                {
                    Console.WriteLine($"Du valde {accountTypes[from]}\n");
                    Console.WriteLine("Välj mottagarkonto: ");
                    cki = Console.ReadKey(true);
                    to = Convert.ToInt32(cki.KeyChar.ToString());

                    // Checks if the 'to' account exists and is not the same as the 'from' account
                    // Restarts the function if not

                    if (userAccountCount[id] > to && to != from)
                    {
                        Console.WriteLine($"Du valde {accountTypes[to]}\n");
                        Console.WriteLine("Skriv in summa: ");
                        amount = int.Parse(Console.ReadLine());

                        // Checks if there is enough money on the from account
                        // Restarts the function if not

                        if (amount > userSaldos[id][from])
                        {
                            Console.WriteLine("\nDet finns inte tillräckligt med pengar på kontot! ");

                            Console.WriteLine("\n Klicka enter för att komma tillbaka till huvudmenyn,\n" +
                                "eller på valfri knapp för att prova igen.");
                            cki = Console.ReadKey(true);

                            transfering = ReturnToMain(cki);
                        }
                        else
                        {
                            // Removes the amount from the sending account
                            // Adds it to the recieving account
                            // Then prints new saldos, and the amount removed/added

                            userSaldos[id][from] -= amount;
                            userSaldos[id][to] += amount;

                            Console.WriteLine("\nNya saldon:");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{accountTypes[from]}: {userSaldos[id][from].ToString("C", CultureInfo.CurrentCulture)} (- {amount.ToString("C", CultureInfo.CurrentCulture)})");
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine($"{accountTypes[to]}: {userSaldos[id][to].ToString("C", CultureInfo.CurrentCulture)} (+ {amount.ToString("C", CultureInfo.CurrentCulture)})");
                            Console.ResetColor();

                            Console.WriteLine("\n Klicka enter för att komma tillbaka till huvudmenyn,\n" +
                                "eller valfri knapp för att göra en ny överföring.");
                            cki = Console.ReadKey(true);

                            transfering = ReturnToMain(cki);
                            Console.Clear();

                        }


                    } // Prints a message and restarts if 'from' and 'to' account are the same
                    else if(to == from)
                    {
                        Console.WriteLine("Till- och frånkonto kan inte vara samma! Försök igen!\n");

                        Console.WriteLine("\n Klicka enter för att komma tillbaka till huvudmenyn,\n" +
                        "eller på valfri knapp för att prova igen.");
                        cki = Console.ReadKey(true);

                        transfering = ReturnToMain(cki);
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Ogiltigt konto, försök igen!\n");

                        Console.WriteLine("\n Klicka enter för att komma tillbaka till huvudmenyn,\n" +
                        "eller på valfri knapp för att prova igen.");
                        cki = Console.ReadKey(true);

                        transfering = ReturnToMain(cki);
                        Console.Clear();
                    }

                }
                else
                {
                    Console.WriteLine("Ogiltigt konto, försök igen!");
                }



                
            }
        }
        
        // Viewing Accounts "shell", putting the actual display in another method
        // allows the program to show it in other methods without it asking to
        // return to the main menu at inopportune times :) 
        static void ViewAccounts(int id)
        {
            Console.Clear();
            ConsoleKeyInfo cki;
            bool viewing = true;
            while(viewing)
            {
                AccountDisplay(id);

                Console.WriteLine("\n Klicka enter för att komma tillbaka till huvudmenyn!\n");
                cki = Console.ReadKey(true);

                viewing = ReturnToMain(cki);
            }

        }

        // The actual displaying of the accounts and information is in this method
        // So it can be accessed from Transfer- and Withdrawal methods.
        static void AccountDisplay(int id)
        {

            string divider = "   -------------------------|--------------------";
            Console.WriteLine("Dina konton:\n");
            Console.WriteLine($"{"   Konto",-25}{"Saldo",24}");
            Console.WriteLine(divider);

            for (int i = 0; i < userAccountCount[id]; i++)
            {
                Console.WriteLine($"{i}. {accountTypes[i],-25}|{userSaldos[id][i].ToString("C", CultureInfo.CurrentCulture),20}");
                Console.WriteLine(divider);
            }
        }

        // A method that returns true if the user wants to
        // remain in the current section of the program.
        // Returns true if they choose to return to main menu
        static bool ReturnToMain(ConsoleKeyInfo click)
        {
            if (click.Key == ConsoleKey.Enter)
            {
                return false;
            }
            else
            {
                Console.Clear();
                return true;
            }
        }

        // Login function
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
                    // After that, the LogIn function returns false.

                    while (!pinCorrect && pinTries < maxPinTries)
                    {

                        Console.WriteLine("Lösenord: ");
                        pinEntry = Convert.ToInt32(Console.ReadLine());

                        // If statement checks if the pin entered matches the user name
                        // In this case by looking if they have the same index in their respective arrays
                        // If true, the method returns true and the user is logged in!
                        // It then sets the activeUserID to the users ID, so we can find their
                        // information in the container arrays.
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

        // MainMenu function that loops while user is logged in.
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
                        TransferFunds(activeUserID);
                    break;
                case '3':
                        WithdrawFunds(activeUserID);
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

        // Function that returns false if the user wants to exit the program,
        // and vice versa.

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
