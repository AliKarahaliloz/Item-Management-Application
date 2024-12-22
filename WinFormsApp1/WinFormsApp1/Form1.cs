namespace WinFormsApp1;
using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;




public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        CreateDatabase(); //launch the function

    }


    //create the database of the user name and password
    private void CreateDatabase()
    {
        //connect to the database address
        string connectionString = "Data Source=C:\\databaseforproject\\databaseproject.db;Version=3;";

        //create if database does not exist
        if (!System.IO.File.Exists("C:\\databaseproject.db"))
        {
            SQLiteConnection.CreateFile("C:\\databaseproject.db");
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                //create the users table if not exists
                string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Users (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Username TEXT NOT NULL UNIQUE,
                            Password TEXT NOT NULL
                        );";

                using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }



                //create the items table if not exists
                string createItemsTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Items (
                     Id INTEGER PRIMARY KEY AUTOINCREMENT,
                     ItemName TEXT NOT NULL,
                     Type TEXT NOT NULL,
                     Price TEXT NOT NULL
                      );";
                using (SQLiteCommand command = new SQLiteCommand(createItemsTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }



                connection.Close();
            }
        }
    }





    private void label1_Click(object sender, EventArgs e)
    {

    }


    //when clicked the register button
    private void button1_Click(object sender, EventArgs e)
    {
        //getting the inputs from textboxes and saving
        string username = textBox1.Text.Trim();
        string password = textBox2.Text.Trim();


        //if user does not fill one of the textboxes, pop up error
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show("Username or Password can't be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }


        //start register
        try
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=C:\\databaseforproject\\databaseproject.db;Version=3;"))
            {
                connection.Open();

                //insert queries to database
                string insertQuery = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", username); //username parameter
                    command.Parameters.AddWithValue("@Password", password); //password parameter

                    command.ExecuteNonQuery(); //run the query
                    MessageBox.Show("Register Successful!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                connection.Close();
            }
        }

        //pop up exception if an error occurs
        catch (Exception ex)
        {
            MessageBox.Show("An error occured: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }








    private void textBox1_TextChanged_1(object sender, EventArgs e)
    {

    }

    private void textBox2_TextChanged(object sender, EventArgs e)
    {

    }



    private void textBox3_TextChanged(object sender, EventArgs e)
    {

    }

    private void textBox4_TextChanged(object sender, EventArgs e)
    {

    }



    //when clicked the login button
    private void loginenter_Click(object sender, EventArgs e)
    {

        //getting the inputs from textboxes and saving
        string username = textBox3.Text.Trim();
        string password = textBox4.Text.Trim();


        //if user does not fill one of the textboxes, pop up error
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show("Username or Password can't be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }


        //start login
        try
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=C:\\databaseforproject\\databaseproject.db;Version=3;"))
            {
                connection.Open();

                //compare if the input and database data matches
                string selectQuery = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
                
                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", username); //username parameter
                    command.Parameters.AddWithValue("@Password", password); //password parameter

                    
                    //run the query and get the results
                    SQLiteDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        MessageBox.Show("Login Successful!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //if datas matches, go to second form
                        Form2 form2 = new Form2();
                        form2.Show();
                        this.Hide();

                        //closing database connections
                        reader.Close(); 
                        command.Dispose(); 
                        connection.Close(); 



                    }
                    //if datas does not match, throw error
                    else
                    {
                        //closing database connections
                        reader.Close();
                        command.Dispose(); 
                        connection.Close(); 

                        MessageBox.Show("Wrong username or password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }



                connection.Close();
            }
        }
        //pop up exception if an error occurs
        catch (Exception ex)
        {
            MessageBox.Show("There is an error occured: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}





