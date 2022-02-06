using System;
using System.Runtime.InteropServices;

namespace Utubz.Internal.Discord
{
	internal partial struct ImageHandle
    {
        static internal ImageHandle User(Int64 id)
        {
            return User(id, 128);
        }

        static internal ImageHandle User(Int64 id, UInt32 size)
        {
            return new ImageHandle
            {
                Type = ImageType.User,
                Id = id,
                Size = size,
            };
        }
    }

    internal partial class ImageManager
    {
        internal void Fetch(ImageHandle handle, FetchHandler callback)
        {
            Fetch(handle, false, callback);
        }

        internal byte[] GetData(ImageHandle handle)
        {
            var dimensions = GetDimensions(handle);
            var data = new byte[dimensions.Width * dimensions.Height * 4];
            GetData(handle, data);
            return data;
        }

#if UNITY_EDITOR || UNITY_STANDALONE
        internal Texture2D GetTexture(ImageHandle handle)
        {
            var dimensions = GetDimensions(handle);
            var texture = new Texture2D((int)dimensions.Width, (int)dimensions.Height, TextureFormat.RGBA32, false, true);
            texture.LoadRawTextureData(GetData(handle));
            texture.Apply();
            return texture;
        }
#endif
    }
}
