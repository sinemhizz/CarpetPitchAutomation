using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Data.SqlClient;

namespace hali_saha_otomasyonu
{
    public partial class Form_Rezervasyon : Form
    {
        SqlConnection baglanti;
        SqlCommand komut;
        SqlDataAdapter da;
        DataTable dt;
        public Form_Rezervasyon()
        {
            InitializeComponent();
        }


        PictureBox p; 
        string secilen = ""; 

        void veritabani()
        {
            baglanti = new SqlConnection("Data Source=DESKTOP-IVAC8KG;Initial Catalog=h_saha;Integrated Security=True");
            baglanti.Open();
            da = new SqlDataAdapter("select * from randevu", baglanti);
            baglanti.Close();
        }

        private void Form_Rezervasyon_Load(object sender, EventArgs e) 
        {
            veritabani();
            baglanti.Open();
            komut = new SqlCommand("select saha_id,saha_adi from saha", baglanti);
            da = new SqlDataAdapter(komut);
            dt = new DataTable();
            da.Fill(dt);

            DataRow dr = dt.NewRow();
            dr["saha_id"] = 0;
            dr["saha_adi"] = "Saha Seçimi";
            dt.Rows.InsertAt(dr, 0);

            comboBox_Saha.DataSource = dt;
            comboBox_Saha.DisplayMember = "saha_adi";
            comboBox_Saha.ValueMember = "saha_id";

            baglanti.Close();
        }

        void temizle() 
        {
            textBox_adsoyad.Text = "";
            textBox_telefon.Text = "";
            textBox_zaman.Text = "";
            textBox_ucret.Text = "";
            richTextBox1.Text = "";
        }

        private void comboBox_Saha_SelectedIndexChanged(object sender, EventArgs e) 
        {
            foreach (Control item in panel2.Controls)  
            {
                if (item is PictureBox)
                {
                    item.BackColor = Color.Green;
                }
            }
            temizle(); 

            if (comboBox_Saha.SelectedIndex != 0) 
            {
                
                baglanti.Open();
                komut = new SqlCommand("select randevu_id from randevu", baglanti);
                da = new SqlDataAdapter(komut);
                dt = new DataTable();
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    foreach (Control item in panel2.Controls)
                    {
                        if (item is PictureBox)
                        {
                            if (dr["randevu_id"].ToString() == comboBox_Saha.SelectedValue.ToString() + item.Name.ToString())
                            {
                                item.BackColor = Color.Red;
                            }
                        }
                    }
                }
                baglanti.Close();

                baglanti.Open();
                komut = new SqlCommand("select randevu_id from abone", baglanti);
                da = new SqlDataAdapter(komut);
                dt = new DataTable();
                SqlDataReader read= komut.ExecuteReader();
                while (read.Read())
                {
                    foreach (Control item in panel2.Controls)
                    {
                        if (item is PictureBox)
                        {
                            if (read["randevu_id"].ToString() == comboBox_Saha.SelectedValue.ToString() + item.Name.ToString())
                            {
                                item.BackColor = Color.Yellow;
                            }
                        }
                    }
                }
            }
            
            baglanti.Close();
        }


        private void pictureBox105_MouseHover(object sender, EventArgs e) 
        {
            PictureBox p1 = sender as PictureBox;
            p1.BorderStyle = BorderStyle.FixedSingle;
        }

        private void pictureBox105_MouseLeave(object sender, EventArgs e) 
        {
            PictureBox p1 = sender as PictureBox;
            p1.BorderStyle = BorderStyle.None;
        }
        private void pbox_Click(object sender, EventArgs e) 
        {
            temizle();
            p = sender as PictureBox;
            secilen = p.Name.ToString();
            textBox_zaman.Text = secilen;
            if (comboBox_Saha.SelectedIndex != 0)
            {
                
                if (p.BackColor == Color.Red ) 
                {
                    button_abone.Enabled = false;
                    button_aboneguncelle.Enabled = false;
                    button_aboneiptal.Enabled = false;
                    button_rezervasyonguncelle.Enabled = true;
                    button_rezervasyonsil.Enabled = true;
                    button_rezervasyon.Enabled = false;
                    baglanti.Open();
                    komut = new SqlCommand("select * from randevu where randevu_id=@pname", baglanti);                
                    komut.Parameters.Add("@pname", comboBox_Saha.SelectedValue.ToString() + p.Name.ToString());
                    da = new SqlDataAdapter(komut);
                    dt = new DataTable();
                    SqlDataReader dr = komut.ExecuteReader();
                    while (dr.Read())
                    {
                        textBox_ucret.Text = dr["ucret"].ToString();
                        textBox_adsoyad.Text = dr["adsoyad"].ToString();
                        textBox_telefon.Text = dr["telefon"].ToString();
                        textBox_zaman.Text = dr["zaman"].ToString();
                        richTextBox1.Text = dr["r_not"].ToString();
                    }
                    baglanti.Close();
                }
                else if(p.BackColor==Color.Yellow)
                {
                    button_aboneguncelle.Enabled = true;
                    button_aboneiptal.Enabled = true;
                    button_abone.Enabled = false;
                    button_rezervasyonguncelle.Enabled = false;
                    button_rezervasyonsil.Enabled = false;
                    button_rezervasyon.Enabled = false;
                    baglanti.Open();
                    komut = new SqlCommand("select * from abone where randevu_id=@pname", baglanti);
                    komut.Parameters.Add("@pname", comboBox_Saha.SelectedValue.ToString() + p.Name.ToString());
                    da = new SqlDataAdapter(komut);
                    dt = new DataTable();
                    SqlDataReader dr = komut.ExecuteReader();
                    while (dr.Read())
                    {
                        textBox_ucret.Text = dr["ucret"].ToString();
                        textBox_adsoyad.Text = dr["adsoyad"].ToString();
                        textBox_telefon.Text = dr["telefon"].ToString();
                        textBox_zaman.Text = dr["zaman"].ToString();
                        richTextBox1.Text = dr["r_not"].ToString();
                    }
                    baglanti.Close();
                }
                else
                {
                    button_rezervasyon.Enabled = true; 
                    button_abone.Enabled = true;
                    button_rezervasyonguncelle.Enabled = false;
                    button_rezervasyonsil.Enabled = false;
                    button_aboneiptal.Enabled = false;
                    button_aboneguncelle.Enabled = false;
                }
            }
        }

        private void Button_Rezervasyon_Click(object sender, EventArgs e) 
        {
            if (textBox_adsoyad.Text != "" || textBox_telefon.Text != "" || textBox_ucret.Text != "" || richTextBox1.Text != "") 
            {
                baglanti.Open(); 
                komut = new SqlCommand("insert into randevu(randevu_id,adsoyad,telefon,zaman,ucret,r_not)values(@pname,@adsoyad,@telefon,@zaman,@ucret,@r_not)", baglanti);
                komut.Parameters.Add("@pname", comboBox_Saha.SelectedValue.ToString() + secilen);
                komut.Parameters.Add("@ucret", textBox_ucret.Text);
                komut.Parameters.Add("@adsoyad", textBox_adsoyad.Text);
                komut.Parameters.Add("@zaman", textBox_zaman.Text);
                komut.Parameters.Add("@telefon", textBox_telefon.Text);
                komut.Parameters.Add("@r_not", richTextBox1.Text);
                komut.ExecuteReader();
                p.BackColor = Color.Red;
                MessageBox.Show(secilen + " Rezervasyon Yapıldı");
                button_rezervasyon.Enabled = false;
                baglanti.Close();

            }
            else
            {
                MessageBox.Show("Boş alan bırakmayınız. ");
            }
            temizle();
        }

        private void button_rezervasyonguncelle_Click(object sender, EventArgs e) 
        {
            if (textBox_adsoyad.Text != "" || textBox_telefon.Text != "" || textBox_ucret.Text != "" || richTextBox1.Text != "" )
            {
                baglanti.Open();
                komut = new SqlCommand("update randevu set adsoyad=@adsoyad,telefon=@telefon,ucret=@ucret,r_not=@r_not where randevu_id=@pname", baglanti);
                komut.Parameters.Add("@pname", comboBox_Saha.SelectedValue.ToString() + p.Name.ToString());
                komut.Parameters.Add("@ucret", textBox_ucret.Text);
                komut.Parameters.Add("@adsoyad", textBox_adsoyad.Text);
                komut.Parameters.Add("@telefon", textBox_telefon.Text);
                komut.Parameters.Add("@zaman", textBox_zaman.Text);
                komut.Parameters.Add("@r_not", richTextBox1.Text);
                komut.ExecuteReader();
                p.BackColor = Color.Red;
                MessageBox.Show(secilen + " Rezervasyon Güncellendi");
                button_rezervasyonguncelle.Enabled = false;
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Boş alan bırakmayınız. ");
            }
            temizle();
        }

        private void button1_Click(object sender, EventArgs e) 
        {
            DialogResult cevap = MessageBox.Show("Haftayı temizlemek istiyor musunuz ? ", "HAFTAYI BİTİR", MessageBoxButtons.YesNo);
            if (cevap == DialogResult.Yes & p.BackColor==Color.Red)
            {
                    baglanti.Open();
                    komut = new SqlCommand("delete from randevu", baglanti);
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    comboBox_Saha.SelectedIndex = 0;
               
            }
        }

        private void Form_Rezervasyon_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }



        private void button_rezervasyonsil_Click(object sender, EventArgs e)
        {
            string sorgu = "Delete from randevu where randevu_id=@pname";
            komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@pname", comboBox_Saha.SelectedValue.ToString() + p.Name.ToString());
            baglanti.Open();
            komut.ExecuteNonQuery();
            p.BackColor = Color.Green;
            baglanti.Close();
            MessageBox.Show("Kayıt Silindi.");
        }


        private void button_abone_Click(object sender, EventArgs e)
        {
            if (textBox_adsoyad.Text != "" || textBox_telefon.Text != "" || textBox_ucret.Text != "") 
            {
                baglanti.Open();
                komut = new SqlCommand("insert into abone(randevu_id,adsoyad,telefon,zaman,ucret,r_not)values(@pname,@adsoyad,@telefon,@zaman,@ucret,@r_not)", baglanti);
                komut.Parameters.Add("@pname", comboBox_Saha.SelectedValue.ToString() + secilen);
                komut.Parameters.Add("@ucret", textBox_ucret.Text);
                komut.Parameters.Add("@adsoyad", textBox_adsoyad.Text);
                komut.Parameters.Add("@zaman", textBox_zaman.Text);
                komut.Parameters.Add("@telefon", textBox_telefon.Text);
                komut.Parameters.Add("@r_not", richTextBox1.Text);
                komut.ExecuteReader();
                p.BackColor = Color.Yellow;                
                MessageBox.Show(secilen + " Abone Eklendi");
                button_rezervasyon.Enabled = false;               
                baglanti.Close();

            }
            else
            {
                MessageBox.Show("Boş alan bırakmayınız. ");
            }
            temizle();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button_aboneguncelle_Click(object sender, EventArgs e)
        {
            if (textBox_adsoyad.Text != "" || textBox_telefon.Text != "" || textBox_ucret.Text != "" || richTextBox1.Text != "")
            {
                baglanti.Open();
                komut = new SqlCommand("update abone set adsoyad=@adsoyad,telefon=@telefon,ucret=@ucret,r_not=@r_not where randevu_id=@pname", baglanti);
                komut.Parameters.Add("@pname", comboBox_Saha.SelectedValue.ToString() + p.Name.ToString());
                komut.Parameters.Add("@ucret", textBox_ucret.Text);
                komut.Parameters.Add("@adsoyad", textBox_adsoyad.Text);
                komut.Parameters.Add("@telefon", textBox_telefon.Text);
                komut.Parameters.Add("@zaman", textBox_zaman.Text);
                komut.Parameters.Add("@r_not", richTextBox1.Text);
                komut.ExecuteReader();
                p.BackColor = Color.Yellow;
                MessageBox.Show(secilen + " Abone Güncellendi");
                button_rezervasyonguncelle.Enabled = false;
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Boş alan bırakmayınız. ");
            }
            temizle();
        }

        private void button_aboneiptal_Click(object sender, EventArgs e)
        {
            string sonuc = "Delete from abone where randevu_id=@pname";
            komut = new SqlCommand(sonuc, baglanti);
            komut.Parameters.AddWithValue("@pname", comboBox_Saha.SelectedValue.ToString() + p.Name.ToString());
            baglanti.Open();
            komut.ExecuteNonQuery();
            p.BackColor = Color.Green;
            baglanti.Close();
            MessageBox.Show("Abone Silindi.");
        }
    }
}
