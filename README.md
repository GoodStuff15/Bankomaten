# Bankomaten

Project for SUT24 - Utveckling med C# och .NET
In C# and .NET framework
Built by: Gustav Eriksson Söderlund

A small console app that simulates a cash machine with some internet bank functionality.

Uses arrays and jagged arrays to keep track of active user and its properties.
Also contains functions for safe number input.

Generates random amounts in user accounts on first run, after that saves and loads information from text files.

--- Motivering --- (namespace: swedish)

Skriva en reflektion/ett resonemang där du motiverar för hur du valt att bygga upp ditt program. Du ska alltså resonera kring den lösning du valt, vilka andra du övervägt och kritiskt granska ditt val och eventuellt motivera för bättre lösningar som du ser men inte gjort. Denna motivering ska finnas i din Readme-fil.

Efter en del funderande valde jag att använda mig av olika arrays / jagged arrays för att hålla ordning på användare
och konton. Vid skapandet av användare och konton läggs all info för varje användare vid samma index i samtliga arrays.
Vid login sätts ett active user ID, vilket motsvarar korrekt index. På detta sätt
blev det smidigt att komma åt rätt information.

Jag funderade på att göra enskilda jagged eller multidimensional arrays för varje användare, men valde bort det för att
enklare kunna lägga till/ta bort användare i framtiden.

I efterhand har jag lärt mig om multidimensioella jagged arrays, vilket jag kunde ha använt att spara all information i.
Det hade gjort saker som att lägga till olika sorters konton mer dynamiskt. Jag hade dock fått konvertera / parse:a information på 
många ställen vilket lätt kunnat bli fel.

Jag har också valt att använda ConsoleKeyInfo för mycket av användarens input. Det har gjort att jag sluppit mycket
"try catch":ande och villkorssatser i koden - samt att programmet helt enkelt inte "accepterar" fel tryck
på tangentbordet. Det blir mer intuitivt att använda och koden är enklare att läsa i funktionerna
som handlar om överföringar och uttag.

Om jag hade börjat om från början hade jag nog provat med en multidimensionell jagged array för all information.
Jag misstänker att det upplägget hade varit lättare att få mer dynamiskt än vad mitt program är idag.

Jag hade också med mer tid försökt undvika nästlade loopar och if-statements mer än vad jag gjort idag, för 
att göra läsbarheten bättre och framtida felsökning enklare.

Allt eftersom jag har provat att lägga till nya "extra-funktioner" har det blivit svårare att hålla koden 
enhetlig. Jag misstänker att det hade varit enklare med OOP dock.



