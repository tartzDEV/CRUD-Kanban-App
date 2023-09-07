using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace KanbanApp
{
    public partial class KanbanTopicsWindow : Window
    {
        private MySqlConnection connection;
        private string projectName;

        private ObservableCollection<string> openTasks = new ObservableCollection<string>();
        private ObservableCollection<string> inProgressTasks = new ObservableCollection<string>();
        private ObservableCollection<string> completedTasks = new ObservableCollection<string>();

        public KanbanTopicsWindow(string projectName)
        {
            InitializeComponent();
            this.projectName = projectName;
            connection = new MySqlConnection("Server=localhost;Database=test;Uid=root;Pwd=;");

            OpenTasksListBox.ItemsSource = openTasks;
            InProgressTasksListBox.ItemsSource = inProgressTasks;
            CompletedTasksListBox.ItemsSource = completedTasks;

            LoadTasks();
        }

        private void LoadTasks()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();

                    string selectQuery = "SELECT task_name, task_status FROM tasks WHERE project_name = @projectName";
                    MySqlCommand command = new MySqlCommand(selectQuery, connection);
                    command.Parameters.AddWithValue("@projectName", projectName);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        openTasks.Clear();
                        inProgressTasks.Clear();
                        completedTasks.Clear();

                        while (reader.Read())
                        {
                            string taskName = reader["task_name"].ToString();
                            string taskStatus = reader["task_status"].ToString();

                            switch (taskStatus)
                            {
                                case "Open":
                                    openTasks.Add(taskName);
                                    break;
                                case "Bezig":
                                    inProgressTasks.Add(taskName);
                                    break;
                                case "Afgerond":
                                    completedTasks.Add(taskName);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het laden van taken: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string newTask = Microsoft.VisualBasic.Interaction.InputBox("Voer een nieuwe taak in:", "Nieuwe Taak", "");

            if (!string.IsNullOrWhiteSpace(newTask))
            {
                try
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO tasks (project_name, task_name, task_status) VALUES (@projectName, @taskName, @taskStatus)";
                    MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@projectName", projectName);
                    insertCommand.Parameters.AddWithValue("@taskName", newTask);
                    insertCommand.Parameters.AddWithValue("@taskStatus", "Open");

                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Nieuwe taak toegevoegd.");
                        openTasks.Add(newTask);
                    }
                    else
                    {
                        MessageBox.Show("Fout bij het toevoegen van de taak.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fout bij het toevoegen van de taak: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void MoveTask(string taskName, string newStatus)
        {
            try
            {
                connection.Open();

                string updateQuery = "UPDATE tasks SET task_status = @newStatus WHERE project_name = @projectName AND task_name = @taskName";
                MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@projectName", projectName);
                updateCommand.Parameters.AddWithValue("@taskName", taskName);
                updateCommand.Parameters.AddWithValue("@newStatus", newStatus);

                int rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show($"Taak \"{taskName}\" is verplaatst naar {newStatus}.");

                    if (newStatus == "Bezig")
                    {
                        openTasks.Remove(taskName);
                        inProgressTasks.Add(taskName);
                        completedTasks.Remove(taskName);
                    }
                    else if (newStatus == "Afgerond")
                    {
                        openTasks.Remove(taskName);
                        inProgressTasks.Remove(taskName);
                        completedTasks.Add(taskName);
                    }

                    OpenTasksListBox.Items.Refresh();
                    InProgressTasksListBox.Items.Refresh();
                    CompletedTasksListBox.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Fout bij het verplaatsen van de taak.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het verplaatsen van de taak: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void RemoveTask(string taskName)
        {
            try
            {
                connection.Open();

                string deleteQuery = "DELETE FROM tasks WHERE project_name = @projectName AND task_name = @taskName";
                MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@projectName", projectName);
                deleteCommand.Parameters.AddWithValue("@taskName", taskName);

                int rowsAffected = deleteCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show($"Taak \"{taskName}\" is verwijderd.");

                    openTasks.Remove(taskName);
                    inProgressTasks.Remove(taskName);
                    completedTasks.Remove(taskName);

                    OpenTasksListBox.Items.Refresh();
                    InProgressTasksListBox.Items.Refresh();
                    CompletedTasksListBox.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Fout bij het verwijderen van de taak.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het verwijderen van de taak: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void OpenTasksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedTask = OpenTasksListBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedTask))
            {
                MoveTask(selectedTask, "Bezig");
            }
        }

        private void InProgressTasksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedTask = InProgressTasksListBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedTask))
            {
                MoveTask(selectedTask, "Afgerond");
            }
        }

        private void CompletedTasksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedTask = CompletedTasksListBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedTask))
            {
                MoveTask(selectedTask, "Bezig");
            }
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
                string userInput = Microsoft.VisualBasic.Interaction.InputBox("Voer de naam van de taak in om deze te verwijderen:", "Verwijder Taak", "");
                if (!string.IsNullOrWhiteSpace(userInput) && userInput == userInput)
                {
                    if (MessageBox.Show($"Weet je zeker dat je de taak \"{userInput}\" wilt verwijderen?", "Bevestig Verwijdering", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        RemoveTask(userInput);
                    }
                }
                else
                {
                    MessageBox.Show("Ongeldige invoer of invoer komt niet overeen met de geselecteerde taaknaam.");
                }
           
        } 
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
