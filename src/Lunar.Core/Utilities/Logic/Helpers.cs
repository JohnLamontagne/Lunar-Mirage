﻿/** Copyright 2018 John Lamontagne https://www.rpgorigin.com

	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
*/

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Lunar.Core.Utilities.Data;

namespace Lunar.Core.Utilities.Logic
{
    public static class Helpers
    {
        /// <summary>
        /// Lineraly interpolates between two values.
        /// </summary>
        /// <param name="valueOne"></param>
        /// <param name="valueTwo"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        /// Credits to: http://msdn.microsoft.com/en-us/library/microsoft.xna.framework.mathhelper.lerp.aspx
        public static float Lerp(float valueOne, float valueTwo, float amount)
        {
            return valueOne + (valueTwo - valueOne) * amount;
        }

        public static double DegreesToRadians(double degrees)
        {
            return (float)((degrees / 360) * (2 * Math.PI));
        }

        public static double RadiansToDegrees(double radians)
        {
            return (float)((radians / (2 * Math.PI)) * 360);
        }

        public static byte[] Hash(string value, string salt)
        {
            return Hash(Encoding.UTF8.GetBytes(value), Encoding.UTF8.GetBytes(salt));
        }

        public static byte[] Hash(byte[] value, byte[] salt)
        {
            byte[] saltedValue = value.Concat(salt).ToArray();

            return new SHA256Managed().ComputeHash(saltedValue);
        }

        public static float RoundDown(float i, double decimalPlaces)
        {
            var power = (float)Math.Pow(10, decimalPlaces);
            return (float)Math.Floor(i * power) / power;
        }

        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            return (value.CompareTo(min) < 0 ? min : (value.CompareTo(max) > 0 ? max : value));
        }

        public static float SmoothStep(float edge0, float edge1, float x)
        {
            // Scale, bias and saturate x to 0..1 range
            x = Helpers.Clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
            // Evaluate polynomial
            return x * x * (3 - 2 * x);
        }

        public static bool IsWithin<T>(this T value, T minimum, T maximum) where T : IComparable<T>
        {
            if (value.CompareTo(minimum) < 0)
                return false;
            if (value.CompareTo(maximum) > 0)
                return false;
            return true;
        }

        public static T[,] ResizeArray<T>(T[,] original, int rows, int cols)
        {
            var newArray = new T[rows, cols];
            int minRows = Math.Min(rows, original.GetLength(0));
            int minCols = Math.Min(cols, original.GetLength(1));
            for (int i = 0; i < minRows; i++)
                for (int j = 0; j < minCols; j++)
                    newArray[i, j] = original[i, j];
            return newArray;
        }

        public static string MakeRelative(string filePath, string referencePath)
        {
            var fileUri = new Uri(filePath);
            var referenceUri = new Uri(referencePath);
            return referenceUri.MakeRelativeUri(fileUri).ToString();
        }

        public static Vector WorldToMapCoords(Vector worldCoords, int tileSize)
        {
            return new Vector(worldCoords.X / tileSize, worldCoords.Y / tileSize);
        }

        public static Vector Abs(Vector vector)
        {
            return new Vector(Math.Abs(vector.X), Math.Abs(vector.Y));
        }

        public static bool IsSubDirectoryOf(this string candidate, string other)
        {
            var isChild = false;
            try
            {
                var candidateInfo = new DirectoryInfo(candidate);
                var otherInfo = new DirectoryInfo(other);

                while (candidateInfo.Parent != null)
                {
                    if (candidateInfo.Parent.FullName == otherInfo.FullName)
                    {
                        isChild = true;
                        break;
                    }
                    else candidateInfo = candidateInfo.Parent;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return isChild;
        }

        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToUpperInvariant();
        }

        public static Vector WorldToMapCoords(Vector worldCoords)
        {
            return worldCoords / EngineConstants.TILE_SIZE;
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string GetNextAvailableFilename(string fullFileName)
        {
            if (!File.Exists(fullFileName)) return fullFileName;

            string alternateFilename;
            int fileNameIndex = 1;
            do
            {
                fileNameIndex += 1;
                string plainName = Path.GetFileNameWithoutExtension(fullFileName);
                string extension = Path.GetExtension(fullFileName);
                alternateFilename = string.Format("{0}{1}{2}", plainName, fileNameIndex, extension);
            } while (File.Exists(alternateFilename));

            return alternateFilename;
        }
    }
}