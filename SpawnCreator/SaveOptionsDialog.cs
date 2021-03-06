﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SpawnCreator
{
    public partial class SaveOptionsDialog : Form
    {
        public SaveOptionsDialog()
        {
            InitializeComponent();
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Form_ItemCreator.stringSQLShare);
            //this.Close();

            //button2.Text = "Copied to Clipboard!";
            // button2.ForeColor = Color.Green;

            label_copy_to_clipboard.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "sql files (*.sql)|*.sql";
                sfd.FilterIndex = 2;
                sfd.FileName = "Item_" + Form_ItemCreator.stringEntryShare;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, Form_ItemCreator.stringSQLShare);
                }
            }
            this.Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void button_execute_query_Click(object sender, EventArgs e)
        {
            Form_ItemCreator frm = new Form_ItemCreator();
            if (frm.textBox1.Text == "")
            {
                MessageBox.Show("Entry column should not be empty", "Error");
                return;
            }

            //Clipboard.SetText(Form_ItemCreator.stringSQLShare);
            Form_MainMenu mainmenu = new Form_MainMenu();
            MySqlConnection connection = new MySqlConnection("datasource=" + mainmenu.textbox_mysql_hostname.Text + ";port=" + mainmenu.textbox_mysql_port.Text + ";username=" + mainmenu.textbox_mysql_username.Text + ";password=" + mainmenu.textbox_mysql_pass.Text);
            string insertQuery = Form_ItemCreator.stringSQLShare;
            connection.Open();
            MySqlCommand command = new MySqlCommand(insertQuery, connection);

            // Test
            try
            {
                //frm.label_query_executed_successfully.Visible = true;
                if (command.ExecuteNonQuery() == 1)
                {
                   // this.Close();
                    //frm.label83.ForeColor = Color.GreenYellow;
                    label_query_executed_successfully.Visible = true;
                }
                else
                {
                    //MessageBox.Show("Data Not Inserted");
                    //label2.ForeColor = Color.Red;
                    //label2.Text = "Eroare!";
                    //MessageBox.Show("Unable to connect to any of the specified MySQL hosts.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        private void SaveOptionsDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            label_query_executed_successfully.Visible = false;
            label_copy_to_clipboard.Visible = false;
        }
    }
}
