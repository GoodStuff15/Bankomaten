// Gustav Eriksson Söderlund, SUT24

using System.Runtime.InteropServices; // For external functionality
using System.Globalization;           // For displaying Ören
using System.Media;                   // To play sounds on Windows



namespace Bankomaten
{
    internal class Program
    {
        // Properties
        static bool running = true;
        static bool loggedin = false;
        static bool failedLogIn = false;
        static int activeUserID;
        static SoundPlayer cash = new SoundPlayer();

        static string[] accountTypes = new string[] { "Lönekonto", "Sparkonto", "Investeringssparkonto", "Aktiekonto", "Magic the Gathering-konto", "International Account" };
        static string[] accountNames = new string[accountTypes.Length];
        static string[] accountCurrency = new string[] { "SEK", "SEK", "SEK", "SEK", "SEK", "SEK" };

        // Containers
        static int[] userIDs = new int[] { 0, 1, 2, 3, 4, 5 };
        static string[] users = new string[] { "Göran", "Gunnar", "Gustav", "Glenn", "Garbodor" };
        static int[] pincodes = new int[] { 123, 666, 420, 808, 000 };
        static int[] userAccountCount = new int[] { 1, 2, 3, 4, 5 };

        static string[] currencyNames = new string[] { "SEK", "Euro", "Dollar", "Pund" };
        static decimal[] conversionRates = new decimal[] { 1m, 11.34m, 10.34m, 13.55m };
        static char[] currencySymbol = new char[] { 'k', '€', '$', '£' };

        static decimal[][] userSaldos = new decimal[users.Length][];

        static void Main(string[] args)
        {
            // Setting up
            Console.Title = "Gustavs Bank-O-Matic";
            GenerateAccounts();
            Console.OutputEncoding = System.Text.Encoding.UTF8; // For €
            
            // Welcome Greeting
            while (running)
            {
                // Calling the Login function: if a failed login is detected, program must be restarted
                if (failedLogIn)
                {
                    Console.WriteLine("Du måste starta om programmet för att försöka logga in igen!\n");
                    Console.WriteLine("Tryck på valfri knapp för att avsluta.");
                    Console.ReadKey();
                    running = false;
                }
                else if (!loggedin)
                {
                    Console.WriteLine("Välkommen till Gustavs Bank-O-Matic!\n" +
                    "Klicka på valfri knapp för att logga in!");
                    Console.ReadKey(true);
                    Console.Clear();

                    loggedin = LogIn();
                    Console.Clear();
                }
                else
                {
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

        // Puts a random amount of money in the accounts the respective users have access to,
        // and saves information to text files.
        // Only generates new amounts if respective save files don't exist
        static void GenerateAccounts()
        {
            Random r = new Random();

            for (int i = 0; i < users.Length; i++)
            {
                if (!File.Exists($"user{userIDs[i]}.txt"))
                {
                    decimal[] temp = new decimal[users.Length];

                    for (int j = 0; j < userAccountCount[i]; j++)
                    {
                        temp[j] = (decimal)r.Next(1, 1000001);
                        accountNames[j] = accountTypes[j];
                    }

                    userSaldos[i] = temp;
                    Save(i);
                }
            }
        }

        // Plays a little animation with sound when user withdraws money
        static void PrintMoney(char v)
        {
            cash.SoundLocation = "cash.wav";
            string[][] dollabill = // Graphics
                [
                ["_", "___", "___", "___", "___", "___", "_"],
                ["|", "100", " SE", "K   ", "uuu", "   ", "|"],
                ["|", "   ", "   ", "  (", "o-o", ")  ", "|"],
                ["|", "   ", "   ", "  /", "'-'", "|  ", "|"],
                ["|", "___", "___", "___", "___", "___", "|"],
                ["=", "===","===", "===", "===", "===", " "]
                ];

            string[][] printed = new string[6][];
            string[] emptyRow = [" ", " ", " ", " ", " ", " ", " "];

            int max = 5;
            int printCheck = 0;

            for (int i = 0; i < 6; i++)
            {
                printed[i] = emptyRow; // fill the jagged array so it's easier to print to
            }

            Console.ForegroundColor = ConsoleColor.DarkGreen;

            for (int i = 0; i < 6 && i >= 0; i++)
            {

                printed[0] = dollabill[max];
                // The display (6x7) 
                for (int j = 0; j < 7; j++)
                {
                    Console.Write($"{printed[0][j]}");
                }
                Console.WriteLine();
                for (int j = 0; j < 7; j++)
                {
                    Console.Write($"{printed[1][j]}");
                }
                Console.WriteLine();
                for (int j = 0; j < 7; j++)
                {
                    Console.Write($"{printed[2][j]}");
                }
                Console.WriteLine();
                for (int j = 0; j < 7; j++)
                {
                    Console.Write($"{printed[3][j]}");
                }
                Console.WriteLine();
                for (int j = 0; j < 7; j++)
                {
                    Console.Write($"{printed[4][j]}");
                }
                Console.WriteLine();
                for (int j = 0; j < 7; j++)
                {
                    Console.Write($"{printed[5][j]}");
                }
                Console.WriteLine();
                Thread.Sleep(400);
                cash.Play();

                // Checks how many times the display has been printed
                // Then prints in correct order
                if (printCheck == 5)
                {
                    // A little surprise for the user!
                    mciSendStringA("open " + "F" + ": type CDaudio alias drive" + "D",
                        "return", 0, 0);
                    mciSendStringA("set drive" + "F" + " door open", "return", 0, 0);

                    Thread.Sleep(2500);
                }
                if (printCheck >= 4)
                {
                    printed[5] = printed[4];
                }
                if (printCheck >= 3)
                {
                    printed[4] = printed[3];
                    printed[3] = emptyRow;

                }
                if (printCheck >= 2)
                {
                    printed[3] = printed[2];
                    printed[2] = emptyRow;
                }
                if (printCheck >= 1)
                {
                    printed[2] = printed[1];
                    printed[1] = emptyRow;
                }
                if (printCheck >= 0)
                {
                    printed[1] = printed[0];
                    printed[0] = emptyRow;
                }

                if (max > 0)
                {
                    max--;
                }

                printCheck++;

                Console.Clear();
            }

            Console.ResetColor();
        }

        // Saves current user details
        static void Save(int id)
        {
            string name = $"Username: {users[id]}";
            string pin = $"PIN: {pincodes[id]}";
            string ID = $"ID: {userIDs[id]}";

            File.WriteAllText($"user{userIDs[id]}.txt", $"{ID}\n{name}\n{pin}\nAccounts\n"); // writes static information first

            for (int j = 0; j < userAccountCount[id]; j++) // then writes dynamic information (eg: there can be any number of accounts)
            {
                File.AppendAllText($"user{userIDs[id]}.txt", $"{accountNames[j]}:{userSaldos[id][j]}:{accountCurrency[j]}\n");
            }
        }

        // Loads current user details
        static void Load(int id)
        {
            string[] open = File.ReadAllLines($"user{userIDs[id]}.txt"); // reads the file, puts each line as a string
                                                                         // in the array
            int accountRow = 0;

            for (int i = 0; i < open.Length; i++) // Finding the row in the file where account info starts
            {
                if (open[i] == "Accounts")
                {
                    accountRow = i + 1;
                }
            }

            decimal[] temp = new decimal[open.Length - accountRow]; // temporary array of users accounts
            int x = 0; // used as index to load accounts into correct position in arrays

            for (int i = accountRow; i < open.Length; i++)
            {
                int amountLength = open[i].LastIndexOf(":") - open[i].IndexOf(":") - 1;
                string stringAmount = open[i].Substring(open[i].IndexOf(":") + 1, amountLength); // gets numbers in file
                decimal amount = Convert.ToDecimal(stringAmount);                               // Converts to double
                temp[x] = amount;                                                           // Puts money in correct account
                int end = open[i].IndexOf(":");                                             // Finds where account name in row ends
                string currency = open[i].Substring(open[i].LastIndexOf(":") + 1);          // Finds account currency

                if (x >= accountNames.Length)                                               // Expands users accounts if needed.
                {                                                                                //Otherwise enters value at correct index
                    accountNames = ExpandStringArray(accountNames, open[i].Substring(0, end));
                    accountCurrency = ExpandStringArray(accountCurrency, currency);
                }
                else
                {
                    accountCurrency[x] = currency;
                    accountNames[x] = open[i].Substring(0, end);
                }
                x++;
            }

            userSaldos[id] = temp;              // Loads user accounts into program array
            userAccountCount[id] = temp.Length; // Correct amount of accounts
        }

        // Creates a new acount
        static void CreateNewAccount(int id)
        {
            bool creating = true;
            while (creating)
            {
                Console.WriteLine("Välj kontotyp i listan:");
                for (int i = 0; i < accountTypes.Length; i++)
                {
                    Console.WriteLine($"{i}. {accountTypes[i]}");
                }

                int choice = Choice(id, accountTypes.Length - 1);
                string newName = accountTypes[choice];

                if (userAccountCount[id] >= accountNames.Length) // Expanding arrays if necessary
                {
                    accountNames = ExpandStringArray(accountNames, newName);
                    accountCurrency = ExpandStringArray(accountCurrency, "");
                }
                else if (userAccountCount[id] < accountNames.Length)
                {
                    accountNames[userAccountCount[id]] = newName;
                }
                userSaldos[id] = ExpandDecimalArray(userSaldos[id], 0);

                if (newName == "International Account") // International accounts have to chose a currency
                {

                    Console.WriteLine("Vilken valuta vill du ha på ditt International Account?");
                    for (int i = 1; i < currencyNames.Length; i++)
                    {
                        Console.WriteLine($"{i}. {currencyNames[i]} ({currencySymbol[i]})");
                    }

                    int valuta = Choice(activeUserID, currencyNames.Length - 1);
                    accountCurrency[userAccountCount[id]] = currencyNames[valuta];

                    Console.WriteLine($"\nDu skapade ett {newName} med {currencyNames[valuta]} som valuta.\n");
                }
                else
                {
                    Console.WriteLine($"\nDu skapade ett {newName}!\n");
                }

                userAccountCount[id]++;
                creating = ReturnToMain("Tryck på valfri annan knapp för att skapa ytterligare konton.");

                Console.Clear();
                Save(id);
            }
        }

        // Three methods that returns arrays with a size of one bigger than it's input parameter
        // Then adds a value from another input parameter
        static string[] ExpandStringArray(string[] arr, string add)
        {
            string[] temp = new string[arr.Length + 1];

            arr.CopyTo(temp, 0);

            temp[arr.Length] = add;

            return temp;
        }

        static int[] ExpandIntArray(int[] arr, int add)
        {
            int[] temp = new int[arr.Length + 1];

            arr.CopyTo(temp, 0);

            temp[arr.Length + 1] = add;

            return temp;
        } // not used, but in for flexibility

        static decimal[] ExpandDecimalArray(decimal[] arr, decimal add)
        {
            decimal[] temp = new decimal[arr.Length + 1];

            arr.CopyTo(temp, 0);

            temp[arr.Length] = add;

            return temp;
        }

        // Withdrawing funds
        // Contains several safeguards from illegal input
        static void WithdrawFunds(int id)
        {
            bool withdrawing = true;
            int from;
            decimal amount = 0;

            while (withdrawing)
            {
                AccountDisplay(id, false); // displaying accounts that can be withdrawn from (where currency is SEK)
                Console.WriteLine("\nVälj ett konto att ta ut pengar ifrån (tryck på motsvarande siffra på tangentbordet): ");

                from = Choice(id, userAccountCount[id] - 1);

                Console.WriteLine($"Du valde {accountNames[from]}\n");
                Console.WriteLine("Skriv in summa (du kan ta ut pengar i sedlar från 100 SEK och uppåt: ");
                amount = NumberInput(true);
                Console.WriteLine("Bekräfta överföringen med din PIN-kod");
                int attempt = (int)NumberInput(false);

                if (attempt != pincodes[id]) // is pin correct?
                {
                    Console.Beep();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nFel PIN-kod!\n");
                    Console.ResetColor();
                    withdrawing = ReturnToMain("Klicka på valfri knapp för att prova igen.");
                }
                else if (accountNames[from] == "International Account") // is account international?
                {
                    Console.Beep();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nDu kan inte ta ut pengar i andra valutor än SEK!\n ");
                    Console.ResetColor();
                    withdrawing = ReturnToMain("Klicka på valfri knapp för att prova igen.");
                }
                else if (amount > userSaldos[id][from]) // is there enough money on sending account?
                {
                    Console.Beep();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nDet finns inte tillräckligt med pengar på kontot!\n ");
                    Console.ResetColor();
                    withdrawing = ReturnToMain("Klicka på valfri knapp för att prova igen.");
                }
                else if (userSaldos[id][from] == 0) // is sending account empty?
                {
                    Console.Beep();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nKontot är tomt! \n");
                    Console.ResetColor();
                    withdrawing = ReturnToMain("Klicka på valfri knapp för att prova igen.");
                }
                else if (amount == 0) // can't withdraw 0
                {
                    Console.Beep();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nDu kan inte föra över 0 kr!\n ");
                    Console.ResetColor();
                    withdrawing = ReturnToMain("Klicka på valfri knapp för att prova igen.");
                }
                else if (amount % 100 != 0) // can't withdraw coins from a cash machine obviously
                {
                    Console.Beep();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nDu måste ta ut en summa i hundralappar!\n ");
                    Console.ResetColor();
                    withdrawing = ReturnToMain("Klicka på valfri knapp för att prova igen.");
                }
                else
                {
                    // Removes the amount from the sending account
                    // Then prints new balance, and the amount withdrawn

                    userSaldos[id][from] -= amount;

                    Console.Clear();
                    PrintMoney(' ');

                    Console.WriteLine($"Nytt saldo på {accountNames[from]}: {userSaldos[id][from].ToString("C", CultureInfo.CurrentCulture)}");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"Ditt uttag: {amount.ToString("C", CultureInfo.CurrentCulture)}");
                    Console.ResetColor();
                    Save(id);
                    withdrawing = ReturnToMain("Klicka på valfri knapp för att göra ett nytt uttag.");
                }
            }
        }

        // Viewing Accounts "shell", putting the actual display in another method
        // allows the program to show it in other methods without it asking to
        // return to the main menu at inopportune times :) 
        // Could probably be removed, but is in for future flexibility
        static void ViewAccounts(int id)
        {
            Console.Clear();

            bool viewing = true;
            while (viewing)
            {
                AccountDisplay(id, true);
                viewing = ReturnToMain("");
            }
            Console.Clear();
        }

        // The actual displaying of the accounts and information is in this method
        // So it can be accessed from Transfer- and Withdrawal methods.
        // Doesn't display international accounts if international is false
        static void AccountDisplay(int id, bool international)
        {
            // Strings for formatting
            string divider = "    ---------------------------|------------------------";
            string space = "";

            Console.WriteLine("Dina konton:\n");
            Console.WriteLine($"{"    Konto",-27}{"Saldo",24}");
            Console.WriteLine(divider);

            for (int i = 0; i < userAccountCount[id]; i++)
            {
                if (i < 10) // Formatting
                {
                    space = " ";
                }
                else
                {
                    space = "";
                }

                if (international && accountNames[i] == "International Account")
                {
                    CultureInfo brp;

                    switch (accountCurrency[i])
                    {
                        case "Euro":
                            brp = new CultureInfo("fi-FI", false);

                            break;
                        case "Dollar":
                            brp = new CultureInfo("en-US", false);

                            break;
                        case "Pund":
                            brp = new CultureInfo("en-GB", false);

                            break;
                        default:
                            brp = CultureInfo.CurrentCulture;
                            break;
                    }

                    Console.WriteLine($"{i}.{space} {accountNames[i],-27}|{userSaldos[id][i].ToString("C", brp.NumberFormat),24}");
                    Console.WriteLine(divider);
                }
                else if (accountNames[i] != "International Account")
                {
                    // using CultureInfo to display in SEK (or current)
                    Console.WriteLine($"{i}.{space} {accountNames[i],-27}|{userSaldos[id][i].ToString("C", CultureInfo.CurrentCulture),24}");
                    Console.WriteLine(divider);
                }


            }
        }

        // A method that returns true if the user wants to
        // remain in the current section of the program.
        // Returns true if they choose to return to main menu
        // Takes input with description of additional option for klicking other key
        // than enter.
        static bool ReturnToMain(string or)
        {
            string star = "";  // Formatting
            if (or.Length > 1)
            {
                star = "* ";
            }

            Console.WriteLine($"* Klicka Enter för att komma tillbaka till huvudmenyn. \n{star}{or}");
            ConsoleKeyInfo cki = Console.ReadKey();

            if (cki.Key == ConsoleKey.Enter)
            {
                Console.Clear();
                Save(activeUserID);
                return false;
            }
            else
            {
                Console.Clear();
                return true;
            }
        }

        // Login function
        static bool LogIn()
        {
            Console.WriteLine("Vänligen fyll i användarnamn och PIN för att logga in: ");

            // Properties

            // Maximum number of PIN entries is 3
            int maxPinTries = 3;

            // Two bools that are used to control while loops below
            bool userCorrect = false;

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
                    activeUserID = Array.IndexOf(users, userName);

                    // User enters a pin code
                    // This while loops runs a number of times equal to number of max tries.
                    // After that, the LogIn function returns false.

                    for (int i = 0; i <= maxPinTries; i++)
                    {
                        Console.WriteLine("PIN-kod: ");
                        int pinEntry = (int)NumberInput(false);

                        if (pinEntry == pincodes[activeUserID])
                        {
                            Load(activeUserID);
                            Console.Clear();
                            Console.WriteLine($"Login lyckades! Välkommen {users[activeUserID]}. ");
                            Thread.Sleep(1500);
                            return true;
                        }
                        else
                        {
                            Console.WriteLine($"Fel PIN-kod. {maxPinTries - i} försök kvar.");
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
                Console.WriteLine("--- MENY ----\n");
                Console.WriteLine("1. Se dina konton och saldo");
                Console.WriteLine("2. Överföring mellan konton");
                Console.WriteLine("3. Ta ut pengar");
                Console.WriteLine("4. Skapa nytt konto");
                Console.WriteLine("5. Logga ut");
                Console.WriteLine("\nGör ditt val genom att trycka på motsvarande siffra på tangentbordet!\n");

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
                        TransferFunds(activeUserID);
                        break;
                    case '3':
                        WithdrawFunds(activeUserID);
                        break;
                    case '4':
                        CreateNewAccount(activeUserID);
                        break;
                    case '5':
                        menuOn = false;
                        break;
                    default:
                        Console.WriteLine("\nOgiltigt val! Prova igen!\n");
                        Thread.Sleep(500);
                        Console.Clear();
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
        static decimal NumberInput(bool money)
        {
            bool isNumber = false;
            string numberString = "";
            decimal number;
            ConsoleKeyInfo cki;

            do
            {   // Takes input and checks if it is a number
                cki = Console.ReadKey(true);
                isNumber = decimal.TryParse(cki.KeyChar.ToString(), out decimal check);

                if (cki.Key != ConsoleKey.Backspace)
                {
                    if (isNumber) //if it is a number, adds to string of numbers 
                    {
                        // Makes sure user can only type two digits after entering a comma
                        if (numberString.Contains(",") && numberString.IndexOf(",") < numberString.Length - 2)
                        {
                            // do nothing
                        }
                        else
                        {
                            numberString += cki.KeyChar;
                            Console.Write(cki.KeyChar);
                        }
                    }
                    else if (money && !numberString.Contains(",") && cki.Key == ConsoleKey.OemComma)
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
            decimal.TryParse(numberString, CultureInfo.CurrentCulture, out number);

            return number;
        }

        // A function that takes user id as input and returns an
        // valid "id" number, used in single-key input.
        // takes a "max" input to make sure number returned is not too large.
        static int Choice(int id, int max)
        {
            bool isNumber;
            int num = 0;
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                // Checks if the choice is a number
                // If it is, converts keypress
                // into an int. 
                isNumber = Char.IsAsciiDigit(key.KeyChar);
                if (isNumber)
                {
                    num = Convert.ToInt32(key.KeyChar.ToString());
                }

            } while (!isNumber || num > max); // exits while loop only if number is not bigger
                                              // than the max index of the active users accounts.
            return num;
        }

        // Method for transfering funds between accounts
        static void TransferFunds(int id)
        {
            int from;
            int to;
            decimal amount;
            bool transfering = true;

            while (transfering)
            {
                AccountDisplay(id, true); // Displaying for easier access.

                Console.WriteLine("Skriv in från-konto: ");
                from = Choice(id, userAccountCount[id] - 1);
                Console.WriteLine($"Från: {accountNames[from]}. Skriv in till-konto:");
                to = Choice(id, userAccountCount[id] - 1);

                if (from == to) // Can't transfer to and from the same account
                {
                    Console.Beep();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nFrån- och Till- konto kan inte vara samma!\n");
                    Console.ResetColor();
                    transfering = ReturnToMain("Tryck på valfri annan knapp för att prova igen!");
                }
                else
                {
                    Console.WriteLine($"Till: {accountNames[to]}. Skriv in summa som ska överföras:");
                    amount = NumberInput(true);

                    if (amount > userSaldos[id][from]) // Can't transfer more money than you have
                    {
                        Console.Beep();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\nDet finns inte tillräckligt saldo på {accountNames[from]}!\n");
                        Console.ResetColor();
                        transfering = ReturnToMain("Tryck på valfri annan knapp för att prova igen!");
                    }
                    else if (amount == 0) // Can't transfer money if account is empty
                    {
                        Console.Beep();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\nDu kan inte föra över 0 kr! \n");
                        Console.ResetColor();
                        transfering = ReturnToMain("Tryck på valfri annan knapp för att prova igen!");
                    }
                    else // Converts the amount to receiving account currency,
                         // prints it, and asks if user wants to do it again or return to main menu.
                    {

                        decimal convert = ConvertCurrency(amount, accountCurrency[from], accountCurrency[to]);

                        userSaldos[id][from] -= amount;
                        userSaldos[id][to] += convert;

                        CultureInfo displayFrom;  // Checks sending and receiving acount currency
                        CultureInfo displayTo;    // for displaying purposes

                        switch (accountCurrency[from])
                        {
                            case "Euro":
                                displayFrom = new CultureInfo("fi-FI", false);

                                break;
                            case "Dollar":
                                displayFrom = new CultureInfo("en-US", false);

                                break;
                            case "Pund":
                                displayFrom = new CultureInfo("en-GB", false);

                                break;
                            default:
                                displayFrom = CultureInfo.CurrentCulture;
                                break;
                        }

                        switch (accountCurrency[to])
                        {
                            case "Euro":
                                displayTo = new CultureInfo("fi-FI", false);

                                break;
                            case "Dollar":
                                displayTo = new CultureInfo("en-US", false);

                                break;
                            case "Pund":
                                displayTo = new CultureInfo("en-GB", false);

                                break;
                            default:
                                displayTo = CultureInfo.CurrentCulture;
                                break;
                        }

                        Console.WriteLine($"{accountNames[from]}: {userSaldos[id][from].ToString("C", displayFrom.NumberFormat)} (-{amount.ToString("C", displayFrom.NumberFormat)})");
                        Console.WriteLine($"{accountNames[to]}: {userSaldos[id][to].ToString("C", displayTo.NumberFormat)} (+{convert.ToString("C", displayTo.NumberFormat)})\n");

                        Save(id);
                        transfering = ReturnToMain("Tryck på valfri annan knapp för att göra en ny överföring.");

                    }
                }
            }
        }


        // External functionality
        [DllImport("winmm.dll", EntryPoint = "mciSendString")]
        public static extern int mciSendStringA(string lpstrCommand, string lpstrReturnString,
                                                int uReturnLength, int hwndCallback);

        // Converts currency from one to another (first to SEK if necessary to be able
        // to use the conversion rates array)
        static decimal ConvertCurrency(decimal number, string from, string to)
        {

            number = number * conversionRates[Array.IndexOf(currencyNames, from)];

            decimal conv = number / conversionRates[Array.IndexOf(currencyNames, to)];

            Console.WriteLine($"Converted from {from} to {to} = {conv}");

            return conv;
        }

    }
}
