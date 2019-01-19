using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteMouseClient
{
    public partial class Form1 : Form
    {
        UdpClient U;
        IPEndPoint E;
        public bool m_right = false;
        public bool m_left = false;
        public static int prevX = 0;
        public static int prevY = 0;
        public bool start = false;

        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();
            Cursor.Position = new Point(Location.X + this.Bounds.Width / 2, Location.Y + this.Bounds.Height / 2);
            this.MouseMove += Form1_MouseMove;
            this.MouseDown += Form1_MouseDown;
            this.MouseUp += Form1_MouseUp;
            this.MouseLeave += Form1_MouseLeave;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            label1.Visible = false;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (start == false) return;
            byte[] toBytes = Encoding.ASCII.GetBytes("K;up;" + e.KeyValue);
            U.Send(toBytes, toBytes.Count(), E);
            System.Diagnostics.Debug.Print("K;up;" + e.KeyValue);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (start == false) return;
            if (e.KeyCode.ToString() == "F4")
            {
                label1.Visible = false;
                btnStart.Text = "Iniciar";
                btnStart.Enabled = true;
                start = false;
            }
            byte[] toBytes = Encoding.ASCII.GetBytes("K;down;" + e.KeyValue);
            U.Send(toBytes, toBytes.Count(), E);
            System.Diagnostics.Debug.Print("K;down;" + e.KeyValue);
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            if (start == false) return;
            Cursor.Position = new Point(Location.X + this.Bounds.Width / 2, Location.Y + this.Bounds.Height / 2);
            prevX = 0;
            prevY = 0;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (start == false) return;
            byte[] toBytes = Encoding.ASCII.GetBytes("M;0;0;u" + e.Button);
            U.Send(toBytes, toBytes.Count(), E);
            System.Diagnostics.Debug.Print("M;0;0;u" + e.Button);
            //if (e.Button == MouseButtons.Left) m_left = false;
            //else if (e.Button == MouseButtons.Right) m_right = false;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (start == false) return;
            byte[] toBytes = Encoding.ASCII.GetBytes("M;0;0;d" + e.Button);
            U.Send(toBytes, toBytes.Count(), E);
            System.Diagnostics.Debug.Print("M;0;0;d" + e.Button);
            //if (e.Button == MouseButtons.Left) m_left = true;
            //else if (e.Button == MouseButtons.Right) m_right = true;
            //if (m_left && m_right)
            //{
            //    Application.Exit();
            //}
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (start == false) return;
            int xp = e.X - (this.Bounds.Width / 2) + 8;//offset de 8 borda
            int yp = e.Y - (this.Bounds.Height / 2) + 30;//offset de 30 borda menu

            if (xp < -50 || xp > +50 || yp < -50 || yp > 50)
            {
                Cursor.Position = new Point(Location.X + this.Bounds.Width / 2, Location.Y + this.Bounds.Height / 2);
                prevX = 0;
                prevY = 0;
                xp = 0;
                yp = 0;
            }
            else
            {
                int X;
                int Y;
                if (prevX == xp)
                {
                    X = 0;
                }
                else if (prevX > xp)
                {
                    X = xp - prevX;
                    prevX = xp;
                }
                else
                {
                    X = xp - prevX;
                    prevX = xp;
                }

                if (prevY == yp)
                {
                    Y = 0;
                }
                else if (prevY > yp)
                {
                    Y = yp - prevY;
                    prevY = yp;
                }
                else
                {
                    Y = yp - prevY;
                    prevY = yp;
                }
                byte[] toBytes = Encoding.ASCII.GetBytes("M;" + X + ";" + Y + ";null");
                U.Send(toBytes, toBytes.Count(), E);
                System.Diagnostics.Debug.Print("X=" + X + ";Y=" + Y);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(File.Exists(Application.StartupPath + @"\IP.txt")){
                string[] settings = File.ReadAllLines(Application.StartupPath + @"\IP.txt");

                radioButton1.Text = settings[0].Substring(0, settings[0].IndexOf("="));
                radioButton1.Tag = Regex.Match(settings[0], "=(.*);").Groups[1].Value;

                radioButton2.Text = settings[1].Substring(0, settings[1].IndexOf("="));
                radioButton2.Tag = Regex.Match(settings[1], "=(.*);").Groups[1].Value;

                radioButton3.Text = settings[2].Substring(0, settings[2].IndexOf("="));
                radioButton3.Tag = Regex.Match(settings[2], "=(.*);").Groups[1].Value;
            }
            else
            {
                label1.Visible = true;
                label1.Text = "Falta arquivo de configuração. Contate filipeso@furnas.com.br";
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            RadioButton checkedButton = this.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            System.Diagnostics.Debug.Print("IP=" + checkedButton.Tag.ToString());
            E = new IPEndPoint(IPAddress.Parse(checkedButton.Tag.ToString()), 27051);
            U = new UdpClient();
            start = true;
            btnStart.Text = "Conectado";
            label1.Visible = true;
            label1.Text = "Pressione \"F4\" para desconectar";
            btnStart.Enabled = false;
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            if(File.Exists(Application.StartupPath + @"\IP.txt"))
            {
                Process.Start(Application.StartupPath + @"\IP.txt");
            }
        }
    }
}
