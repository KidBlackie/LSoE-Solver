using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSoE_Solver
{
    /*
    class Program
    {
        private static string ProgramDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        static void Main()
        {
            Matrixd sample1 = new Matrixd(2, 3, new double[,] { { 1, 2, 3 }, { 2, 1, 3 } });
            Matrixd res1;
            var res1Dir = GetVariableDir(nameof(res1));
            var sample1Dir = GetVariableDir(nameof(sample1));
            FileIo.SaveMatrixData(sample1Dir, sample1);
            res1 = sample1.GetReducedMatrix<Matrixd>(sample1);
            FileIo.SaveMatrixData(res1Dir, res1);
            Console.WriteLine("Reduce Results: ");
            Console.WriteLine(sample1);
            Console.WriteLine(res1);
            sample1 = null;
            res1 = null;
            Console.WriteLine("Reset Results: ");
            Console.WriteLine(sample1);
            Console.WriteLine(res1);
            Console.WriteLine("Load Results: ");
            sample1 = FileIo.LoadMatrixData<Matrixd, double>(sample1Dir);
            res1 = FileIo.LoadMatrixData<Matrixd, double>(res1Dir);
            Console.WriteLine(sample1);
            Console.WriteLine(res1);
        }

        static string GetVariableDir(string variableName)
        {
            return Path.Combine(ProgramDir, FileIo.AppendExtension(variableName));
        }
    }
    */
}
