using InvestCloud.Extensions;
using InvestCloud.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InvestCloud.Business
{
    internal class Matrix
    {
        private readonly int[,] matrixA;
        private readonly int[,] matrixB;

        private int[,] matrixR;

        private readonly ApiHelper apiHelper;
        private readonly int numTasks;


        public Matrix(int size, int numTasks)
        {
            // init API
            this.numTasks = numTasks;
            apiHelper = new ApiHelper();
            apiHelper.Initialize(size).Wait();

            // load matrixA and matrixB
            var ma = load("A", size);
            matrixA = ma.To2DArray();
            var mb = load("B", size);
            matrixB = mb.To2DArray();
        }

        public void Multiply()
        {
            var size = matrixA.GetLength(0);

            matrixR = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var element = 0;
                    for (int k = 0; k < size; k++)
                    {
                        element += matrixA[i, k] * matrixB[k, j];
                    }
                    matrixR[i, j] = element;
                }
            }
        }

        public string Validate()
        {
            // create concatenated string from matrix
            var strResult = new StringBuilder();
            var size = matrixR.GetLength(0);
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    strResult.Append(matrixR[row, col]);
                }
            }
            // hash MD5 with UTF8
            using var md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(strResult.ToString());
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            // send to API
            var result = apiHelper.Validate(hashBytes).Result;

            return result;
        }

        private List<List<int>> load(string dataset, int size)
        {
            var data = new List<List<int>>();
            var loaders = new List<Task<RowResponse>>();
            for (int idx = 0; idx < size; idx++)
            {
                loaders.Add(createLoader(dataset, idx));
                if (loaders.Count == numTasks)
                {
                    executeTasks(data, loaders);
                }
            }
            executeTasks(data, loaders);

            return data;
        }

        private Task<RowResponse> createLoader(string dataset, int idx)
        {
            return apiHelper.GetRow(dataset, idx);
        }

        private void executeTasks(List<List<int>> data, List<Task<RowResponse>> loaders)
        {
            var result = new List<List<int>>();
            Task.WaitAll(loaders.ToArray());
            loaders.ForEach(task =>
            {
                result.Add(task.Result.Value.ToList());
            });
            data.AddRange(result);
            loaders.Clear();
        }

    }
}
