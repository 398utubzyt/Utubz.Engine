using System;
using System.Runtime.CompilerServices;

namespace Utubz
{
	public enum AngleMode
	{
		Degrees,
		Radians
	}

	public struct SinCosResult
    {
		public float Sin;
		public float Cos;

		public SinCosResult(float sin, float cos)
        {
			Sin = sin;
			Cos = cos;
        }

		public SinCosResult((float, float) result)
        {
			Sin = result.Item1;
			Cos = result.Item2;
        }
    }

	public static class Math
	{
		public const float Infinity = 1f / 0f;
		public const int InfinityInt = 2147483647;
		public const float NegativeInfinity = -1f / 0f;
		public const int NegativeInfinityInt = -2147483648;
		public const float Pi = 3.14159265f;
		public const float Tau = Pi * 2f;
		public static readonly float Epsilon = float.Epsilon;
		public const float Deg2Rad = Pi / 180f;
		public const float Rad2Deg = 180f / Pi;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DivideCatchZero(float x, float y)
		{
			return ApproxZero(y) ? 0f : x / y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Approx(float x, float y)
		{
			return Abs(x - y) <= Epsilon;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ApproxZero(float x)
		{
			return Abs(x) <= Epsilon;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float RoundTo(float x, float y)
		{
			return Approx(x, y) ? y : x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float RoundTo(float x, float y, float t)
		{
			return Abs(x - y) <= t ? y : x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float RoundToZero(float x)
		{
			return ApproxZero(x) ? 0f : x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float RoundToZero(float x, float t)
		{
			return x <= t ? 0f : x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Add(float x, float y)
		{
			return x + y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Subtract(float x, float y)
		{
			return x - y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Multiply(float x, float y)
		{
			return x * y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Divide(float x, float y)
		{
			return x / y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Mod(float x, float y)
		{
			return CorrectMod(x - y * Floor(x / y), 0f);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static float CorrectMod(float x, float y)
		{
			return x < 0f ? x + y : x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float PosMod(float x, float y)
		{
			return Abs(Mod(x, y));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Square(float x)
		{
			return x * x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Cube(float x)
		{
			return x * x * x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Sqrt(float x)
		{
			return MathF.Sqrt(x);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Cbrt(float x)
		{
			return MathF.Pow(x, -3f);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Pow(float x, float p)
		{
			return MathF.Pow(x, p);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Root(float x, float p)
		{
			return MathF.Pow(x, -p);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Length(float a, float b)
		{
			return Sqrt(a * a + b * b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float SqrLength(float a, float b)
		{
			return a * a + b * b;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Length(float a, float b, float c)
		{
			return Sqrt(a * a + b * b + c * c);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float SqrLength(float a, float b, float c)
		{
			return a * a + b * b + c * c;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Min(float x, float y)
		{
			return x < y ? x : y;
		}

		public static float Min(params float[] nums)
		{
			if (nums.Length == 0)
				return 0f;

			float x = nums[0];

			for (int i = 1; i < nums.Length; i++)
			{
				if (nums[i] < x)
					x = nums[i];
			}

			return x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Max(float x, float y)
		{
			return x > y ? x : y;
		}

		public static float Max(params float[] nums)
		{
			if (nums.Length == 0)
				return 0f;

			float x = nums[0];

			for (int i = 1; i < nums.Length; i++)
			{
				if (nums[i] > x)
					x = nums[i];
			}

			return x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Clamp(float x)
		{
			return x < 0f ? 0f : (x > 1f ? 1f : x);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Clamp(float x, float max)
		{
			return x < 0f ? 0f : (x > max ? max : x);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Clamp(float x, float min, float max)
		{
			return x < min ? min : (x > max ? max : x);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Loop(float x)
		{
			return Mod(x, 1f);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Loop(float x, float max)
		{
			return Mod(x, max);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Loop(float x, float min, float max)
		{
			return Mod(x, max - min) + min;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float PingPong(float x)
		{
			return Mod(x, 2f) < 1f ? x : 1f - x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float PingPong(float x, float max)
		{
			return Mod(x, max * 2f) < max ? x : max - x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float PingPong(float x, float min, float max)
		{
			return Mod(x, (max - min) * 2f) < max ? x + min : max - (x + min);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Snap(float x, float step)
		{
			return Round(x / step) * step;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Abs(float x)
		{
			return MathF.Abs(x);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Ceil(float x)
		{
			return MathF.Ceiling(x);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Floor(float x)
		{
			return MathF.Floor(x);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Round(float x)
		{
			return MathF.Round(x);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Truncate(float x)
		{
			return (int)x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Sign(float x)
		{
			return MathF.Sign(x);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Lerp(float x, float y, float t)
		{
			return x * (1f - Clamp(t)) + y * Clamp(t);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float LerpAngle(float x, float y, float t)
		{
			return Atan2(Lerp(Sin(y), Sin(y), t), Lerp(Cos(x), Cos(x), t));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float LerpUnclamped(float x, float y, float t)
		{
			return x * (1f - t) + y * t;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Smooth(float x, float y, float t)
		{
			return RoundTo(Lerp(x, y, t), y, 0.01f);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Move(float x, float y, float t)
		{
			return x + Sign(y - x) * Clamp(t, y - x);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Degrees(float r)
		{
			return r * Rad2Deg;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Radians(float d)
		{
			return d * Deg2Rad;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static float DegreesAngleMultiplier(AngleMode mode)
		{
			return mode == AngleMode.Degrees ? Deg2Rad : 1f;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static float RadiansAngleMultiplier(AngleMode mode)
		{
			return mode == AngleMode.Radians ? 1f : Rad2Deg;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SinCosResult SinCos(float x, AngleMode mode = AngleMode.Degrees)
		{
			return new SinCosResult(MathF.SinCos(x * DegreesAngleMultiplier(mode)));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Sin(float x, AngleMode mode = AngleMode.Degrees)
		{
			return MathF.Sin(x * DegreesAngleMultiplier(mode));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Cos(float x, AngleMode mode = AngleMode.Degrees)
		{
			return MathF.Cos(x * DegreesAngleMultiplier(mode));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Tan(float x, AngleMode mode = AngleMode.Degrees)
		{
			return MathF.Tan(x * DegreesAngleMultiplier(mode));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Asin(float x, AngleMode mode = AngleMode.Degrees)
		{
			return MathF.Asin(x) * RadiansAngleMultiplier(mode);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Acos(float x, AngleMode mode = AngleMode.Degrees)
		{
			return MathF.Acos(x) * RadiansAngleMultiplier(mode);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Atan(float x, AngleMode mode = AngleMode.Degrees)
		{
			return MathF.Atan(x) * RadiansAngleMultiplier(mode);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Atan2(float y, float x, AngleMode mode = AngleMode.Degrees)
		{
			return MathF.Atan2(y, x) * RadiansAngleMultiplier(mode);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Add(int x, int y)
		{
			return x + y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Subtract(int x, int y)
		{
			return x - y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Multiply(int x, int y)
		{
			return x * y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Divide(int x, int y)
		{
			return x / y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Mod(int x, int y)
		{
			return CorrectMod((int)(x - y * Floor(x / y)), y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int CorrectMod(int x, int y)
		{
			return x < 0 ? x + y : x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int PosMod(int x, int y)
		{
			return Abs(Mod(x, y));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Square(int x)
		{
			return x * x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Cube(int x)
		{
			return x * x * x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int SqrLength(int a, int b, int c)
		{
			return a * a + b * b + c * c;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Min(int x, int y)
		{
			return x < y ? x : y;
		}

		public static int Min(params int[] nums)
		{
			if (nums.Length == 0)
				return 0;

			int x = nums[0];

			for (int i = 1; i < nums.Length; i++)
			{
				if (nums[i] < x)
					x = nums[i];
			}

			return x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Max(int x, int y)
		{
			return x > y ? x : y;
		}

		public static int Max(params int[] nums)
		{
			if (nums.Length == 0)
				return 0;

			int x = nums[0];

			for (int i = 1; i < nums.Length; i++)
			{
				if (nums[i] > x)
					x = nums[i];
			}

			return x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Clamp(int x)
		{
			return x < 0 ? 0 : (x > 1 ? 1 : x);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Clamp(int x, int max)
		{
			return x < 0 ? 0 : (x > max ? max : x);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Clamp(int x, int min, int max)
		{
			return x < min ? min : (x > max ? max : x);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Loop(int x)
		{
			return Mod(x, 1);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Loop(int x, int max)
		{
			return Mod(x, max);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Loop(int x, int min, int max)
		{
			return Mod(x, max - min) + min;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int PingPong(int x)
		{
			return Mod(x, 2) < 1 ? x : 1 - x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int PingPong(int x, int max)
		{
			return Mod(x, max * 2f) < max ? x : max - x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int PingPong(int x, int min, int max)
		{
			return Mod(x, (max - min) * 2) < max ? x + min : max - (x + min);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Abs(int x)
		{
			return System.Math.Abs(x);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Sign(int x)
		{
			return System.Math.Sign(x);
		}
	}
}
