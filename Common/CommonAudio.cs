using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using NAudio.Wave;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace keyupMusic2
{
    public partial class Common
    {
        public static WaveOutEvent player = new WaveOutEvent();
        public static WaveFileReader player_reader;
        public static TimeSpan _player_time;
        public static int player_time => (int)(_player_time.TotalSeconds + 1) * 1000;
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
                _player_time = player_reader.TotalTime;
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
            _player_time = player_reader.TotalTime;
            player.Stop();
            player.Init(player_reader);
            player.Play();
        }
        public static void play_sound_bongocat(Keys key)
        {
            string wav = "wav\\bongocat\\keyboard" + key.ToString().Replace("D", "") + ".wav";
            if (!File.Exists(wav)) return;

            try
            {
                player_reader_bongo = new WaveFileReader(wav);
                player_bongo.Stop();
                player_bongo.Init(player_reader_bongo);
                player_bongo.Play();
            }
            catch (Exception ex)
            {
                // 可以根据需要记录日志或提示用户
                Console2.WriteLine($"播放bongocat音效时发生异常: {ex.Message}");
            }
        }
        public static void play_sound_bongocat(int key)
        {
            string wav = "wav\\bongocat\\keyboard" + key + ".wav";
            if (!File.Exists(wav)) return;

            try
            {
                player_reader_bongo = new WaveFileReader(wav);
                float volume = GetSystemVolume();
                if (volume > 0.2) { volume = 0.2f / volume; }
                var volumeProvider2 = new VolumeWaveProvider16(player_reader_bongo) { Volume = volume };
                player_bongo.Stop();
                player_bongo.Init(volumeProvider2);
                player_bongo.Play();
            }
            catch (Exception ex)
            {
                // 可以根据需要记录日志或提示用户
                Console2.WriteLine($"播放bongocat音效时发生异常: {ex.Message}");
            }
        }
        static string wav_di = "wav\\d2.wav";
        static bool is_di = File.Exists(wav_di);
        public static void play_sound_di()
        {
            if (!is_di) return;
            try
            {
                player_reader_di = new WaveFileReader(wav_di);
                float volume = GetSystemVolume();
                if (volume > 0.2) { volume = 1 / volume; }
                else volume = 1;
                var volumeProvider2 = new VolumeWaveProvider16(player_reader_di) { Volume = volume };
                player_di.Stop();
                player_di.Init(volumeProvider2);
                player_di.Play();
            }
            catch (Exception ex)
            {
                // 可以根据需要记录日志或提示用户
                Console2.WriteLine($"播放bongocat音效时发生异常: {ex.Message}");
            }
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
        public static bool IsAnyAudioPlaying()
        {
            using (var enumerator = new MMDeviceEnumerator())
            using (var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console))
            {
                var sessions = device.AudioSessionManager.Sessions;
                for (var i = 0; i < sessions.Count; i++)
                {
                    var session = sessions[i];
                    if (session.State == AudioSessionState.AudioSessionStateActive
                        && session.GetSessionIdentifier.Contains("music"))
                        return true;
                }
            }
            return false;
        }
        public static void ConvertMp3ToWav(string mp3File, string wavFile)
        {
            using (var mp3Reader = new Mp3FileReader(mp3File))
            {
                // Convert MP3 to PCM (16-bit, SubChunk1Size = 16)
                var pcmFormat = new WaveFormat(16000, 16, 1); // 44.1kHz, 16-bit, stereo
                using (var pcmStream = new MediaFoundationResampler(mp3Reader, pcmFormat))
                {
                    pcmStream.ResamplerQuality = 1; // Optional: set resample quality (1–60)

                    WaveFileWriter.CreateWaveFile(wavFile, pcmStream);
                }
            }
        }

    }
}
