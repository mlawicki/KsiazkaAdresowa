using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;


namespace KsiazkaAdresowa
{
    public partial class Form1 : Form
    {
        public string ConnectionString = "Server=Localhost;Database=KsiazkaAdresowa;Uid=root;Pwd=";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            przeladujBazeDanych();
        }

        public void SQL(string command)
        {


            try
            {
                MySqlConnection connection = new MySqlConnection(ConnectionString);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(command, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                imieBox.Clear();
                adresBox.Clear();
                emailBox.Clear();
                dateTimePicker1.Value = DateTime.Now;

            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message.ToString()); }
        }

        public void przeladujBazeDanych()
        {
            listView1.Clear();

            MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();
            string query = "SELECT * FROM ludzie";
            MySqlDataAdapter da = new MySqlDataAdapter(query, connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            connection.Close();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ListViewItem item = new ListViewItem(row[0].ToString());
                item.SubItems.Add(row[1].ToString());
                item.SubItems.Add(row[2].ToString());
                item.SubItems.Add(row[3].ToString());
                item.SubItems.Add(row[4].ToString());
                listView1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string imie, adres, email, data, dodatkowe;
                imie = imieBox.Text;
                adres = adresBox.Text;
                email = emailBox.Text;
                data = dateTimePicker1.Value.ToString();
                dodatkowe = dodatkoweBox.Text;

                string cmd = "INSERT INTO ludzie (imie, adres, mail, urodziny, dodatkowe) VALUES ('" + imie + "', '" + adres + "', '" + email + "', '" + data + "', '" + dodatkowe + "')";
                SQL(cmd);
                przeladujBazeDanych();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }
        }

        private void usuńToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem item = listView1.SelectedItems[0];
            string cmd = "DELETE FROM ludzie WHERE imie='" + item.SubItems[0].Text.ToString() + "' AND adres='"+item.SubItems[1].Text.ToString() + "' AND mail='"+item.SubItems[2].Text.ToString()+"'";
            SQL(cmd);
            listView1.Items.Remove(item);
            przeladujBazeDanych();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // try
           // {
                ListViewItem item = listView1.SelectedItems[0];
                imieBox.Text = item.SubItems[0].Text.ToString();
                adresBox.Text = item.SubItems[1].Text.ToString();
                emailBox.Text = item.SubItems[2].Text.ToString();
                string data = item.SubItems[3].Text.ToString();
                dateTimePicker1.Value = DateTime.Parse(data);
                dodatkoweBox.Text = item.SubItems[4].Text.ToString();
           // }
           // catch (Exception ex)
           // { MessageBox.Show(ex.Message.ToString()); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ListViewItem item = listView1.SelectedItems[0];
            string cmd = "DELETE FROM ludzie WHERE imie='" + item.SubItems[0].Text.ToString() + "' AND adres='" + item.SubItems[1].Text.ToString() + "' AND mail='" + item.SubItems[2].Text.ToString() + "'";
            SQL(cmd);
            listView1.Items.Remove(item);
            przeladujBazeDanych();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ListViewItem item = listView1.SelectedItems[0];
            item.SubItems[0].Text = imieBox.Text;
            item.SubItems[1].Text = adresBox.Text;
            item.SubItems[2].Text = emailBox.Text;
            item.SubItems[3].Text = dateTimePicker1.Value.ToString();
            item.SubItems[4].Text = dodatkoweBox.Text;

            string cmd = "UPDATE `ludzie` SET `adres`='"+adresBox.Text+ "',`mail`='" + emailBox.Text + ",`dodatkowe`='" + dodatkoweBox.Text + " WHERE 'imie' = '" + imieBox.Text+"' AND 'urodziny' = '"+dateTimePicker1.Value.ToString()+"'";
            SQL(cmd);
            przeladujBazeDanych();
        }

      /*public static bool poprawnoscMaila(string email)
        {
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }*/
    }
}
