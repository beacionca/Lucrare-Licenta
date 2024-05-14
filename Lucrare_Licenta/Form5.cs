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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mail = textBox1.Text;
            string nume = textBox2.Text;
            string prenume = textBox3.Text;
            string parola = textBox4.Text;
            string rol = "profesor";

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
            else if (!textBox1.Text.EndsWith("@upt.ro"))
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
                sc2.Parameters.AddWithValue("@specializare", "");
                sc2.Parameters.AddWithValue("@an", "");
                sc2.Parameters.AddWithValue("@serie", "");
                sc2.Parameters.AddWithValue("@grupa", "");
                sc2.ExecuteNonQuery();
                cnn2.Close();
                this.DialogResult = DialogResult.OK;
                this.Close();
                MessageBox.Show("Contul a fost creat", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
            }
            else
            {
                MessageBox.Show("Contul nu a fost gasit in baza de date", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            //MessageBox.Show("Cont creat cu succes", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4.PasswordChar = '*';
        }
    }
}
