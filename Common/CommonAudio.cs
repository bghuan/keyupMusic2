using NAudio.CoreAudioApi.Interfaces;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Media;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using static keyupMusic2.Native;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Point = System.Drawing.Point;

namespace keyupMusic2
{
    public partial class Common
    {
        public static WaveOutEvent player = new WaveOutEvent();
        public static WaveFileReader player_reader;
        public static TimeSpan player_time;
        public static bool is_playing { get { return player != null && player.PlaybackState == PlaybackState.Playing; } }

        public static WaveFileReader player_reader_di;
        public static WaveOutEvent player_di = new WaveOutEvent();

        public static WaveFileReader player_reader_bongo;
        public static WaveOutEvent player_bongo = new WaveOutEvent();

        public static bool key_sound = true;
        public static void play_sound(Keys key, bool force = false)
        {
            if (is_down(Keys.LWin)) return;
            if (Position.Y == 0) return;
            //if (key_sound && keys.Contains(e.key))
            if (key_sound || force)
            {
                string wav = "wav\\" + key.ToString().Replace("D", "").Replace("F", "") + ".wav";
                if (!File.Exists(wav)) return;

                player_reader = new WaveFileReader(wav);
                player_time = player_reader.TotalTime;
                player.Stop();
                player.Init(player_reader);
                player.Play();
            }
        }
        public static void play_sound(int key)
        {
            string wav = "wav\\" + key + ".wav";
            if (!File.Exists(wav)) return;

            player_reader = new WaveFileReader(wav);
            player_time = player_reader.TotalTime;
            player.Stop();
            player.Init(player_reader);
            player.Play();
        }
        public static void play_sound_bongocat(Keys key)
        {
            string wav = "wav\\bongocat\\keyboard" + key.ToString().Replace("D", "") + ".wav";
            if (!File.Exists(wav)) return;

            player_reader_bongo = new WaveFileReader(wav);
            player_bongo.Stop();
            player_bongo.Init(player_reader_bongo);
            player_bongo.Play();
        }
        static string wav_di = "wav\\d2.wav";
        static bool is_di = File.Exists(wav_di);
        public static void play_sound_di(int tick = 0)
        {
            if (!is_di) return;

            player_reader_di = new WaveFileReader(wav_di);
            player_di.Stop();
            player_di.Init(player_reader_di);
            player_di.Play();
            Sleep(tick);
        }
        public static void play_sound_di2(int tick = 0)
        {
            play_sound_di();
            TaskRun(() => play_sound_di(), 80);
        }
        public static float GetSystemVolume()
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                // 获取默认音频输出设备（扬声器/耳机）
                var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

                // 获取音量（范围：0.0f 到 1.0f）
                return device.AudioEndpointVolume.MasterVolumeLevelScalar;
            }
        }
        public static void SetSystemVolume(float volume)
        {
            // 确保音量在有效范围内
            volume = Math.Max(0.0f, Math.Min(1.0f, volume));

            using (var enumerator = new MMDeviceEnumerator())
            {
                var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                device.AudioEndpointVolume.MasterVolumeLevelScalar = volume;
            }
        }
        public bool IsAnyAudioPlaying()
        {
            try
            {
                using (var enumerator = new MMDeviceEnumerator())
                {
                    var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
                    var sessions = device.AudioSessionManager.Sessions;

                    for (global::System.Int32 i = 0; i < sessions.Count; i++)
                    {
                        var session = sessions [i];
                        if (session.State == AudioSessionState.AudioSessionStateActive)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // 处理异常（如权限不足）
            }

            return false;
        }

    }
}
