using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices; 

namespace beller
{
    class Sound
    {
        const int Crotchets = 500;//1分钟480拍，62毫秒

        public int pitch { get; set; }//音高，高音3，中音2，低音1
        public int note { get; set; }//简谱，音符,1,2,3,4,5,6,7
        public int duration { get; set; }//音长，以三十二分音符为0，十六分音符为1，八分为2，四分为3，二分音符为4，全音符为5

        //音的频率，低音，中音，高音，DO-XI，静音另算
        public static readonly int[,] soundFreq=new int[,]{{138,156,175,185,208,233,261},{262,294,330,349,392,440,494},{523,587,659,698,784,880,988},{1046,1175,1318,1397,1568,1760,1976},{2095,2352,2640,2797,3140,3525,2957}};
        //音的时长，0-9,3对应四分音符，一拍1200ms
        public static readonly int[] soundBaseDuration = new int[] { Crotchets / 8, Crotchets / 4, Crotchets / 2, Crotchets, Crotchets * 2, Crotchets * 4, Crotchets * 6, Crotchets * 8, Crotchets * 16, Crotchets * 32 };

        //2
        //[DllImport("kernel32.dll", EntryPoint = "Beep")]
        //public static extern int Beep(int dwFreq, int dwDuration);

        /// <summary>
        /// 默认值为四分的中音DO
        /// </summary>
        public Sound()
        {
            pitch = 2;
            note = 1;
            duration = 3;
        }

        public Sound(int pitch, int note, int length)
        {
            this.pitch = pitch;
            this.note = note;
            this.duration = length;
        }

        public void playSound()
        {
            int soundduration = soundBaseDuration[duration];
            int freq = 0;
            if (note == 0)
            {
                //Beep(0, soundduration/2);
                Thread.Sleep(soundduration);
            }
            else
            {
                freq = soundFreq[pitch - 1,note - 1];
                Console.Beep(freq, soundduration);
                //Beep(freq, soundduration/2);
            }
        }
    }
    class BellMusicClass
    {
        private List<Sound> musiclist;

        public BellMusicClass()
        {
            musiclist = new List<Sound>();
        }

        public bool addSound(int sound)//只允许sound为3位,第一位为音高（1-3），第二位为音符（1--7,8休止），第三位为音长，三十二分音符为0
        {
            if (sound < 110 || sound > 589) return false;
            int length = sound % 10;
            int pitch = ((sound - sound % 100) / 100) % 10;
            if (pitch < 1 || pitch > 5) return false; //暂时只支持高音，低音和中音三个
            int note = ((sound - length) / 10 ) % 10;
            if(note > 7) note = 7;//只允许同阶最高为 XI,休止符为0，休止符音高任意，长度以拍计
            musiclist.Add(new Sound( pitch, note, length));
            return true;
        }

        public bool addSound(string sound)
        {
            if (sound.Trim().Length == 0) return false;
            int intSound = 0;
            if (int.TryParse(sound, out intSound)) return addSound(intSound);
            else return false;

        }

        public bool addMusic(string music, char separate)
        {
            string[] musicparts = music.Split(separate);
            for (int i = 0; i < musicparts.Length; ++i)
            {
                if (musicparts[i].Trim().Length != 3) continue; //一个音的长度被限制为3个
                if (addSound(musicparts[i]) == false)
                    
                    return false;
            }
            return true;
        }

        public bool playMusic()
        {
            if (musiclist.Count == 0) return false;
            for (int i = 0; i < musiclist.Count; ++i)
            {
                musiclist[i].playSound();
            }
            return true;
        }
    }
}
