# Documentatie voor KanbanApp - MainWindow

Dit is een C#-programma voor het beheren van Kanban-borden. De `MainWindow`-klasse vertegenwoordigt het hoofdvenster van de applicatie waarin gebruikers nieuwe Kanban-borden kunnen maken of bestaande borden kunnen openen.

## Variabelen en Eigenschappen

- `connection`: Een MySQL-verbinding om te communiceren met de database.
- `connectionString`: De verbindingsreeks voor de MySQL-database.
- `projectName`: Een variabele om de naam van het huidige project bij te houden.

## Constructor `MainWindow()`

- Deze constructor initialiseert het hoofdvenster en maakt een databaseverbinding met behulp van de `connectionString`.

## Methode `LoadKanbanBoard_Click(object sender, RoutedEventArgs e)`

- Deze methode wordt aangeroepen wanneer de knop "Kanban-bord laden" wordt ingedrukt.
- Het haalt de projectnaam op uit een tekstvak (`ProjectNameTextBox`).
- Probeert de database te openen en zoekt naar een project met de opgegeven naam.
- Als het project wordt gevonden, wordt een nieuw venster (`KanbanTopicsWindow`) geopend voor dat project, anders wordt een foutmelding weergegeven.

## Methode `CreateKanbanBoard_Click(object sender, RoutedEventArgs e)`

- Deze methode wordt aangeroepen wanneer de knop "Kanban-bord maken" wordt ingedrukt.
- Het haalt de projectnaam op uit een tekstvak (`ProjectNameTextBox`).
- Probeert de database te openen en controleert of er al een project bestaat met dezelfde naam.
- Als het project al bestaat, wordt een melding weergegeven. Zo niet, dan wordt het nieuwe project toegevoegd aan de database en wordt een succesmelding weergegeven.

Deze C#-toepassing biedt een eenvoudige manier om Kanban-borden te maken en te openen met behulp van een MySQL-database voor opslag. Gebruikers kunnen projecten maken, bestaande projecten openen en Kanban-borden beheren.
