using System;
using System.Collections.Generic;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED.Effects
{
    public static class LEDEffectUtility
    {
        public const int LEDByteCount = 4;

        /// <summary>
        /// Given a byte array <paramref name="bytes"/>, update all bytes to store the given <paramref name="color"/>.
        /// </summary>
        /// <param name="color">The desired color.</param>
        /// <param name="bytes">The byte array to write to.</param>
        public static void WriteColorToBytes(Color32 color, byte[] bytes)
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
        /// <param name="color">The desired color.</param>
        /// <param name="arraySegment">The array segment to write to.</param>
        public static void WriteColorToBytes(Color32 color, ArraySegment<byte> arraySegment)
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
        /// Given a byte array <paramref name="bytes"/>, convert to and return a list of 
        /// <see cref="Color32"/>.
        /// </summary>
        /// <param name="bytes">The byte array to unpack.</param>
        /// <returns></returns>
        public static List<Color32> ConvertBytesToColors(byte[] bytes)
        {
            List<Color32> colors = new List<Color32>();
            for (int i = 0; i < bytes.Length; i++)
            {
                // Create a color from 4 bits of byte data
                Color32 color = new Color32
                {
                    r = bytes[i++],
                    g = bytes[i++],
                    b = bytes[i++],
                    a = bytes[i]
                };
                colors.Add(color);
            }
            return colors;
        }

        /// <summary>
        /// Given an array segment <paramref name="arraySegment"/>, convert to and return a list of
        /// <see cref="Color32"/>.
        /// </summary>
        /// <param name="arraySegment">The array segment to unpack.</param>
        /// <returns></returns>
        public static List<Color32> ConvertBytesToColors(ArraySegment<byte> arraySegment)
        {
            List<Color32> colors = new List<Color32>();
            for (int i = arraySegment.Offset; i < arraySegment.Offset + arraySegment.Count; i++)
            {
                // Create a color from 4 bits of byte data
                Color32 color = new Color32
                {
                    r = arraySegment.Array[i++],
                    g = arraySegment.Array[i++],
                    b = arraySegment.Array[i++],
                    a = arraySegment.Array[i]
                };
                colors.Add(color);
            }
            return colors;
        }
    }
}