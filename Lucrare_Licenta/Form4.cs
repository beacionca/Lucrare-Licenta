using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lucrare_Licenta
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mail = textBox1.Text;
            string nume = textBox2.Text;
            string prenume = textBox3.Text;
            string parola = textBox4.Text;
            string specializare = listBox1.SelectedItem.ToString();
            string anul = listBox2.SelectedItem.ToString();
            string seria = listBox3.SelectedItem.ToString();
            string grupa = listBox4.SelectedItem.ToString();
            string rol = "student";

            string minuscule = "abcdefghijklmnopqrstuvwxyz";
            string majuscule = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string cifre = "0123456789";
            string caractere_speciale = "!@#$%^&*()-_+={}:;,.?";

            string pattern = @"\b[A-Z][a-z]*(?![0-9])(\s+[A-Z][a-z]*(?![0-9]))*\b";

            string connect = @"Data Source=DESKTOP-GEVNFAS;Initial Catalog=GRAPHIX;Integrated Security=True";
            SqlConnection cnn = new SqlConnection(connect);
            cnn.Open();

            if ((textBox1.Text == "") || (textBox2.Text == "") || (textBox3.Text == "") || (textBox4.Text == ""))
                MessageBox.Show("Completati toate campurile!");
            else if (!textBox1.Text.EndsWith("@student.upt.ro"))
            {
                MessageBox.Show("Introduceti un e-mail corespunzator universitatii!", "ATENTIE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                return;
            }
            else if (!Regex.IsMatch(textBox2.Text, pattern))
            {
                MessageBox.Show("Nume invalid", "ATENTIE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Focus();
                return;
            }
            else if (!Regex.IsMatch(textBox3.Text, pattern))
            {
                MessageBox.Show("Prenume invalid", "ATENTIE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox3.Focus();
                return;
            }
            else if (!textBox4.Text.Any(char.IsUpper))
            {
                MessageBox.Show("Parola invalida - introduceti cel putin o litera mare", "ATENTIE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox4.Focus();
                return;
            }
            else if (!textBox4.Text.Any(char.IsLower))
            {
                MessageBox.Show("Parola invalida - introduceti cel putin o litera mica", "ATENTIE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox4.Focus();
                return;
            }
            else if (!textBox4.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Parola invalida - introduceti cel putin o cifra", "ATENTIE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox4.Focus();
                return;
            }
            else if (!textBox4.Text.Any(char.IsSymbol))
            {
                MessageBox.Show("Parola invalida - introduceti cel putin un simbol", "ATENTIE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox4.Focus();
                return;
            }
            else if (textBox4.Text.Length < 8)
            {
                MessageBox.Show("Parola invalida - introduceti cel putin 8 caractere", "ATENTIE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox4.Focus();
                return;
            }
            string sql1 = "select COUNT(*) from Evidenta_Academica where [e-mail]= @mail and rol= @rol";
            SqlCommand sc = new SqlCommand(sql1, cnn);
            sc.Parameters.AddWithValue("@mail", mail);
            sc.Parameters.AddWithValue("@rol", rol);
            int rezultatInterogare = Convert.ToInt32(sc.ExecuteScalar());
            sc.ExecuteNonQuery();
            cnn.Close();
            this.DialogResult = DialogResult.OK;
            this.Close();

            if (rezultatInterogare > 0)
            {
                string connect2 = @"Data Source=DESKTOP-GEVNFAS;Initial Catalog=GRAPHIX;Integrated Security=True";
                SqlConnection cnn2 = new SqlConnection(connect2);
                cnn2.Open();
                string sql2 = "insert into Informatii_Utilizatori([e-mail], [parola], [nume], [prenume], [rol], [specializare], [an], [serie], [grupa]) values (@mail, @parola, @nume, @prenume, @rol, @specializare, @an, @serie, @grupa)";
                SqlCommand sc2 = new SqlCommand(sql2, cnn2);
                sc2.Parameters.AddWithValue("@mail", mail);
                sc2.Parameters.AddWithValue("@parola", parola);
                sc2.Parameters.AddWithValue("@nume", nume);
                sc2.Parameters.AddWithValue("@prenume", prenume);
                sc2.Parameters.AddWithValue("@rol", rol);
                sc2.Parameters.AddWithValue("@specializare", specializare);
                sc2.Parameters.AddWithValue("@an", anul);
                sc2.Parameters.AddWithValue("@serie", seria);
                sc2.Parameters.AddWithValue("@grupa", grupa);
                sc2.ExecuteNonQuery();
                cnn2.Close();
                this.DialogResult = DialogResult.OK;
                this.Close();
                MessageBox.Show("Contul a fost creat", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string connect3 = @"Data Source=DESKTOP-GEVNFAS;Initial Catalog=GRAPHIX;Integrated Security=True";
                SqlConnection cnn3 = new SqlConnection(connect3);
                cnn3.Open();
                string sql3 = "insert into Situatie_Studenti([e-mail], [nume], [prenume], [specializare], [an], [serie], [grupa], [nota_grafuri], [nota_arbori], [medie]) values (@mail, @nume, @prenume, @specializare, @an, @serie, @grupa, @nota_grafuri, @nota_arbori, @medie)";
                SqlCommand sc3 = new SqlCommand(sql3, cnn3);
                sc3.Parameters.AddWithValue("@mail", mail);
                sc3.Parameters.AddWithValue("@nume", nume);
                sc3.Parameters.AddWithValue("@prenume", prenume);
                sc3.Parameters.AddWithValue("@specializare", specializare);
                sc3.Parameters.AddWithValue("@an", anul);
                sc3.Parameters.AddWithValue("@serie", seria);
                sc3.Parameters.AddWithValue("@grupa", grupa);
                sc3.Parameters.AddWithValue("@nota_grafuri", 0);
                sc3.Parameters.AddWithValue("@nota_arbori", 0);
                sc3.Parameters.AddWithValue("@medie", 0);
                sc3.ExecuteNonQuery();
                cnn3.Close();
                this.DialogResult = DialogResult.OK;
                this.Close();
                Application.Restart();
            }
            else
            {
                MessageBox.Show("Contul nu a fost gasit in baza de date", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem.ToString() == "Info")
            {
                listBox2.Items.Add("1");
                listBox2.Items.Add("2");
                listBox2.Items.Add("3");
                listBox3.Items.Add("1");
                listBox4.Items.Add("1.1");
                listBox4.Items.Add("1.2");
                listBox4.Items.Add("2.1");
                listBox4.Items.Add("2.2");
                listBox4.Items.Add("3.1");
                listBox4.Items.Add("3.2");
            }
            else
            {
                listBox2.Items.Add("1");
                listBox2.Items.Add("2");
                listBox2.Items.Add("3");
                listBox2.Items.Add("4");
                listBox3.Items.Add("1");
                listBox3.Items.Add("2");
                listBox4.Items.Add("1.1");
                listBox4.Items.Add("1.2");
                listBox4.Items.Add("2.1");
                listBox4.Items.Add("2.2");
                listBox4.Items.Add("3.1");
                listBox4.Items.Add("3.2");
                listBox4.Items.Add("4.1");
                listBox4.Items.Add("4.2");
                listBox4.Items.Add("5.1");
                listBox4.Items.Add("5.2");
                listBox4.Items.Add("6.1");
                listBox4.Items.Add("6.2");
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4.PasswordChar = '*';
        }
    }
}
