﻿using System.Globalization;

namespace Bankomaten
{
    internal class Program
    {
        // Properties
        static bool running = true;
        static bool loggedin = false;
        static bool failedLogIn = false;
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

            // Testing Space


            // Welcome Greeting
            while (running)
            {


                // Calling the Login function, which takes 'users' and 'pincodes' arrays as arguments.
                // It returns true if login is correct
                if (failedLogIn)
                {
                    Console.WriteLine("Du måste starta om programmet för att försöka logga in igen!\n");
                    Console.WriteLine("Tryck på valfri knapp för att avsluta.");
                    Console.ReadKey();
                    running = false;
                }
                else
                {
                    Console.WriteLine("Välkommen till Gustavs Bank-O-Matic!\n" +
                    "Klicka på valfri knapp för att logga in!");
                    Console.ReadKey(true);
                    Console.Clear();

                    loggedin = LogIn(users, pincodes);
                    Console.Clear();
                }

                // if the login was correct, this while loop will run until we log out.
                while (loggedin)
                {
                    Console.WriteLine("Login lyckades!\n");

                    MainMenu();

                    // if we choose to log out from the main menu,
                    // loggedin status is set to false, and the program
                    // asks if we want to exit. If we don't, we return
                    // to the login screen.
                    loggedin = false;
                    running = ExitProgram();
                }

            }
            Console.WriteLine("Tack för ditt bidrag till våra aktieägare!");            
        }

        // Puts a random amount of money in the accounts the respective users have access to.
        static void GenerateAccounts()
        {

            Random r = new Random();
            for (int i = 0; i < users.Length; i++)
            {
                for (int j = 0; j < userAccountCount[i]; j++)
                {
                    userSaldos[i][j] = (double)r.Next(1, 1000001);
                }
            }
        }

        static void WithdrawFunds(int id)
        {
            bool withdrawing = true;
            int from;
            double amount = 0;

            while (withdrawing)
            {
                AccountDisplay(id);
                Console.WriteLine("\nVälj ett konto att ta ut pengar ifrån (tryck på motsvarande siffra på tangentbordet): ");

                from = Choice(id);

                if (userAccountCount[id] > from)
                {
                    Console.WriteLine($"Du valde {accountTypes[from]}\n");
                    Console.WriteLine("Skriv in summa: ");
                    amount = NumberInput(true);

                    if (amount > userSaldos[id][from])
                    {
                        Console.Beep();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nDet finns inte tillräckligt med pengar på kontot! ");
                        Console.ResetColor();
                        withdrawing = ReturnToMain("Klicka på valfri knapp för att prova igen.");
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

                        withdrawing = ReturnToMain("Klicka på valfri knapp för att göra ett nytt uttag.");
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
            double amount = 0;
            ConsoleKeyInfo cki;

            while (transfering)
            {
                // Viewing accounts (useful for us short term memory-challenged)
                AccountDisplay(id);

                Console.WriteLine("\nVälj ett konto att föra över pengar ifrån (tryck på motsvarande siffra på tangentbordet): ");
                from = Choice(id);

                // Checks if the from account exists
                // Restarts the function if not
                if (userAccountCount[id] > from)
                {
                    Console.WriteLine($"Du valde {accountTypes[from]}\n");
                    Console.WriteLine("Välj mottagarkonto: ");
                    cki = Console.ReadKey(true);
                    to = Choice(id);

                    // Checks if the 'to' account exists and is not the same as the 'from' account
                    // Restarts the function if not
                    if (userAccountCount[id] > to && to != from)
                    {
                        Console.WriteLine($"Du valde {accountTypes[to]}\n");
                        Console.WriteLine("Skriv in summa: ");
                        amount = NumberInput(true);

                        // Checks if there is enough money on the from account
                        // Restarts the function if not
                        if (amount > userSaldos[id][from])
                        {
                            Console.WriteLine("\nDet finns inte tillräckligt med pengar på kontot!");

                            transfering = ReturnToMain("Klicka på Valfri knapp för att prova igen.");
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

                            transfering = ReturnToMain("Klicka på valfri knapp för att göra en ny överföring.");
                            Console.Clear();                   
                        }

                    } // Prints a message and restarts if 'from' and 'to' account are the same
                    else if (to == from)
                    {
                        Console.WriteLine("Till- och frånkonto kan inte vara samma! \n");

                        transfering = ReturnToMain("Klicka på valfri knapp för att prova igen!");
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Ogiltigt konto!\n");

                        transfering = ReturnToMain("Klicka på valfri knapp för att prova igen!");
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

            bool viewing = true;
            while (viewing)
            {
                AccountDisplay(id);
                viewing = ReturnToMain("");
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
        static bool ReturnToMain(string or)
        {
            Console.WriteLine($"* Klicka Enter för att komma tillbaka till huvudmenyn. \n* {or}");
            ConsoleKeyInfo cki = Console.ReadKey();

            if (cki.Key == ConsoleKey.Enter)
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
            string userName ="";

            // Maximum number of PIN entries is 3
            int pinTries = 0;
            int maxPinTries = 3;

            // Two bools that are used to control while loops below
            bool userCorrect = false;
            bool pinCorrect = false;

            while (!userCorrect)
            {
                Console.WriteLine("Användarnamn: ");

                   
                userName = Console.ReadLine();


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

                        Console.WriteLine("PIN-kod: ");
                        pinEntry = (int)NumberInput(false);

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
            // sets failed login state, upon return to start screen will prompt
            // a reboot of program
            failedLogIn = true;
            return false;
        }

        // MainMenu function that loops while user is logged in.
        static void MainMenu()
        {

            ConsoleKeyInfo cki;
            bool menuOn = true;

            while (menuOn)
            {
                Console.Clear();

                Console.WriteLine("--- MENY ----\n");

                Console.WriteLine("1. Se dina konton och saldo");
                Console.WriteLine("2. Överföring mellan konton");
                Console.WriteLine("3. Ta ut pengar");
                Console.WriteLine("4. Logga ut");
                Console.WriteLine("\n Gör ditt val genom att trycka på motsvarande siffra på tangentbordet!\n");

                cki = Console.ReadKey(true);
                // converts keypress to char (simplest way to enable both numpad
                // and number key presses)
                char choice = cki.KeyChar;

                switch (choice)
                {
                    case '1':
                        ViewAccounts(activeUserID);
                        break;
                    case '2':
                        TransferFundsUpdate(activeUserID);
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
                Console.WriteLine("Vill du avsluta programmet? Tryck Y för ja eller N om du vill logga in igen:");

                yn = Console.ReadKey(true);

                switch (yn.Key)
                {
                    case ConsoleKey.Y:
                        Console.Clear();
                        return false;

                    case ConsoleKey.N:
                        Console.Clear();
                        return true;

                    default:
                        Console.WriteLine("Ogiltigt val, prova med Y eller N!");
                        Console.Clear();
                        break;
                }
            }
        }

        // A function that takes input from user, makes sure its in number form,
        // and then returns it. Takes a bool as argument to determine whether to
        // return in currency form (double including ören) or regular number form.
        static double NumberInput(bool money)
        {
            bool isNumber = false;
           
            string numberString = "";
            double number;
            
            ConsoleKeyInfo cki;

            do
            {   // Takes input and checks if it is a number
                cki = Console.ReadKey(true);
                isNumber = double.TryParse(cki.KeyChar.ToString(), out double check);

                if (cki.Key != ConsoleKey.Backspace)
                {
                    if (isNumber) //if it is a number, adds to string of numbers 
                    {
                        // Makes sure user can only type two digits after entering a comma
                        if(numberString.Contains(",") && numberString.IndexOf(",") < numberString.Length -2)
                        {
                        }
                        else
                        {
                        numberString += cki.KeyChar;
                        Console.Write(cki.KeyChar);
                        }
                    }
                    else if(money && !numberString.Contains(",") && cki.Key == ConsoleKey.OemComma)
                    { // if key pressed is comma and there is no comma in string, adds a comma.
                        numberString += cki.KeyChar;
                        Console.Write(cki.KeyChar);
                    }
                    
                } // Backspace key delete characters from the string
                else if (cki.Key == ConsoleKey.Backspace && numberString.Length > 0)
                {
                    numberString = numberString.Substring(0, (numberString.Length - 1));
                    Console.Write("\b \b");
                }
                // Exits while loop when user presses enter after entering at least 1 number
            } while (cki.Key != ConsoleKey.Enter || numberString.Length == 0);

            Console.WriteLine();

            // The string is converted to double and returned.
            // CurrentCulture used to make sure Ören are represented correctly.
            double.TryParse(numberString, CultureInfo.CurrentCulture, out number);
           
            return number;
        }

        // A function that takes user id as input and returns an
        // valid account "id" number, used in withdrawal and transactions.
        static int Choice(int id)
        {
            int max = userAccountCount[id] - 1;
            bool isNumber;
            int num = 0;
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                // Checks if the choice is a number
                // If it is, converts keypress
                // into an int. 
                isNumber = Char.IsAsciiDigit(key.KeyChar);
                if(isNumber)
                {
                num = Convert.ToInt32(key.KeyChar.ToString());
                }

            } while (!isNumber || num > max); // exits while loop only if number is not bigger
                                              // than the max index of the active users accounts.
            return num;
        }

        static void TransferFundsUpdate(int id)
        {
            int from;
            int to;
            double amount;
            bool transfering = true;

            while (transfering)
            {
                AccountDisplay(id);

                Console.WriteLine("Skriv in från-konto: ");
                from = Choice(id);
                Console.WriteLine($"Från: {accountTypes[from]}. Skriv in till-konto:");
                to = Choice(id);

                if (from == to)
                {
                    Console.Beep();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nFrån- och Till- konto kan inte vara samma!\n");
                    Console.ResetColor();
                    transfering = ReturnToMain("Tryck på valfri annan knapp för att prova igen!");
                }
                else
                {
                    Console.WriteLine($"Till: {accountTypes[to]}. Skriv in summa som ska överföras:");
                    amount = NumberInput(true);

                    if (amount > userSaldos[id][from])
                    {
                        Console.Beep();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\nDet finns inte tillräckligt saldo på {accountTypes[from]}!\n");
                        Console.ResetColor();
                        transfering = ReturnToMain("Tryck på valfri annan knapp för att prova igen!");
                    }
                    else
                    {

                        userSaldos[id][from] -= amount;
                        userSaldos[id][to] += amount;

                        Console.WriteLine($"{accountTypes[from]}: {userSaldos[id][from].ToString("C", CultureInfo.CurrentCulture)} (-{amount.ToString("C", CultureInfo.CurrentCulture)})");
                        Console.WriteLine($"{accountTypes[to]}: {userSaldos[id][to].ToString("C", CultureInfo.CurrentCulture)} (+{amount.ToString("C", CultureInfo.CurrentCulture)})\n");

                        transfering = ReturnToMain("Tryck på valfri annan knapp för att göra en ny överföring.");
                        Console.ReadKey();
                    }
                }
            }
        }

    }
}
