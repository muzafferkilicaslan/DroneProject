using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Data.SqlClient;
 


namespace DroneProject
{

    public partial class Form1 : Form
    {
        string command;
        readonly UDPSocket s = new UDPSocket();
        readonly UDPSocket c = new UDPSocket();
        readonly List<Panel> listPanel = new List<Panel>();
        readonly int[,] droneBaslangic = { { 20, 20 } };

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            this.IsMdiContainer = true;
        }

        // SQL BAĞLANTI-SQL CONNECTION

        static readonly string constring= "Data Source="+"" //add here your sql server connection string
            +";Initial Catalog=project;Integrated Security=True";
        readonly SqlConnection connect = new SqlConnection(constring);
        readonly SqlConnection connect2 = new SqlConnection(constring);

        int position = 0;
        readonly SerialPort SerialPort = new SerialPort();

        private void Form1_Load(object sender, EventArgs e)
        {
            var ports = SerialPort.GetPortNames(); //Seri portları diziye ekleme
            foreach (var port in ports)
                comboBox1.Items.Add(port);
            //TrackBar ayarları
            trackBar1.Minimum = 1;
            trackBar1.Maximum = 180;
        }

        private void WriteAngle(int value)
        {
            try
            {
                if (SerialPort.IsOpen)
                {
                    SerialPort.WriteLine(value.ToString()); //Değeri port üzerinden gönder
                    label2.Text = "Açı: " + value.ToString() + "°"; //Güncel değeri label2'ye yaz
                }
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message, "Hata"); //Hata mesajı
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SerialPort.PortName = comboBox1.SelectedItem.ToString();
            SerialPort.Open();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (SerialPort.IsOpen) SerialPort.Close(); //Seri port açıksa kapat
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!SerialPort.IsOpen)
                {
                    /* Seri Port Ayarları */
                    SerialPort.PortName = comboBox1.Text;
                    SerialPort.BaudRate = 9600;
                    SerialPort.Parity = Parity.None;
                    SerialPort.DataBits = 8;
                    SerialPort.StopBits = StopBits.One;
                    SerialPort.Open(); //Seri portu aç
                    label3.Text = "Bağlantı Sağlandı.";
                    label3.ForeColor = System.Drawing.Color.Green;
                    button1.Text = "KES"; //Buton1 yazısını değiştir
                }
                else
                {
                    label3.Text = "Bağlantı Kesildi.";
                    label3.ForeColor = System.Drawing.Color.Red;
                    button1.Text = "BAĞLAN"; //Buton1 yazısını değiştir
                    SerialPort.Close(); //Seri portu kapa
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata"); //Hata mesajı
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            position = 30;
            WriteAngle(position);
            label2.Text = "Açı: " + position.ToString() + "°"; //Güncel değeri label2'ye yaz
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            position = 90;
            WriteAngle(position);
            label2.Text = "Açı: " + position.ToString() + "°"; //Güncel değeri label2'ye yaz
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            position = 120;
            WriteAngle(position);
            label2.Text = "Açı: " + position.ToString() + "°"; //Güncel değeri label2'ye yaz
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            position = 180;
            WriteAngle(position);
            label2.Text = "Açı: " + position.ToString() + "°"; //Güncel değeri label2'ye yaz
        }

        private void TrackBar1_Scroll_1(object sender, EventArgs e)
        {
            position = trackBar1.Value;
            WriteAngle(position);
            label2.Text = "Açı: " + position.ToString() + "°"; //Güncel değeri label2'ye yaz
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            listPanel.Add(panel1);
            listPanel.Add(panel2);           
            listPanel.Add(panel6);
            listPanel.Add(panel5);
            
            connect.Open();
            SqlCommand getir = new SqlCommand("Select isim from projectTable", connect);
            SqlDataReader oku = getir.ExecuteReader();
            while (oku.Read())
            {
                comboBox2.Items.Add(oku["isim"].ToString());
            }

            connect.Close();

            label24.Text = droneBaslangic[0, 0].ToString();
            label25.Text = droneBaslangic[0, 1].ToString();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            listPanel[1].BringToFront();
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            listPanel[0].BringToFront();
        }

        private void Button25_Click(object sender, EventArgs e)
        {
            listPanel[2].BringToFront();
            
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            int pos;
            pos = position;
            pos += 5;
            position = pos;
            label2.Text = "Açı: " + position.ToString() + "°"; //Güncel değeri label2'ye yaz
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            if (position != 0)
            {
                int pos;
                pos = position;
                pos -= 5;
                position = pos;
                label2.Text = "Açı: " + position.ToString() + "°"; //Güncel değeri label2'ye yaz
            }
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            try
            {
                c.Client("192.168.10.1", 8889);
                s.Server("0.0.0.0", 8890);
                label4.Text = "Bağlantı Sağlandı.";
                label4.ForeColor = System.Drawing.Color.Green;
                //button1.Text = "KES"; //Buton1 yazısını değiştir
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata"); //Hata mesajı
            }
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            c.Disconnect();
        }

        
        private void Button12_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            label5.Text = btn.Text;
            c.Send(btn.Text);	     
                       
        }

        private void Button17_Click(object sender, EventArgs e)
        {
            string com = textBox1.Text;
            c.Send(com);
            Thread.Sleep(2000);         
            command = textBox1.Text;
            label5.Text = command;                       
        }

        private void Button24_Click(object sender, EventArgs e)
        {          
            listPanel[2].BringToFront();
            listPanel[3].BringToFront();
        }        
 
        private void DroneHazırla(object sender, EventArgs e)
        {
            c.Send("command");
            c.Send("takeoff");
        }

        private void Kayitbitir_btn(object sender, EventArgs e)
        {
            try
            {
                if (connect.State == ConnectionState.Closed)
                    connect.Open();
                
                //KAYIT EKLEME
                string kayit = "insert into projectTable (isim,telefon,il,ilce,mahalle,sokak,bina,tip) values (@isim,@telefon,@il,@ilce,@mahalle,@sokak,@bina,@tip)";
                SqlCommand komut = new SqlCommand(kayit, connect);

                komut.Parameters.AddWithValue("@isim", textBox3.Text);
                komut.Parameters.AddWithValue("@telefon", textBox4.Text);
                komut.Parameters.AddWithValue("@il", comboBox6.SelectedItem.ToString());
                komut.Parameters.AddWithValue("@ilce", comboBox5.SelectedItem.ToString());
                komut.Parameters.AddWithValue("@mahalle", comboBox4.SelectedItem.ToString());
                komut.Parameters.AddWithValue("@sokak", comboBox3.SelectedItem.ToString());
                komut.Parameters.AddWithValue("@bina", comboBox7.SelectedItem.ToString());
                komut.Parameters.AddWithValue("@tip", comboBox8.SelectedItem.ToString());
                komut.ExecuteNonQuery();
                connect.Close();
                MessageBox.Show("Kayıt Eklendi");

                //mevcut kayıtları silme

                connect2.Open();
                SqlCommand getiri = new SqlCommand("Select isim from projectTable", connect2);
                SqlDataReader okur = getiri.ExecuteReader();
                while (okur.Read())
                {
                    comboBox2.Items.Remove(okur["isim"].ToString());
                }
                connect2.Close();

                //Kayıt okuma
                connect.Open();
                SqlCommand getir = new SqlCommand("Select isim from projectTable", connect);
                SqlDataReader oku = getir.ExecuteReader();
                while (oku.Read())
                {                    
                    comboBox2.Items.Add(oku["isim"].ToString());
                }

                connect.Close();

            }
            catch (Exception hata)
            {
                MessageBox.Show("Hata Meydana Geldi " + hata.Message);
            }
        }

        

        private void Button28_Click(object sender, EventArgs e)
        {
            
            #region V2

            ////
            //BİLGİ YAZISI
            ////
            ////

            int[,] drone = { { 20, 20 } };
            int xGidis, yGidis;

            connect.Open();
            string name = comboBox2.SelectedItem.ToString();
            SqlCommand getir = new SqlCommand("Select bina, mahalle, sokak,tip , il, ilce from projectTable where isim = '" + name + "'", connect);
            SqlDataReader oku = getir.ExecuteReader();
            while (oku.Read())
            {
                label10.Text = oku["mahalle"].ToString() + " " + oku["sokak"].ToString() + " " + oku["bina"].ToString() + Environment.NewLine + oku["tip"].ToString() + Environment.NewLine + oku["il"].ToString() + "/" + oku["ilce"].ToString();
            }
            connect.Close();
            connect.Open();
            connect2.Open();
            
            ////
            //GPS YÖNLENDİRME
            ////

            SqlDataReader okur = getir.ExecuteReader();

            int x, y;
            while (okur.Read())
            {
                SqlCommand getiri = new SqlCommand("Select binaİsim, x, y from map where binaİsim = '" + okur["bina"].ToString() + "'", connect2);
                SqlDataReader okur2 = getiri.ExecuteReader();
                
                while (okur2.Read())
                {                    

                    string x2 = okur2["x"].ToString();
                    x = Int32.Parse(x2);
                    string y2 = okur2["y"].ToString();
                    y = Int32.Parse(y2);

                    xGidis = x - drone[0, 0];
                    yGidis = y - drone[0, 1];

                    MessageBox.Show("x: " + x + " ve y: " + y + "hedef "+xGidis+" "+ yGidis);

                    int ySag = yGidis*-1;
                    int ySol = yGidis;                    
                    if (xGidis > 0)
                    {
                        if (yGidis > 0)//4 ve 5. bina
                        {
                            Wait();                            
                            Command("go " + xGidis + " " + ySag + " 0 15");
                            drone[0, 0] += xGidis;
                            drone[0, 1] += yGidis;
                            label24.Text = drone[0, 0].ToString();
                            label25.Text = drone[0, 1].ToString();
                            MessageBox.Show("İşlem yapıldı.");
                            Command("down 40");
                            MessageBox.Show("İşlem yapıldı.");
                            Delivery();
                            MessageBox.Show("İşlem yapıldı.");
                            Command("up 40");
                            MessageBox.Show("İşlem yapıldı.");
                            Command("cw 180");
                            MessageBox.Show("İşlem yapıldı.");
                            Command("go " + xGidis + " " + ySag + " 0 15");
                            drone[0, 0] -= xGidis;
                            drone[0, 1] -= yGidis;
                            label24.Text = drone[0, 0].ToString();
                            label25.Text = drone[0, 1].ToString();
                            MessageBox.Show("İşlem yapıldı.");
                            Command("land");
                            MessageBox.Show("İşlem yapıldı.");
                        }
                        else //1. ve 2. bina
                        {
                            Wait();                            
                            Command("go " + xGidis + " 0 0 15");
                            drone[0, 0] += xGidis;
                            label24.Text = drone[0, 0].ToString();
                            MessageBox.Show("İşlem yapıldı.");
                            Command("down 40");
                            MessageBox.Show("İşlem yapıldı.");
                            Delivery();
                            MessageBox.Show("İşlem yapıldı.");
                            Command("up 40");
                            MessageBox.Show("İşlem yapıldı.");
                            Command("cw 180");
                            MessageBox.Show("İşlem yapıldı.");
                            Command("go " + xGidis + " 0 0 15");
                            drone[0, 0] -= xGidis;
                            label24.Text = drone[0, 0].ToString();
                            MessageBox.Show("İşlem yapıldı.");
                            Command("land");
                            MessageBox.Show("İşlem yapıldı.");                            
                        }
                    }
                    else if (yGidis > 0) //3.Bina
                    {
                        Wait();                        
                        Command("go 0 " + ySag + " 0 15");
                        drone[0, 1] += yGidis;
                        label25.Text = drone[0, 1].ToString();
                        MessageBox.Show("İşlem yapıldı.");
                        Command("down 40");
                        MessageBox.Show("İşlem yapıldı.");
                        Delivery();
                        MessageBox.Show("İşlem yapıldı.");
                        Command("up 40");
                        MessageBox.Show("İşlem yapıldı.");
                        //Komut("cw 180");
                        //MessageBox.Show("İşlem yapıldı.");
                        Command("go 0 " + ySol + " 0 15");
                        drone[0, 1] -= yGidis;
                        label25.Text = drone[0, 1].ToString();
                        MessageBox.Show("İşlem yapıldı.");
                        Command("land");
                        MessageBox.Show("İşlem yapıldı.");
                    }
                    else
                    {
                        MessageBox.Show("Hata!!!!");
                    }
                }
                connect2.Close();

                #endregion
            }
        }

        public void Delivery()
        {
            //WriteAngle(180);
            Thread.Sleep(4000);
            //WriteAngle();
        }
        public void Wait()
        {
            Thread.Sleep(3000);
        }

        public void Command(string x)
        {
            c.Send(x);
            Thread.Sleep(2500);
        }        
    }        
}