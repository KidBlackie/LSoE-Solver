using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Complex;
using Newtonsoft.Json;

namespace LSoE_Solver
{
    public static class FileIo
    {
        /// <summary>
        /// Reads in a file containing json text and serializes it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValType"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T LoadMatrixData<T, TValType>(string filePath) where T : MatrixBase<TValType> 
            where TValType : unmanaged, IComparable<TValType>, IComparable, IEquatable<TValType>, IConvertible
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException();
            using var fs = File.OpenRead(filePath);
            using var reader = new StreamReader(fs);
            string json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(json) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Serializes the object into json and saves it to the filepath provided.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="matrix"></param>
        public static void SaveMatrixData<T>(string filePath, MatrixBase<T> matrix) where T : unmanaged, IComparable<T>, IComparable, IEquatable<T>, IConvertible
        {
            FileInfo fi = new FileInfo(filePath);
            if (fi.DirectoryName == null) throw new DirectoryNotFoundException();
            if (!Directory.Exists(fi.DirectoryName)) Directory.CreateDirectory(fi.DirectoryName);
            using var fs = File.Create(fi.FullName);
            using var writer = new StreamWriter(fs);
            var json = JsonConvert.SerializeObject(matrix, Formatting.Indented);
            writer.WriteLine(json);
        }

        public static string AppendExtension(string fileName)
        {
            return fileName + ".json";
        }
    }
}
