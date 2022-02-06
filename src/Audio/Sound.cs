using Utubz.Internal.Native.Bass;
using System.IO;

namespace Utubz.Audio
{
    public sealed class Sound : Object
    {
        private readonly uint handle;

        public float Position { get => (float)bass.BASS_ChannelBytes2Seconds(handle, bass.BASS_ChannelGetPosition(handle, 2)); }

        public void Play()
        {
            Debug.LogVerbose($"Playing audio: {bass.BASS_ChannelPlay(handle, 0)}");
        }

        public void Stop()
        {
            bass.BASS_ChannelStop(handle);
        }

        public void Restart()
        {
            Debug.LogVerbose($"Playing audio: {bass.BASS_ChannelPlay(handle, 1)}");
        }

        public void Pause()
        {
            bass.BASS_ChannelPause(handle);
        }

        public static Sound FromFile(string file)
        {
            if (!File.Exists(file))
                return null;

            Debug.LogVerbose($"Load file from: {file.Replace('\\', '/')}");
            return new Sound(bass.BASS_StreamCreateFile(0, file.Replace('\\', '/'), 0, 0, 0));
        }

        protected override void Clean()
        {
            bass.BASS_StreamFree(handle);
        }

        private class AudioLoadException : System.Exception
        {
            public AudioLoadException() : base($"Audio Load Error (code: {bass.BASS_ErrorGetCode()})")
            {

            }
        }

        private Sound(uint h)
        {
            if (h == 0)
                throw new AudioLoadException();
            handle = h;
        }
    }
}
