using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO.Ports;
using System.Data.SqlClient;

namespace SıcaklıkSensörü
{
    public partial class Form1 : Form
    {
        SqlConnection bag = new SqlConnection("Data Source=ULKUYONDEM\\SQLEXPRESS;Initial Catalog=sicaklik;Integrated Security=True");
        SqlCommand komut = new SqlCommand();
        DataTable tablo = new DataTable();
        //veritabanı bağlantısı
        string[] ports = SerialPort.GetPortNames(); //arduino porttan alına değer


        public Form1()
        {
            InitializeComponent();
            serialPort1.BaudRate = 9600;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
            timer1.Start();
            foreach (string port in ports)// verilerin alındığı portu combobox da gösterir
            {
                comboBox1.Items.Add(port);  
                comboBox1.SelectedIndex = 0;
            }
        }

        private void btn_baglan_Click(object sender, EventArgs e)
        {
        
            if (!serialPort1.IsOpen)
            {
                if (comboBox1.Text == "")
                    return;
                serialPort1.PortName = comboBox1.Text;
                try
                {
                    serialPort1.Open();
                    MessageBox.Show("Bağlantı Başarıyla Kuruldu");
                    timer1.Start();
                }
                catch (Exception ex)
                {
                    timer1.Stop();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btn_durdur_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            serialPort1.Close();
            MessageBox.Show("Bağlantı Sonlandırıldı");
        }

        private void btn_goster_Click(object sender, EventArgs e)
        {
          
                tablo.Clear();
                SqlDataAdapter adaptr = new SqlDataAdapter("SELECT sicaklik FROM sicaklik_tablosu", bag);//veritabanından bilgileri getirir
                adaptr.Fill(tablo);
                dataGridView1.DataSource = tablo;
            
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            try 
            {
                serialPort1.Write("a"); //arduino ve programon senkron çalısması gerketiğinden a tanımladık 
                int data = Convert.ToInt32(serialPort1.ReadLine());
                textBox1.Text = data.ToString();

                if (bag.State == ConnectionState.Closed) bag.Open();
                SqlCommand komut = new SqlCommand("INSERT INTO sicaklik_tablosu (sicaklik) VALUES(' " + textBox1.Text + " ' ) ", bag);
                komut.ExecuteNonQuery();
                bag.Close();
            }  
             catch(Exception ex)
            {
                 MessageBox.Show(ex.Message);
                 bag.Close();
             }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.ToLongTimeString();// sistem saaati
            label4.Text = DateTime.Now.ToShortDateString();// sistem tarihi
        }

        
    }
}
