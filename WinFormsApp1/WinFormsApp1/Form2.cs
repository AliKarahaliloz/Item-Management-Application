
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }




        //load all items and print them to textbox1 (all items textbox)
        private void LoadAllItems()
        {
            try
            {
                //open database connection
                using (SQLiteConnection connection = new SQLiteConnection("Data Source=C:\\databaseforproject\\databaseitems.db;Version=3;"))
                {
                    connection.Open();
                    //sql query to select objects
                    string query = "SELECT ItemName, Type, Price FROM Items";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            //collect objects with stringbuilder
                            StringBuilder items = new StringBuilder();
                            while (reader.Read())
                            {
                                //append the items
                                items.AppendLine($"Item: {reader["ItemName"]}, Type: {reader["Type"]}, Price: {reader["Price"]}");
                            }
                            //print items to textbox1 (all items textbox)
                            textBox1.Text = items.ToString();
                        }
                    }
                    connection.Close();
                }
            }
            //pop up error if exception occurs
            catch (Exception ex)
            {
                MessageBox.Show("An error occured: " + ex.Message);
            }
        }



        //when form2 loads, load all items
        private void Form2_Load(object sender, EventArgs e)
        {
            LoadAllItems();
        }


        //when user clicks button1 (search item) button after enter the item name
        private void button1_Click(object sender, EventArgs e)
        {

            //get the object user enters
            string searchItem = textBox3.Text.Trim();
            
            //warn the user if textbox is empty
            if (string.IsNullOrEmpty(searchItem))
            {
                MessageBox.Show("Please enter an item name!");
                return;
            }


            try
            {   
                //open database connection
                using (SQLiteConnection connection = new SQLiteConnection("Data Source=C:\\databaseforproject\\databaseitems.db;Version=3;"))
                {
                    connection.Open();
                    //sql query to select item
                    string query = "SELECT * FROM Items WHERE ItemName = @ItemName";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        //add object name as parameter
                        command.Parameters.AddWithValue("@ItemName", searchItem);
                        
                        
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {   //if item is found
                            if (reader.Read())
                            {
                                textBox2.Text = $"Item: {reader["ItemName"]}, Type: {reader["Type"]}, Price: {reader["Price"]}\n";
                            }
                            //if item is not found
                            else
                            {   
                                textBox2.Text = "Item have not found!";
                            }
                        }
                    }
                    connection.Close();
                }
            }
            //pop up error if exception occurs
            catch (Exception ex)
            {
                MessageBox.Show("An error occured: " + ex.Message);
            }




        }



        //go to form3 (the button of add/delete item menu)
        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {


        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }


        //go to form1 (logout button)
        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
    }
}

