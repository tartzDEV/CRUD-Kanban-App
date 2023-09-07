using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using MySql.Data.MySqlClient;

namespace KanbanApp
{
    public partial class MainWindow : Window
    {
        private MySqlConnection connection;
        private string connectionString = "Server=localhost;Database=test;Uid=root;Pwd=;";
        private string projectName = "";


        public MainWindow()
        {
            InitializeComponent();
            connection = new MySqlConnection(connectionString);
        }

        private void LoadKanbanBoard_Click(object sender, RoutedEventArgs e)
{
                projectName = ProjectNameTextBox.Text;

    try
    {
        connection.Open();

        MySqlCommand cmd = new MySqlCommand("SELECT * FROM projects WHERE project_name = @projectName", connection);
        cmd.Parameters.AddWithValue("@projectName", projectName);

        MySqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
                    KanbanTopicsWindow kanbanTopicsWindow = new KanbanTopicsWindow(projectName);
            kanbanTopicsWindow.Show();

            MessageBox.Show($"Project Naam: {projectName}");
        }
        else
        {
            MessageBox.Show("Project niet gevonden.");
        }

        reader.Close();
    }
    catch (Exception ex)
    {
        MessageBox.Show("Fout bij het ophalen van het Kanban-bord: " + ex.Message);
    }
    finally
    {
        connection.Close();
    }
}


        private void CreateKanbanBoard_Click(object sender, RoutedEventArgs e)
        {
            projectName = ProjectNameTextBox.Text;

            try
            {
                connection.Open();

                // Controleer of het project met dezelfde naam al bestaat
                MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM projects WHERE project_name = @projectName", connection);
                checkCommand.Parameters.AddWithValue("@projectName", projectName);
                int projectCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (projectCount > 0)
                {
                    MessageBox.Show("Een project met dezelfde naam bestaat al.");
                }
                else
                {
                    // Voeg het project toe als het niet bestaat
                    MySqlCommand insertCommand = new MySqlCommand("INSERT INTO projects (project_name) VALUES (@projectName)", connection);
                    insertCommand.Parameters.AddWithValue("@projectName", projectName);

                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Kanban-bord succesvol aangemaakt.");
                    }
                    else
                    {
                        MessageBox.Show("Kanban-bord kon niet worden aangemaakt.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het maken van het Kanban-bord: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }


    }
}
