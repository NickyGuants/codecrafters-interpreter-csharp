using System;
using System.IO;
using interpreter;

if (args.Length < 2)
{
    Console.Error.WriteLine("Usage: ./your_program.sh tokenize <filename>");
    Environment.Exit(1);
}

string command = args[0];
string filename = args[1];

if (command != "tokenize")
{
    Console.Error.WriteLine($"Unknown command: {command}");
    Environment.Exit(1);
}

string fileContents = File.ReadAllText(filename);

Scanner scanner = new Scanner(fileContents);
var tokens = scanner.scanTokens();

foreach (var token in tokens)
{
    Console.WriteLine(token.ToString());
}

if (scanner.HasError())
{
    Environment.Exit(65);
}