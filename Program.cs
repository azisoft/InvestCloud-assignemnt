// See https://aka.ms/new-console-template for more information
using InvestCloud;
using System.Diagnostics;

using InvestCloud.Business;

const int size = 1000; 
const int numTasks = 500;

var swTotal = new Stopwatch();
swTotal.Start();
var sw = new Stopwatch();

// load matrices
Console.WriteLine("Loading data...");
sw.Start();
var matrix = new Matrix(size, numTasks);
sw.Stop();
Console.WriteLine($"Loaded 2 square matrices of size {size} x {size}, with {numTasks} concurrent requests at a time. Elapsed time: {sw.Elapsed}.");

// multiply matrices
Console.WriteLine("Multiplying matrices...");
sw = new Stopwatch();
sw.Start();
matrix.Multiply();
sw.Stop();
Console.WriteLine($"Multiply complete. Elapsed time?: {sw.Elapsed}.");

// validate result
Console.WriteLine("Validating result...");
sw = new Stopwatch();
sw.Start();
var validationResult = matrix.Validate();
sw.Stop();
Console.WriteLine($"Validation returned \"{validationResult}\". Elapsed time?: {sw.Elapsed}.");

swTotal.Stop();
Console.WriteLine($"Total elapsed time: {swTotal.Elapsed}.");
