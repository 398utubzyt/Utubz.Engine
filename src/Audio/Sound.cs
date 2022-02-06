using Utubz.Internal.Native.Bass;
using System.IO;

namespace Utubz.Audio
{
    public sealed class Sound : Channel
    {

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

        private Sound(uint h) : base(h)
        {
        }
    }
}
