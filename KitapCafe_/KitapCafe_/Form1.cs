using DataAccess.Concrete;
using System.Drawing.Printing;
using Timer = System.Windows.Forms.Timer;
using Entities.Concrete;

namespace KitapCafe_
{
    public partial class Form1 : Form
    {
        private KitapCafeContext _context;
        private Timer _timer;
        public Form1()
        {
            InitializeComponent();
            _context = new KitapCafeContext();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            _timer = new Timer();
            _timer.Interval = 5000;
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            var wifiPasswords = _context.WifiPasswords.ToList();


            dataGridView1.Rows.Clear();


            foreach (var wifi in wifiPasswords)
            {
                dataGridView1.Rows.Add(wifi.Id, wifi.Password);
            }
        }
        private void PrintPassword(string password)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += (sender, e) =>
            {
                Image logo = Image.FromFile("D:\\.NET Framework\\nArchitecture-master\\KitapCafe_\\KitapCafe_\\assets\\images\\narven_logo.png");

                float logoWidth = 700;
                float logoHeight = 250;
                float logoX = 100;
                float logoY = 30;
                
                using (Bitmap resizedLogo = new Bitmap(logo, new Size((int)logoWidth, (int)logoHeight)))
                {
                    e.Graphics.DrawImage(resizedLogo, new PointF(logoX, logoY));
                }
                Font font = new Font("Arial", 20);
                Brush brush = Brushes.Black;
                float spacing = 30;
                float textX = logoX;
                float textY = logoY + logo.Height + spacing;
                e.Graphics.DrawString($"Þifre: {password}", font, brush, new PointF(textX, textY));

            };
            printDocument.Print();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (var context = new KitapCafeContext())
            {
                var passwordRecord = context.WifiPasswords
                    .OrderBy(p => p.Id)
                    .FirstOrDefault();
                if (passwordRecord != null)
                {
                    PrintPassword(passwordRecord.Password);

                    context.WifiPasswords.Remove(passwordRecord);
                    context.SaveChanges();

                    LoadData();
                }
                else
                {
                    MessageBox.Show("Þifreniz kalmadý.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            using (var context = new KitapCafeContext())
            {
                int sayi = int.Parse(textBox1.Text);
                int passwordSayi = context.WifiPasswords.Count();
                
                for (int i = 0; sayi > i; i++)
                {
                    if (passwordSayi < 500)
                    {
                        string newPassword = GenerateRandomPassword();

                        var newPasswordRecord = new WifiPassword
                        {
                            Password = newPassword
                        };

                        context.WifiPasswords.Add(newPasswordRecord);
                        context.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Yeterince Þifre Mevcut");
                        break;
                    }
                }
                LoadData();
            }
        }
        static string GenerateRandomPassword()
        {
            Random random = new Random();
            char[] password = new char[8];

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            for (int a = 0; a < 8; a++)
            {
                password[a] = chars[random.Next(chars.Length)];
            }
            return new string(password);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            int passwordCount = GetPasswordCount();
            label3.Text = $"Kalan Þifre Sayýnýz:  {passwordCount} ";
        }
        private int GetPasswordCount()
        {
            using (var context = new KitapCafeContext())
            {
                return context.WifiPasswords.Count();
            }
        }
    }
}