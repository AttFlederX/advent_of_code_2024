﻿var lines = File.ReadAllLines("input.txt");
var start = DateTime.Now;

var ram = new Ram(71, lines);
var res = ram.FindShortestEscapeRoute(1024);

var duration = DateTime.Now - start;
Console.WriteLine($"\nResult: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");