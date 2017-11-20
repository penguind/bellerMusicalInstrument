using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace beller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "每个音节分三部分：\n第一部分1-3代表低音、中音、高音\n第二部分0-7代表休止，DO到XI（同简谱）\n第三部分0-9代表音长，0是三十二分音符，1是十六分音符，2是八分音符，3是四分音符，4是二分音符，5是全音符，依此类推\n音节以小数点 . 分隔";
            textBox2.Text = "433.413.423.463.432.422.412.422.463.303.433.413.423.423.452.432.373.412";// "162.211.211.212.211.211.222.221.221.221.211.212.213";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread playThread = new Thread(new ParameterizedThreadStart(playmusic));
            playThread.Start(this.textBox2.Text);
        }

        private void playmusic(object music)
        {
            string musicText = music.ToString();
            BellMusicClass bmc = new BellMusicClass();
            bmc.addMusic(musicText, '.');
            bmc.playMusic();
        }
    }
}
