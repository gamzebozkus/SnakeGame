﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yilan_Oyunu
{
    public partial class Form1 : Form
    {
        private Label _yilanKafasi;
        private int _yilanParcasiArasiMesafe = 2;
        private int _yilanParcasiSayisi;
        private int _yilanBoyutu = 20;
        private int _yemBoyutu = 20;
        private Label _yem;
        private Random _random;
        private HareketYonu _yon;


        public Form1()
        {
            InitializeComponent();
            _random = new Random();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            Sifirla();
        }

        private void YenidenBaslat()
        {
            label3.Text = "0";
            label5.Text = "0";
            this.pnl.Controls.Clear();
            Sifirla();
        }
        public void Sifirla()
        {
            pnl.Controls.Clear();
            _yilanParcasiSayisi = 0;
            YemOlustur();
            YeminYeriniDegistir();
            YilaniYerlestir();
            _yon = HareketYonu.Saga;
            timerYilanHareket.Enabled = true;
            timerSaat.Enabled = true;
        }
        private Label YilanParcasiOlustur(int locationX, int locationY)
        {
            _yilanParcasiSayisi++;
            Label lbl = new Label()
            {
                Name = "yılanParca" + _yilanParcasiSayisi,
                BackColor = Color.Red,
                Width = _yilanBoyutu,
                Height = _yilanBoyutu,
                Location = new Point(locationX, locationY)

            };
            this.pnl.Controls.Add(lbl);
            return lbl;
        }
        private void YilaniYerlestir()
        {
            _yilanKafasi = YilanParcasiOlustur(0, 0);
            _yilanKafasi.Text = ":";
            _yilanKafasi.TextAlign = ContentAlignment.MiddleCenter;
            _yilanKafasi.ForeColor = Color.White;
            var LocationX = (pnl.Width / 2) - (_yilanKafasi.Width / 2);
            var LocationY = (pnl.Height / 2) - (_yilanKafasi.Height / 2);
            _yilanKafasi.Location = new Point(LocationX, LocationY);
        }
        private void YemOlustur()
        {
            Label lbl = new Label()
            {
                Name = "yem",
                BackColor = Color.Yellow,
                Width = _yemBoyutu,
                Height = _yemBoyutu,


            };
            _yem = lbl;
            this.pnl.Controls.Add(lbl);

        }
        private void YeminYeriniDegistir()
        {
            var locationX = 0;
            var locationY = 0;

            bool durum;
            do
            {
                durum = false;
                locationX = _random.Next(0, pnl.Width - _yemBoyutu);
                locationY = _random.Next(0, pnl.Height - _yemBoyutu);
                var rect1 = new Rectangle(new Point(locationX, locationY), _yem.Size);
                foreach (Control control in pnl.Controls)
                {
                    if (control is Label && control.Name.Contains("yilanParca"))
                    {
                        var rect2 = new Rectangle(control.Location, control.Size);
                        if (rect1.IntersectsWith(rect2))
                        {
                            durum = true;
                            break;
                        }

                    }
                }
            } while (durum);

            _yem.Location = new Point(locationX, locationY);
        }
        private enum HareketYonu
        {
            Yukari,
            Asagi,
            Sola,
            Saga
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            var keyCode = e.KeyCode;
            if (_yon == HareketYonu.Sola && keyCode == Keys.D
                || _yon == HareketYonu.Saga && keyCode == Keys.A
                || _yon == HareketYonu.Yukari && keyCode == Keys.S
                || _yon == HareketYonu.Asagi && keyCode == Keys.W)
            {
                return;

            }
            switch (keyCode)
            {
                case Keys.W:
                    _yon = HareketYonu.Yukari;
                    break;
                case Keys.S:
                    _yon = HareketYonu.Asagi;
                    break;
                case Keys.A:
                    _yon = HareketYonu.Sola;
                    break;
                case Keys.D:
                    _yon = HareketYonu.Saga;
                    break;
                case Keys.P:
                    timerSaat.Enabled = false;
                    timerYilanHareket.Enabled = false;
                    break;
                case Keys.C:
                    timerSaat.Enabled = true;
                    timerYilanHareket.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        private void timerYilanHareket_Tick(object sender, EventArgs e)
        {
            YilanKafasiniTakipEt();
            YilaniYurut();
            OyunBittimi();
            YilanYemiYedimi();

        }
        private void YilaniYurut()
        {
            var locationX = _yilanKafasi.Location.X;
            var locationY = _yilanKafasi.Location.Y;
            switch (_yon)
            {
                case HareketYonu.Yukari:
                    _yilanKafasi.Location = new Point(locationX, locationY - (_yilanKafasi.Width + _yilanParcasiArasiMesafe));
                    break;
                case HareketYonu.Asagi:
                    _yilanKafasi.Location = new Point(locationX, locationY + (_yilanKafasi.Width + _yilanParcasiArasiMesafe));
                    break;
                case HareketYonu.Sola:
                    _yilanKafasi.Location = new Point(locationX - (_yilanKafasi.Width + _yilanParcasiArasiMesafe), locationY);
                    break;
                case HareketYonu.Saga:
                    _yilanKafasi.Location = new Point(locationX + (_yilanKafasi.Width + _yilanParcasiArasiMesafe), locationY);
                    break;
                default:
                    break;
            }
        }
        private void OyunBittimi()
        {
            bool oyunBittimi = false;
            var rect1 = new Rectangle(_yilanKafasi.Location, _yilanKafasi.Size);
            foreach (Control control in pnl.Controls)
            {

                if (control is Label && control.Name.Contains("yılanParca") && control.Name != _yilanKafasi.Name)
                {
                    var rect2 = new Rectangle(control.Location, control.Size);
                    if (rect1.IntersectsWith(rect2))
                    {
                        oyunBittimi = true;
                        break;
                    }
                }

            }
            if (oyunBittimi)
            {
                timerYilanHareket.Enabled = false;
                timerSaat.Enabled = false;
                DialogResult sonuc = MessageBox.Show("Puanınız:" + label3.Text, "Oyun Bitti!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (sonuc == DialogResult.OK)
                {
                    YenidenBaslat();
                }
            }
        }


        private void YilanYemiYedimi()
        {
            var rect1 = new Rectangle(_yilanKafasi.Location, _yilanKafasi.Size);
            var rect2 = new Rectangle(_yem.Location, _yem.Size);

            if (rect1.IntersectsWith(rect2))
            {
                label3.Text = (Convert.ToInt32(label3.Text) + 10).ToString();
                YeminYeriniDegistir();
                YilanParcasiOlustur(-_yilanBoyutu, -_yilanBoyutu);
            }
        }
        private void YilanKafasiniTakipEt()
        {
            if (_yilanParcasiSayisi <= 1) return;

            for (int i = _yilanParcasiSayisi; i > 1; i--)
            {
                var sonrakiParca = (Label)pnl.Controls[i];
                var oncekiParca = (Label)pnl.Controls[i - 1];
                sonrakiParca.Location = oncekiParca.Location;
            }
        }

        private void timerSaat_Tick(object sender, EventArgs e)
        {
            label5.Text = (Convert.ToInt32(label5.Text) + 1).ToString();
        }
    }
}
