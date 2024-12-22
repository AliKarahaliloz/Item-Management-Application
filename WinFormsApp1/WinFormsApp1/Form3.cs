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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        //go to form2 (see items menu)
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        //add a new item to the database
        private void button2_Click(object sender, EventArgs e)
        {

            //get input values from textboxes
            string itemName = textBox1.Text.Trim();
            string itemType = textBox2.Text.Trim();
            string itemPrice = textBox3.Text.Trim();

            //check if any input field is empty
            if (string.IsNullOrEmpty(itemName) || string.IsNullOrEmpty(itemType) || string.IsNullOrEmpty(itemPrice))
            {
                MessageBox.Show("You need to full all areas!");
                return;
            }

            //validate if the price is a numeric value
            if (!decimal.TryParse(itemPrice, out _))
            {
                MessageBox.Show("Price must be a numeric value!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            try
            {
                using (SQLiteConnection connection = new SQLiteConnection("Data Source=C:\\databaseforproject\\databaseitems.db;Version=3;Pooling=False"))
                {
                    connection.Open();



                    //begin transaction
                    using (var transaction = connection.BeginTransaction())
                    {



                        //SQL query to insert a new item into the database
                        string query = "INSERT INTO Items (ItemName, Type, Price) VALUES (@ItemName, @Type, @Price)";
                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {   //bind parameters to prevent SQL injection
                            command.Parameters.AddWithValue("@ItemName", itemName);
                            command.Parameters.AddWithValue("@Type", itemType);
                            command.Parameters.AddWithValue("@Price", itemPrice);
                            command.ExecuteNonQuery();
                            MessageBox.Show("Item saved successfully!");
                        }

                        transaction.Commit();
                        connection.Close();
                    }

                    connection.Close();


                }
            }
            //show error message if an exception occurs
            catch (Exception ex)
            {
                MessageBox.Show("An error occured: " + ex.Message);
            }




        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        //delete an item from the database
        private void button3_Click(object sender, EventArgs e)
        {
            //get the name of the item to delete from the user
            string itemNameToDelete = textBox4.Text.Trim();

            //if the input field is empty, show a warning to the user
            if (string.IsNullOrEmpty(itemNameToDelete))
            {
                MessageBox.Show("Please enter the name of the item to delete!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection("Data Source=C:\\databaseforproject\\databaseitems.db;Version=3;Pooling=False"))
                {
                    connection.Open();

                    //query to check if the item exists in the database
                    string selectQuery = "SELECT * FROM Items WHERE ItemName = @ItemName";
                    using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@ItemName", itemNameToDelete);

                        using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                        {
                            //if the item does not exist, show a warning message
                            if (!reader.HasRows)
                            {
                                MessageBox.Show("Item not found in the database!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                    //if the item exists, proceed to delete it
                    string deleteQuery = "DELETE FROM Items WHERE ItemName = @ItemName";
                    using (SQLiteCommand deleteCommand = new SQLiteCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@ItemName", itemNameToDelete);
                        deleteCommand.ExecuteNonQuery();
                        MessageBox.Show("Item deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                //show an error message if an exception occurs
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
