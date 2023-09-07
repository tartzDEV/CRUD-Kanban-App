# Documentatie voor KanbanApp

Dit is een C#-programma voor het beheren van taken in een Kanban-bordstijl. Het programma maakt gebruik van de Windows Presentation Foundation (WPF) voor de gebruikersinterface en communiceert met een MySQL-database om taken op te slaan en te beheren.

## Klasse `KanbanTopicsWindow`

`KanbanTopicsWindow` is een klasse die een venster vertegenwoordigt waarin de Kanban-taken worden weergegeven en beheerd. Hier is een overzicht van de belangrijkste onderdelen van deze klasse:

### Variabelen en Eigenschappen

- `connection`: Een MySQL-verbinding om te communiceren met de database.
- `projectName`: De naam van het project waarvoor de taken worden beheerd.
- `openTasks`, `inProgressTasks`, `completedTasks`: ObservableCollections om de taken in verschillende categorieën (Open, Bezig, Afgerond) op te slaan en weer te geven in de gebruikersinterface.

### Constructor `KanbanTopicsWindow(string projectName)`

- Deze constructor initialiseert het venster en neemt de projectnaam als parameter.
- Het maakt de databaseverbinding en laadt de taken in de juiste ObservableCollections.
- Elk van de ObservableCollections wordt gekoppeld aan de juiste ListBox in de gebruikersinterface.

### Methode `LoadTasks()`

- Deze methode haalt taken op uit de database en verdeelt ze in de juiste ObservableCollections op basis van hun status (Open, Bezig, Afgerond).
- Het gebruikt een `switch`-statement om de taken te categoriseren.

### Methode `AddTask_Click(object sender, RoutedEventArgs e)`

- Deze methode wordt aangeroepen wanneer de knop "Taak toevoegen" wordt ingedrukt.
- Het toont een dialoogvenster waarin de gebruiker een nieuwe taaknaam kan invoeren.
- Als een geldige naam wordt ingevoerd, wordt de nieuwe taak toegevoegd aan de database en aan de `openTasks`-ObservableCollection.

### Methode `MoveTask(string taskName, string newStatus)`

- Deze methode verplaatst een taak van de ene status naar de andere (bijv. van "Open" naar "Bezig" of van "Bezig" naar "Afgerond").
- Het bijwerken van de database en de ObservableCollections gebeurt hier.
- Het toont ook een berichtvenster om de gebruiker te informeren over de statuswijziging.

### Methode `RemoveTask(string taskName)`

- Deze methode verwijdert een taak uit de database en de juiste ObservableCollections.
- Het toont een berichtvenster om de gebruiker te informeren over de verwijdering.

### Event Handlers

- Er zijn drie event handlers voor ListBox-selectiewijzigingen: `OpenTasksListBox_SelectionChanged`, `InProgressTasksListBox_SelectionChanged` en `CompletedTasksListBox_SelectionChanged`. Ze worden aangeroepen wanneer een taak van categorie verandert en roepen de `MoveTask`-methode aan om de taak te verplaatsen naar de juiste categorie.

### Methode `RemoveTask_Click(object sender, RoutedEventArgs e)`

- Deze methode wordt aangeroepen wanneer de knop "Taak verwijderen" wordt ingedrukt.
- Het vraagt de gebruiker om de naam van de taak die moet worden verwijderd.
- Als de taaknaam geldig is en overeenkomt met een bestaande taak, wordt de gebruiker om bevestiging gevraagd en wordt de taak verwijderd als de bevestiging positief is.

### Methode `CloseButton_Click(object sender, RoutedEventArgs e)`

- Deze methode wordt aangeroepen wanneer de knop "Sluiten" wordt ingedrukt en sluit het venster.

Dit C#-programma biedt een eenvoudige Kanban-achtige taakbeheerfunctionaliteit met de mogelijkheid om taken toe te voegen, te verplaatsen tussen statussen en taken te verwijderen voor een specifiek project. Het gebruikt een MySQL-database voor het opslaan en beheren van taken en biedt een intuïtieve gebruikersinterface met behulp van WPF.
