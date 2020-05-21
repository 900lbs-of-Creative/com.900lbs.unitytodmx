using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace NineHundredLbs.UnitytoDMX.LED
{
    public static class LEDEffectUtils
    {
        #region Constants
        public const int LEDByteCount = 4;
        #endregion

        #region Public Methods
        /// <summary>
        /// Given a byte array <paramref name="bytes"/>, update all bytes to store the given <paramref name="color"/>.
        /// </summary>
        /// <param name="bytes">The byte array to write to.</param>
        /// <param name="color">The desired color.</param>
        public static void WriteColorToBytes(byte[] bytes, Color32 color)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                switch (i % LEDByteCount)
                {
                    // RED
                    case 0:
                        bytes[i] = color.r;
                        break;

                    // GREEN
                    case 1:
                        bytes[i] = color.g;
                        break;

                    // BLUE
                    case 2:
                        bytes[i] = color.b;
                        break;

                    // INTENSITY
                    case 3:
                        bytes[i] = color.a;
                        break;
                }
            }
        }

        /// <summary>
        /// Given an array segment <paramref name="arraySegment"/>, update all bytes to store the given <paramref name="color"/>.
        /// </summary>
        /// <param name="arraySegment">The array segment to write to.</param>
        /// <param name="color">The desired color.</param>
        public static void WriteColorToBytes(ArraySegment<byte> arraySegment, Color32 color)
        {
            for (int i = arraySegment.Offset; i < arraySegment.Offset + arraySegment.Count; i++)
            {
                switch (i % 4)
                {
                    // RED
                    case 0:
                        arraySegment.Array[i] = color.r;
                        break;

                    // GREEN
                    case 1:
                        arraySegment.Array[i] = color.g;
                        break;

                    // BLUE
                    case 2:
                        arraySegment.Array[i] = color.b;
                        break;

                    // ALPHA
                    case 3:
                        arraySegment.Array[i] = color.a;
                        break;
                }
            }
        }
        
        /// <summary>
        /// Given a byte array <paramref name="bytes"/>, convert to and populate <paramref name="colors"/> with
        /// values.
        /// </summary>
        /// <param name="bytes">The byte array to unpack.</param>
        /// <param name="colors">The array of colors to unpack into.</param>
        /// <returns></returns>
        public static void TryConvertBytesToColors(byte[] bytes, ref Color32[] colors)
        {
            Assert.IsTrue(bytes.Length % LEDByteCount == 0);
            for (int i = 0; i < bytes.Length; i++)
            {
                colors[i / LEDByteCount] = new Color32
                {
                    r = bytes[i++],
                    g = bytes[i++],
                    b = bytes[i++],
                    a = bytes[i]
                };
            }
        }

        /// <summary>
        /// Given an array segment <paramref name="arraySegment"/>, convert to and populate <paramref name="colors"/> with
        /// values.
        /// </summary>
        /// <param name="arraySegment">The array segment to unpack.</param>
        /// <param name="colors">The array of colors ot unpack into.</param>
        /// <returns></returns>
        public static bool TryConvertBytesToColors(ArraySegment<byte> arraySegment, ref Color32[] colors)
        {
            Assert.IsTrue(arraySegment.Count % LEDByteCount == 0);
            for (int i = arraySegment.Offset; i < arraySegment.Offset + arraySegment.Count; i++)
            {
                colors[(i - arraySegment.Offset) / LEDByteCount] = new Color32
                {
                    r = arraySegment.Array[i++],
                    g = arraySegment.Array[i++],
                    b = arraySegment.Array[i++],
                    a = arraySegment.Array[i]
                };
            }
            return true;
        }

        /// <summary>
        /// Given a byte array <paramref name="bytes"/>, convert to and return a <see cref="Color32"/>.
        /// </summary>
        /// <param name="bytes">The byte array to unpack.</param>
        /// <returns></returns>
        public static Color32 ConvertBytesToColor(byte[] bytes)
        {
            Assert.IsTrue(bytes.Length == 4);
            return new Color32
            {
                r = bytes[0],
                g = bytes[1],
                b = bytes[2],
                a = bytes[3]
            };
        }

        /// <summary>
        /// Given an array segment <paramref name="arraySegment"/>, convert to and return a <see cref="Color32"/>.
        /// </summary>
        /// <param name="arraySegment">The array segment to unpack.</param>
        /// <returns></returns>
        public static Color32 ConvertBytesToColor(ArraySegment<byte> arraySegment)
        {
            Assert.IsTrue(arraySegment.Count == LEDByteCount);
            return new Color32
            {
                r = arraySegment.Array[arraySegment.Offset],
                g = arraySegment.Array[arraySegment.Offset + 1],
                b = arraySegment.Array[arraySegment.Offset + 2],
                a = arraySegment.Array[arraySegment.Offset + 3]
            };
        }
        #endregion
    }
}