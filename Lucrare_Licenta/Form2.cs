using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lucrare_Licenta
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connect = @"Data Source=DESKTOP-GEVNFAS;Initial Catalog=GRAPHIX;Integrated Security=True";
            SqlConnection cnn = new SqlConnection(connect);
            cnn.Open();

            string mail = textBox1.Text;
            string parola = textBox2.Text;

            string sql1 = "select COUNT(*) from Informatii_Utilizatori where [e-mail]= @mail and parola= @parola";
            SqlCommand sc = new SqlCommand(sql1, cnn);
            sc.Parameters.AddWithValue("@mail", mail);
            sc.Parameters.AddWithValue("@parola", parola);
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
                string sql2 = "select rol from Informatii_Utilizatori where [e-mail]= @mail and parola= @parola";
                SqlCommand sc2 = new SqlCommand(sql2, cnn2);
                sc2.Parameters.AddWithValue("@mail", mail);
                sc2.Parameters.AddWithValue("@parola", parola);
                string rol_utilizator = sc2.ExecuteScalar()?.ToString();
                //MessageBox.Show(rol_utilizator);
                sc2.ExecuteNonQuery();
                cnn2.Close();
                this.DialogResult = DialogResult.OK;
                this.Close();
                if ((rol_utilizator.Replace(" ", "")).Equals("student"))
                {
                    //Form f = new Form();
                    //f.ShowDialog();
                }
                else
                {
                    //Form f = new Form();
                    //f.ShowDialog();
                }
                /*
                MessageBox.Show("Contul a fost creat", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
                */
            }
            else
            {
                string connect3 = @"Data Source=DESKTOP-GEVNFAS;Initial Catalog=GRAPHIX;Integrated Security=True";
                SqlConnection cnn3 = new SqlConnection(connect3);
                cnn3.Open();

                string sql3 = "select COUNT(*) from Informatii_Utilizatori where [e-mail]= @mail";
                SqlCommand sc3 = new SqlCommand(sql3, cnn3);
                sc3.Parameters.AddWithValue("@mail", mail);
                int rezultatInterogare_mail = Convert.ToInt32(sc3.ExecuteScalar());
                sc3.ExecuteNonQuery();
                cnn3.Close();
                this.DialogResult = DialogResult.OK;
                this.Close();

                if (rezultatInterogare_mail > 0)
                {
                    MessageBox.Show("Parola incorecta!\n", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Nu exista un cont creat cu acest e-mail!\n", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }
    }
}
