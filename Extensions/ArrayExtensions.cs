using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestCloud.Extensions
{
    internal static class ArrayExtensions
    {
        public static int[,] To2DArray(this List<List<int>> src)
        {
            // assumption is that this is a square matrix, no error handling implemented here!
            var size = src.Count;
            var ret = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                    ret[i, j] = src[i][j];
            }
            return ret;
        }

        public static string ToPlainString(this byte[] src)
        {
            return src.Select(b => b.ToString()).Aggregate((s1, s2) => s1 + s2);
        }
    }
}
