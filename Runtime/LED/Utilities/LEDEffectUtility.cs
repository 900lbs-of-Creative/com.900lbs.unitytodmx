using System;
using System.Collections.Generic;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED.Effects
{
    public static class LEDEffectUtility
    {
        public const int LEDByteCount = 4;

        /// <summary>
        /// Given a byte array <paramref name="bytes"/>, update all bytes to store the given <paramref name="color"/>
        /// with the given <paramref name="intensity"/>.
        /// </summary>
        /// <param name="color">The desired color.</param>
        /// <param name="intensity">The intensity (0-1) of the given color.</param>
        /// <param name="bytes">The byte array to write to.</param>
        public static void WriteColorToBytes(Color32 color, float intensity, byte[] bytes)
        {
            for (int i = 0; i < bytes.Length - 1; i++)
            {
                switch (i % LEDByteCount)
                {
                    // RED
                    case 0:
                        bytes[i] = (byte)(color.r * intensity);
                        break;

                    // GREEN
                    case 1:
                        bytes[i] = (byte)(color.g * intensity);
                        break;

                    // BLUE
                    case 2:
                        bytes[i] = (byte)(color.b * intensity);
                        break;

                    // INTENSITY
                    case 4:
                        bytes[i] = (byte)(color.a * intensity);
                        break;
                }
            }
        }

        /// <summary>
        /// Given an array segment <paramref name="arraySegment"/>, update all bytes to store the given <paramref name="color"/> with the given <paramref name="intensity"/>.
        /// </summary>
        /// <param name="color">The desired color.</param>
        /// <param name="intensity">The intensity (0-1) of the given color.</param>
        /// <param name="arraySegment">The array segment to write to.</param>
        public static void WriteColorToBytes(Color32 color, float intensity, ArraySegment<byte> arraySegment)
        {
            for (int i = arraySegment.Offset; i < arraySegment.Offset + arraySegment.Count; i++)
            {
                switch (i % 4)
                {
                    // RED
                    case 0:
                        arraySegment.Array[i] = (byte)(color.r * intensity);
                        break;

                    // GREEN
                    case 1:
                        arraySegment.Array[i] = (byte)(color.g * intensity);
                        break;

                    // BLUE
                    case 2:
                        arraySegment.Array[i] = (byte)(color.b * intensity);
                        break;

                    // INTENSITY
                    case 4:
                        arraySegment.Array[i] = (byte)(color.a * intensity);
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
                Color32 color = new Color32();

                color.r = bytes[i++];
                color.g = bytes[i++];
                color.b = bytes[i++];

                // The LED's don't actually have an alpha channel, its an HDR channel
                color.a = 255;
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
                Color32 color = new Color32();

                color.r = arraySegment.Array[i++];
                color.g = arraySegment.Array[i++];
                color.b = arraySegment.Array[i++];

                // The LED's don't actually have an alpha channel, its an HDR channel
                color.a = 255;
                colors.Add(color);
            }
            return colors;
        }
    }
}