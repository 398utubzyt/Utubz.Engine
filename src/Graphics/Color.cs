﻿using System;
using System.Runtime.InteropServices;

namespace Utubz.Graphics
{
    /// <summary>
    /// A color represented by red, green, blue, and alpha (RGBA) components.
    /// The alpha component is often used for transparency.
    /// Values are in floating-point and usually range from 0 to 1.
    ///
    /// If you want to supply values in a range of 0 to 255, you should use
    /// <see cref="Color8"/> and the <c>r8</c>/<c>g8</c>/<c>b8</c>/<c>a8</c> properties.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Color : IEquatable<Color>
    {
        /// <summary>
        /// The color's red component, typically on the range of 0 to 1.
        /// </summary>
        public float r;

        /// <summary>
        /// The color's green component, typically on the range of 0 to 1.
        /// </summary>
        public float g;

        /// <summary>
        /// The color's blue component, typically on the range of 0 to 1.
        /// </summary>
        public float b;

        /// <summary>
        /// The color's alpha (transparency) component, typically on the range of 0 to 1.
        /// </summary>
        public float a;

        /// <summary>
        /// Wrapper for <see cref="r"/> that uses the range 0 to 255 instead of 0 to 1.
        /// </summary>
        /// <value>Getting is equivalent to multiplying by 255 and rounding. Setting is equivalent to dividing by 255.</value>
        public int r8
        {
            get
            {
                return (int)Math.Round(r * 255.0f);
            }
            set
            {
                r = value / 255.0f;
            }
        }

        /// <summary>
        /// Wrapper for <see cref="g"/> that uses the range 0 to 255 instead of 0 to 1.
        /// </summary>
        /// <value>Getting is equivalent to multiplying by 255 and rounding. Setting is equivalent to dividing by 255.</value>
        public int g8
        {
            get
            {
                return (int)Math.Round(g * 255.0f);
            }
            set
            {
                g = value / 255.0f;
            }
        }

        /// <summary>
        /// Wrapper for <see cref="b"/> that uses the range 0 to 255 instead of 0 to 1.
        /// </summary>
        /// <value>Getting is equivalent to multiplying by 255 and rounding. Setting is equivalent to dividing by 255.</value>
        public int b8
        {
            get
            {
                return (int)Math.Round(b * 255.0f);
            }
            set
            {
                b = value / 255.0f;
            }
        }

        /// <summary>
        /// Wrapper for <see cref="a"/> that uses the range 0 to 255 instead of 0 to 1.
        /// </summary>
        /// <value>Getting is equivalent to multiplying by 255 and rounding. Setting is equivalent to dividing by 255.</value>
        public int a8
        {
            get
            {
                return (int)Math.Round(a * 255.0f);
            }
            set
            {
                a = value / 255.0f;
            }
        }

        /// <summary>
        /// The HSV hue of this color, on the range 0 to 1.
        /// </summary>
        /// <value>Getting is a long process, refer to the source code for details. Setting uses <see cref="FromHSV"/>.</value>
        public float h
        {
            get
            {
                float max = Math.Max(r, Math.Max(g, b));
                float min = Math.Min(r, Math.Min(g, b));

                float delta = max - min;

                if (delta == 0)
                {
                    return 0;
                }

                float h;

                if (r == max)
                {
                    h = (g - b) / delta; // Between yellow & magenta
                } else if (g == max)
                {
                    h = 2 + ((b - r) / delta); // Between cyan & yellow
                } else
                {
                    h = 4 + ((r - g) / delta); // Between magenta & cyan
                }

                h /= 6.0f;

                if (h < 0)
                {
                    h += 1.0f;
                }

                return h;
            }
            set
            {
                this = FromHSV(value, s, v, a);
            }
        }

        /// <summary>
        /// The HSV saturation of this color, on the range 0 to 1.
        /// </summary>
        /// <value>Getting is equivalent to the ratio between the min and max RGB value. Setting uses <see cref="FromHSV"/>.</value>
        public float s
        {
            get
            {
                float max = Math.Max(r, Math.Max(g, b));
                float min = Math.Min(r, Math.Min(g, b));

                float delta = max - min;

                return max == 0 ? 0 : delta / max;
            }
            set
            {
                this = FromHSV(h, value, v, a);
            }
        }

        /// <summary>
        /// The HSV value (brightness) of this color, on the range 0 to 1.
        /// </summary>
        /// <value>Getting is equivalent to using <see cref="Math.Max(float, float)"/> on the RGB components. Setting uses <see cref="FromHSV"/>.</value>
        public float v
        {
            get
            {
                return Math.Max(r, Math.Max(g, b));
            }
            set
            {
                this = FromHSV(h, s, value, a);
            }
        }

        /// <summary>
        /// Access color components using their index.
        /// </summary>
        /// <value>
        /// <c>[0]</c> is equivalent to <see cref="r"/>,
        /// <c>[1]</c> is equivalent to <see cref="g"/>,
        /// <c>[2]</c> is equivalent to <see cref="b"/>,
        /// <c>[3]</c> is equivalent to <see cref="a"/>.
        /// </value>
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return r;
                    case 1:
                        return g;
                    case 2:
                        return b;
                    case 3:
                        return a;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        r = value;
                        return;
                    case 1:
                        g = value;
                        return;
                    case 2:
                        b = value;
                        return;
                    case 3:
                        a = value;
                        return;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Returns a new color resulting from blending this color over another.
        /// If the color is opaque, the result is also opaque.
        /// The second color may have a range of alpha values.
        /// </summary>
        /// <param name="over">The color to blend over.</param>
        /// <returns>This color blended over <paramref name="over"/>.</returns>
        public Color Blend(Color over)
        {
            Color res;

            float sa = 1.0f - over.a;
            res.a = (a * sa) + over.a;

            if (res.a == 0)
            {
                return new Color(0, 0, 0, 0);
            }

            res.r = ((r * a * sa) + (over.r * over.a)) / res.a;
            res.g = ((g * a * sa) + (over.g * over.a)) / res.a;
            res.b = ((b * a * sa) + (over.b * over.a)) / res.a;

            return res;
        }

        /// <summary>
        /// Returns a new color with all components clamped between the
        /// components of <paramref name="min"/> and <paramref name="max"/>
        /// using <see cref="Math.Clamp(float, float, float)"/>.
        /// </summary>
        /// <param name="min">The color with minimum allowed values.</param>
        /// <param name="max">The color with maximum allowed values.</param>
        /// <returns>The color with all components clamped.</returns>
        public Color Clamp(Color? min = null, Color? max = null)
        {
            Color minimum = min ?? new Color(0, 0, 0, 0);
            Color maximum = max ?? new Color(1, 1, 1, 1);
            return new Color
            (
                (float)Math.Clamp(r, minimum.r, maximum.r),
                (float)Math.Clamp(g, minimum.g, maximum.g),
                (float)Math.Clamp(b, minimum.b, maximum.b),
                (float)Math.Clamp(a, minimum.a, maximum.a)
            );
        }

        /// <summary>
        /// Returns a new color resulting from making this color darker
        /// by the specified ratio (on the range of 0 to 1).
        /// </summary>
        /// <param name="amount">The ratio to darken by.</param>
        /// <returns>The darkened color.</returns>
        public Color Darkened(float amount)
        {
            Color res = this;
            res.r *= 1.0f - amount;
            res.g *= 1.0f - amount;
            res.b *= 1.0f - amount;
            return res;
        }

        /// <summary>
        /// Returns the inverted color: <c>(1 - r, 1 - g, 1 - b, a)</c>.
        /// </summary>
        /// <returns>The inverted color.</returns>
        public Color Inverted()
        {
            return new Color(
                1.0f - r,
                1.0f - g,
                1.0f - b,
                a
            );
        }

        /// <summary>
        /// Returns a new color resulting from making this color lighter
        /// by the specified ratio (on the range of 0 to 1).
        /// </summary>
        /// <param name="amount">The ratio to lighten by.</param>
        /// <returns>The darkened color.</returns>
        public Color Lightened(float amount)
        {
            Color res = this;
            res.r += (1.0f - res.r) * amount;
            res.g += (1.0f - res.g) * amount;
            res.b += (1.0f - res.b) * amount;
            return res;
        }

        /// <summary>
        /// Returns the result of the linear interpolation between
        /// this color and <paramref name="to"/> by amount <paramref name="weight"/>.
        /// </summary>
        /// <param name="to">The destination color for interpolation.</param>
        /// <param name="weight">A value on the range of 0.0 to 1.0, representing the amount of interpolation.</param>
        /// <returns>The resulting color of the interpolation.</returns>
        public Color Lerp(Color to, float weight)
        {
            return new Color
            (
                Math.Lerp(r, to.r, weight),
                Math.Lerp(g, to.g, weight),
                Math.Lerp(b, to.b, weight),
                Math.Lerp(a, to.a, weight)
            );
        }

        /// <summary>
        /// Returns the result of the linear interpolation between
        /// this color and <paramref name="to"/> by color amount <paramref name="weight"/>.
        /// </summary>
        /// <param name="to">The destination color for interpolation.</param>
        /// <param name="weight">A color with components on the range of 0.0 to 1.0, representing the amount of interpolation.</param>
        /// <returns>The resulting color of the interpolation.</returns>
        public Color Lerp(Color to, Color weight)
        {
            return new Color
            (
                Math.Lerp(r, to.r, weight.r),
                Math.Lerp(g, to.g, weight.g),
                Math.Lerp(b, to.b, weight.b),
                Math.Lerp(a, to.a, weight.a)
            );
        }

        /// <summary>
        /// Returns the color converted to an unsigned 32-bit integer in ABGR
        /// format (each byte represents a color channel).
        /// ABGR is the reversed version of the default format.
        /// </summary>
        /// <returns>A <see langword="uint"/> representing this color in ABGR32 format.</returns>
        public uint ToAbgr32()
        {
            uint c = (byte)Math.Round(a * 255);
            c <<= 8;
            c |= (byte)Math.Round(b * 255);
            c <<= 8;
            c |= (byte)Math.Round(g * 255);
            c <<= 8;
            c |= (byte)Math.Round(r * 255);

            return c;
        }

        /// <summary>
        /// Returns the color converted to an unsigned 64-bit integer in ABGR
        /// format (each word represents a color channel).
        /// ABGR is the reversed version of the default format.
        /// </summary>
        /// <returns>A <see langword="ulong"/> representing this color in ABGR64 format.</returns>
        public ulong ToAbgr64()
        {
            ulong c = (ushort)Math.Round(a * 65535);
            c <<= 16;
            c |= (ushort)Math.Round(b * 65535);
            c <<= 16;
            c |= (ushort)Math.Round(g * 65535);
            c <<= 16;
            c |= (ushort)Math.Round(r * 65535);

            return c;
        }

        /// <summary>
        /// Returns the color converted to an unsigned 32-bit integer in ARGB
        /// format (each byte represents a color channel).
        /// ARGB is more compatible with DirectX, but not used much in Godot.
        /// </summary>
        /// <returns>A <see langword="uint"/> representing this color in ARGB32 format.</returns>
        public uint ToArgb32()
        {
            uint c = (byte)Math.Round(a * 255);
            c <<= 8;
            c |= (byte)Math.Round(r * 255);
            c <<= 8;
            c |= (byte)Math.Round(g * 255);
            c <<= 8;
            c |= (byte)Math.Round(b * 255);

            return c;
        }

        /// <summary>
        /// Returns the color converted to an unsigned 64-bit integer in ARGB
        /// format (each word represents a color channel).
        /// ARGB is more compatible with DirectX, but not used much in Godot.
        /// </summary>
        /// <returns>A <see langword="ulong"/> representing this color in ARGB64 format.</returns>
        public ulong ToArgb64()
        {
            ulong c = (ushort)Math.Round(a * 65535);
            c <<= 16;
            c |= (ushort)Math.Round(r * 65535);
            c <<= 16;
            c |= (ushort)Math.Round(g * 65535);
            c <<= 16;
            c |= (ushort)Math.Round(b * 65535);

            return c;
        }

        /// <summary>
        /// Returns the color converted from a signed 32-bit integer in RGBA
        /// format (each byte represents a color channel).
        /// RGBA is Godot's default and recommended format.
        /// </summary>
        /// <returns>A <see cref="Color"/> representing the RGBA32 <see langword="int"/> provided.</returns>
        public Color FromRgba32(int color)
        {
            r8 = (color >> 24) & 0xFF;
            g8 = (color >> 16) & 0xFF;
            b8 = (color >> 8) & 0xFF;
            a8 = color & 0xFF;

            return this;
        }

        /// <summary>
        /// Returns the color converted from a signed 32-bit integer in RGBA
        /// format (each byte represents a color channel).
        /// RGBA is Godot's default and recommended format.
        /// </summary>
        /// <returns>A <see cref="Color"/> representing the RGBA32 <see langword="int"/> provided.</returns>
        public Color FromRgba32(uint color)
        {
            r8 = (int)((color >> 24) & 0xFF);
            g8 = (int)((color >> 16) & 0xFF);
            b8 = (int)((color >> 8) & 0xFF);
            a8 = (int)(color & 0xFF);

            return this;
        }

        /// <summary>
        /// Returns the color converted to an unsigned 32-bit integer in RGBA
        /// format (each byte represents a color channel).
        /// RGBA is Godot's default and recommended format.
        /// </summary>
        /// <returns>A <see langword="int"/> representing this color in RGBA32 format.</returns>
        public int ToRgba32()
        {
            int c = (byte)Math.Round(r * 255);
            c <<= 8;
            c |= (byte)Math.Round(g * 255);
            c <<= 8;
            c |= (byte)Math.Round(b * 255);
            c <<= 8;
            c |= (byte)Math.Round(a * 255);

            return c;
        }

        /// <summary>
        /// Returns the color converted to an unsigned 32-bit integer in RGBA
        /// format (each byte represents a color channel).
        /// RGBA is Godot's default and recommended format.
        /// </summary>
        /// <returns>A <see langword="uint"/> representing this color in RGBA32 format.</returns>
        public uint ToUnsignedRgba32()
        {
            uint c = (byte)Math.Round(r * 255);
            c <<= 8;
            c |= (byte)Math.Round(g * 255);
            c <<= 8;
            c |= (byte)Math.Round(b * 255);
            c <<= 8;
            c |= (byte)Math.Round(a * 255);

            return c;
        }

        /// <summary>
        /// Returns the color converted to an unsigned 64-bit integer in RGBA
        /// format (each word represents a color channel).
        /// RGBA is Godot's default and recommended format.
        /// </summary>
        /// <returns>A <see langword="ulong"/> representing this color in RGBA64 format.</returns>
        public ulong ToRgba64()
        {
            ulong c = (ushort)Math.Round(r * 65535);
            c <<= 16;
            c |= (ushort)Math.Round(g * 65535);
            c <<= 16;
            c |= (ushort)Math.Round(b * 65535);
            c <<= 16;
            c |= (ushort)Math.Round(a * 65535);

            return c;
        }

        /// <summary>
        /// Returns the color's HTML hexadecimal color string in RGBA format.
        /// </summary>
        /// <param name="includeAlpha">
        /// Whether or not to include alpha. If <see langword="false"/>, the color is RGB instead of RGBA.
        /// </param>
        /// <returns>A string for the HTML hexadecimal representation of this color.</returns>
        public string ToHTML(bool includeAlpha = true)
        {
            string txt = string.Empty;

            txt += ToHex32(r);
            txt += ToHex32(g);
            txt += ToHex32(b);

            if (includeAlpha)
            {
                txt += ToHex32(a);
            }

            return txt;
        }

        /// <summary>
        /// Constructs a <see cref="Color"/> from RGBA values, typically on the range of 0 to 1.
        /// </summary>
        /// <param name="r">The color's red component, typically on the range of 0 to 1.</param>
        /// <param name="g">The color's green component, typically on the range of 0 to 1.</param>
        /// <param name="b">The color's blue component, typically on the range of 0 to 1.</param>
        /// <param name="a">The color's alpha (transparency) value, typically on the range of 0 to 1. Default: 1.</param>
        public Color(float r, float g, float b, float a = 1.0f)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        /// <summary>
        /// Constructs a <see cref="Color"/> from an existing color and an alpha value.
        /// </summary>
        /// <param name="c">The color to construct from. Only its RGB values are used.</param>
        /// <param name="a">The color's alpha (transparency) value, typically on the range of 0 to 1. Default: 1.</param>
        public Color(Color c, float a = 1.0f)
        {
            r = c.r;
            g = c.g;
            b = c.b;
            this.a = a;
        }

        /// <summary>
        /// Constructs a <see cref="Color"/> from an unsigned 32-bit integer in RGBA format
        /// (each byte represents a color channel).
        /// </summary>
        /// <param name="rgba">The <see langword="uint"/> representing the color.</param>
        public Color(uint rgba)
        {
            a = (rgba & 0xFF) / 255.0f;
            rgba >>= 8;
            b = (rgba & 0xFF) / 255.0f;
            rgba >>= 8;
            g = (rgba & 0xFF) / 255.0f;
            rgba >>= 8;
            r = (rgba & 0xFF) / 255.0f;
        }

        /// <summary>
        /// Constructs a <see cref="Color"/> from an unsigned 64-bit integer in RGBA format
        /// (each word represents a color channel).
        /// </summary>
        /// <param name="rgba">The <see langword="ulong"/> representing the color.</param>
        public Color(ulong rgba)
        {
            a = (rgba & 0xFFFF) / 65535.0f;
            rgba >>= 16;
            b = (rgba & 0xFFFF) / 65535.0f;
            rgba >>= 16;
            g = (rgba & 0xFFFF) / 65535.0f;
            rgba >>= 16;
            r = (rgba & 0xFFFF) / 65535.0f;
        }

        /// <summary>
        /// Constructs a <see cref="Color"/> either from an HTML color code or from a
        /// standardized color name. Supported color names are the same as the
        /// <see cref="Colors"/> constants.
        /// </summary>
        /// <param name="code">The HTML color code or color name to construct from.</param>
        public Color(string code)
        {
            if (HtmlIsValid(code))
            {
                this = FromHTML(code);
            } else
            {
                this = Color.Black;
            }
        }

        /// <summary>
        /// Constructs a <see cref="Color"/> either from an HTML color code or from a
        /// standardized color name, with <paramref name="alpha"/> on the range of 0 to 1. Supported
        /// color names are the same as the <see cref="Colors"/> constants.
        /// </summary>
        /// <param name="code">The HTML color code or color name to construct from.</param>
        /// <param name="alpha">The alpha (transparency) value, typically on the range of 0 to 1.</param>
        public Color(string code, float alpha)
        {
            this = new Color(code);
            a = alpha;
        }

        /// <summary>
        /// Constructs a <see cref="Color"/> from the HTML hexadecimal color string in RGBA format.
        /// </summary>
        /// <param name="rgba">A string for the HTML hexadecimal representation of this color.</param>
        /// <exception name="ArgumentOutOfRangeException">
        /// Thrown when the given <paramref name="rgba"/> color code is invalid.
        /// </exception>
        private static Color FromHTML(string rgba)
        {
            Color c;
            if (rgba.Length == 0)
            {
                c.r = 0f;
                c.g = 0f;
                c.b = 0f;
                c.a = 1.0f;
                return c;
            }

            if (rgba[0] == '#')
            {
                rgba = rgba.Substring(1);
            }

            // If enabled, use 1 hex digit per channel instead of 2.
            // Other sizes aren't in the HTML/CSS spec but we could add them if desired.
            bool isShorthand = rgba.Length < 5;
            bool alpha;

            if (rgba.Length == 8)
            {
                alpha = true;
            } else if (rgba.Length == 6)
            {
                alpha = false;
            } else if (rgba.Length == 4)
            {
                alpha = true;
            } else if (rgba.Length == 3)
            {
                alpha = false;
            } else
            {
                throw new ArgumentOutOfRangeException(
                    $"Invalid color code. Length is {rgba.Length}, but a length of 6 or 8 is expected: {rgba}");
            }

            c.a = 1.0f;
            if (isShorthand)
            {
                c.r = ParseCol4(rgba, 0) / 15f;
                c.g = ParseCol4(rgba, 1) / 15f;
                c.b = ParseCol4(rgba, 2) / 15f;
                if (alpha)
                {
                    c.a = ParseCol4(rgba, 3) / 15f;
                }
            } else
            {
                c.r = ParseCol8(rgba, 0) / 255f;
                c.g = ParseCol8(rgba, 2) / 255f;
                c.b = ParseCol8(rgba, 4) / 255f;
                if (alpha)
                {
                    c.a = ParseCol8(rgba, 6) / 255f;
                }
            }

            if (c.r < 0)
            {
                throw new ArgumentOutOfRangeException("Invalid color code. Red part is not valid hexadecimal: " + rgba);
            }

            if (c.g < 0)
            {
                throw new ArgumentOutOfRangeException("Invalid color code. Green part is not valid hexadecimal: " + rgba);
            }

            if (c.b < 0)
            {
                throw new ArgumentOutOfRangeException("Invalid color code. Blue part is not valid hexadecimal: " + rgba);
            }

            if (c.a < 0)
            {
                throw new ArgumentOutOfRangeException("Invalid color code. Alpha part is not valid hexadecimal: " + rgba);
            }
            return c;
        }

        /// <summary>
        /// Returns a color constructed from integer red, green, blue, and alpha channels.
        /// Each channel should have 8 bits of information ranging from 0 to 255.
        /// </summary>
        /// <param name="r8">The red component represented on the range of 0 to 255.</param>
        /// <param name="g8">The green component represented on the range of 0 to 255.</param>
        /// <param name="b8">The blue component represented on the range of 0 to 255.</param>
        /// <param name="a8">The alpha (transparency) component represented on the range of 0 to 255.</param>
        /// <returns>The constructed color.</returns>
        public static Color Color8(byte r8, byte g8, byte b8, byte a8 = 255)
        {
            return new Color(r8 / 255f, g8 / 255f, b8 / 255f, a8 / 255f);
        }

        /// <summary>
        /// Constructs a color from an HSV profile, with values on the
        /// range of 0 to 1. This is equivalent to using each of
        /// the <c>h</c>/<c>s</c>/<c>v</c> properties, but much more efficient.
        /// </summary>
        /// <param name="hue">The HSV hue, typically on the range of 0 to 1.</param>
        /// <param name="saturation">The HSV saturation, typically on the range of 0 to 1.</param>
        /// <param name="value">The HSV value (brightness), typically on the range of 0 to 1.</param>
        /// <param name="alpha">The alpha (transparency) value, typically on the range of 0 to 1.</param>
        /// <returns>The constructed color.</returns>
        public static Color FromHSV(float hue, float saturation, float value, float alpha = 1.0f)
        {
            if (saturation == 0)
            {
                // Achromatic (grey)
                return new Color(value, value, value, alpha);
            }

            int i;
            float f, p, q, t;

            hue *= 6.0f;
            hue %= 6f;
            i = (int)hue;

            f = hue - i;
            p = value * (1 - saturation);
            q = value * (1 - (saturation * f));
            t = value * (1 - (saturation * (1 - f)));

            switch (i)
            {
                case 0: // Red is the dominant color
                    return new Color(value, t, p, alpha);
                case 1: // Green is the dominant color
                    return new Color(q, value, p, alpha);
                case 2:
                    return new Color(p, value, t, alpha);
                case 3: // Blue is the dominant color
                    return new Color(p, q, value, alpha);
                case 4:
                    return new Color(t, p, value, alpha);
                default: // (5) Red is the dominant color
                    return new Color(value, p, q, alpha);
            }
        }

        /// <summary>
        /// Converts a color to HSV values. This is equivalent to using each of
        /// the <c>h</c>/<c>s</c>/<c>v</c> properties, but much more efficient.
        /// </summary>
        /// <param name="hue">Output parameter for the HSV hue.</param>
        /// <param name="saturation">Output parameter for the HSV saturation.</param>
        /// <param name="value">Output parameter for the HSV value.</param>
        public void ToHSV(out float hue, out float saturation, out float value)
        {
            float max = (float)Math.Max(r, Math.Max(g, b));
            float min = (float)Math.Min(r, Math.Min(g, b));

            float delta = max - min;

            if (delta == 0)
            {
                hue = 0;
            } else
            {
                if (r == max)
                {
                    hue = (g - b) / delta; // Between yellow & magenta
                } else if (g == max)
                {
                    hue = 2 + ((b - r) / delta); // Between cyan & yellow
                } else
                {
                    hue = 4 + ((r - g) / delta); // Between magenta & cyan
                }

                hue /= 6.0f;

                if (hue < 0)
                    hue += 1.0f;
            }

            if (max == 0)
                saturation = 0;
            else
                saturation = 1 - (min / max);

            value = max;
        }

        private static int ParseCol4(string str, int ofs)
        {
            char character = str[ofs];

            if (character >= '0' && character <= '9')
            {
                return character - '0';
            } else if (character >= 'a' && character <= 'f')
            {
                return character + (10 - 'a');
            } else if (character >= 'A' && character <= 'F')
            {
                return character + (10 - 'A');
            }
            return -1;
        }

        private static int ParseCol8(string str, int ofs)
        {
            return ParseCol4(str, ofs) * 16 + ParseCol4(str, ofs + 1);
        }

        private string ToHex32(float val)
        {
            byte b = (byte)Math.Round(Math.Clamp(val * 255, 0, 255));
            return b.ToString();
        }

        internal static bool HtmlIsValid(string color)
        {
            if (color.Length == 0)
            {
                return false;
            }

            if (color[0] == '#')
            {
                color = color.Substring(1);
            }

            // Check if the amount of hex digits is valid.
            int len = color.Length;
            if (!(len == 3 || len == 4 || len == 6 || len == 8))
            {
                return false;
            }

            // Check if each hex digit is valid.
            for (int i = 0; i < len; i++)
            {
                if (ParseCol4(color, i) == -1)
                {
                    return false;
                }
            }

            return true;
        }

        public static Color operator +(Color left, Color right)
        {
            left.r += right.r;
            left.g += right.g;
            left.b += right.b;
            left.a += right.a;
            return left;
        }

        public static Color operator -(Color left, Color right)
        {
            left.r -= right.r;
            left.g -= right.g;
            left.b -= right.b;
            left.a -= right.a;
            return left;
        }

        public static Color operator -(Color color)
        {
            return Color.White - color;
        }

        public static Color operator *(Color color, float scale)
        {
            color.r *= scale;
            color.g *= scale;
            color.b *= scale;
            color.a *= scale;
            return color;
        }

        public static Color operator *(float scale, Color color)
        {
            color.r *= scale;
            color.g *= scale;
            color.b *= scale;
            color.a *= scale;
            return color;
        }

        public static Color operator *(Color left, Color right)
        {
            left.r *= right.r;
            left.g *= right.g;
            left.b *= right.b;
            left.a *= right.a;
            return left;
        }

        public static Color operator /(Color color, float scale)
        {
            color.r /= scale;
            color.g /= scale;
            color.b /= scale;
            color.a /= scale;
            return color;
        }

        public static Color operator /(Color left, Color right)
        {
            left.r /= right.r;
            left.g /= right.g;
            left.b /= right.b;
            left.a /= right.a;
            return left;
        }

        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Color left, Color right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(Color left, Color right)
        {
            if (Math.Approx(left.r, right.r))
            {
                if (Math.Approx(left.g, right.g))
                {
                    if (Math.Approx(left.b, right.b))
                    {
                        return left.a < right.a;
                    }
                    return left.b < right.b;
                }
                return left.g < right.g;
            }
            return left.r < right.r;
        }

        public static bool operator >(Color left, Color right)
        {
            if (Math.Approx(left.r, right.r))
            {
                if (Math.Approx(left.g, right.g))
                {
                    if (Math.Approx(left.b, right.b))
                    {
                        return left.a > right.a;
                    }
                    return left.b > right.b;
                }
                return left.g > right.g;
            }
            return left.r > right.r;
        }

        /// <summary>
        /// Returns <see langword="true"/> if this color and <paramref name="obj"/> are equal.
        /// </summary>
        /// <param name="obj">The other object to compare.</param>
        /// <returns>Whether or not the color and the other object are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Color)
            {
                return Equals((Color)obj);
            }

            return false;
        }

        /// <summary>
        /// Returns <see langword="true"/> if this color and <paramref name="other"/> are equal
        /// </summary>
        /// <param name="other">The other color to compare.</param>
        /// <returns>Whether or not the colors are equal.</returns>
        public bool Equals(Color other)
        {
            return r == other.r && g == other.g && b == other.b && a == other.a;
        }

        /// <summary>
        /// Returns <see langword="true"/> if this color and <paramref name="other"/> are approximately equal,
        /// by running <see cref="Math.IsEqualApprox(float, float)"/> on each component.
        /// </summary>
        /// <param name="other">The other color to compare.</param>
        /// <returns>Whether or not the colors are approximately equal.</returns>
        public bool Approx(Color other)
        {
            return Math.Approx(r, other.r) && Math.Approx(g, other.g) && Math.Approx(b, other.b) && Math.Approx(a, other.a);
        }

        /// <summary>
        /// Serves as the hash function for <see cref="Color"/>.
        /// </summary>
        /// <returns>A hash code for this color.</returns>
        public override int GetHashCode()
        {
            return r.GetHashCode() ^ g.GetHashCode() ^ b.GetHashCode() ^ a.GetHashCode();
        }

        /// <summary>
        /// Converts this <see cref="Color"/> to a string.
        /// </summary>
        /// <returns>A string representation of this color.</returns>
        public override string ToString()
        {
            return $"({r}, {g}, {b}, {a})";
        }

        /// <summary>
        /// Converts this <see cref="Color"/> to a string with the given <paramref name="format"/>.
        /// </summary>
        /// <returns>A string representation of this color.</returns>
        public string ToString(string format)
        {
            return $"({r.ToString(format)}, {g.ToString(format)}, {b.ToString(format)}, {a.ToString(format)})";
        }

        public static readonly Color Clear = new Color(0f, 0f, 0f, 0f);

        public static readonly Color Black = new Color(0f, 0f, 0f, 1f);
        public static readonly Color White = new Color(1f, 1f, 1f, 1f);

        public static readonly Color DarkGray = new Color(0.25f, 0.25f, 0.25f, 1f);
        public static readonly Color Gray = new Color(0.5f, 0.5f, 0.5f, 1f);
        public static readonly Color LightGray = new Color(0.75f, 0.75f, 0.75f, 1f);

        public static readonly Color DarkGrey = new Color(0.25f, 0.25f, 0.25f, 1f);
        public static readonly Color Grey = new Color(0.5f, 0.5f, 0.5f, 1f);
        public static readonly Color LightGrey = new Color(0.75f, 0.75f, 0.75f, 1f);

        public static readonly Color Red = new Color(1f, 0f, 0f, 1f);
        public static readonly Color Green = new Color(0f, 1f, 0f, 1f);
        public static readonly Color Blue = new Color(0f, 0f, 1f, 1f);

        public static readonly Color Yellow = new Color(1f, 1f, 0f, 1f);
        public static readonly Color Cyan = new Color(0f, 1f, 1f, 1f);
        public static readonly Color Magenta = new Color(1f, 0f, 1f, 1f);
    }
}
