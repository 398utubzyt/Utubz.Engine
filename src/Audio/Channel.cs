using Utubz.Internal.Native.Bass;
using System.Runtime.InteropServices;

namespace Utubz.Audio
{
    public abstract class Channel : Object
    {
        protected uint handle;
        private BASS_CHANNELINFO info;

        public uint Frequency { get { { return info.Freq; } } }
        public float Position { get => (float)bass.BASS_ChannelBytes2Seconds(handle, bass.BASS_ChannelGetPosition(handle, 2)); }
        public float Volume { get { float x = 0f; bass.BASS_ChannelGetAttribute(handle, 2, ref x); return x; } set { bass.BASS_ChannelSetAttribute(handle, 2, value); } }
        public float Pitch { get { float x = 0f; bass.BASS_ChannelGetAttribute(handle, 1, ref x); return x / Frequency; } set { bass.BASS_ChannelSetAttribute(handle, 1, value * Frequency); } }
        // TODO: Get the speed to actually work lmao
        public float Speed { get { float x = 0f; bass.BASS_ChannelGetAttribute(handle, 65537, ref x); return (x + 100f) / 100f; } set { bass.BASS_ChannelSetAttribute(handle, 65537, value * 100f - 100f); } }
        public bool Loop { get { return (bass.BASS_ChannelFlags(handle, 0, 0) & 4) == 4; } set { bass.BASS_ChannelFlags(handle, value ? 4u : 0u, 4); } }

        public bool Playing { get => bass.BASS_ChannelIsActive(handle) != 0; }
        public bool Paused { get => bass.BASS_ChannelIsActive(handle) == 3; }

        public void Play()
        {
            bass.BASS_ChannelPlay(handle, 0);
        }

        public void Stop()
        {
            bass.BASS_ChannelStop(handle);
        }

        public void Restart()
        {
            bass.BASS_ChannelPlay(handle, 1);
        }

        public void Pause()
        {
            bass.BASS_ChannelPause(handle);
        }

        protected override void Clean()
        {
            Close();

            info.Dispose();
        }

        protected abstract void Close();

        private class AudioLoadException : System.Exception
        {
            public AudioLoadException() : base($"Audio Load Error (code: {bass.BASS_ErrorGetCode()})")
            {
            }
        }

        protected Channel(uint h)
        {
            if (h == 0)
                throw new AudioLoadException();
            handle = h;
            
            bass_fx.BASS_FX_TempoCreate(handle, 0x10000);

            info = new BASS_CHANNELINFO();
            bass.BASS_ChannelGetInfo(handle, info);
        }
    }
}
