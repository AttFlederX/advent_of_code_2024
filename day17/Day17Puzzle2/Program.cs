var lines = File.ReadAllLines("test.txt");
var start = DateTime.Now;

var cpu = new Cpu(lines.Take(3).ToArray());
var program = lines.Last().Split(": ").Last();

cpu.Disasm(program);

var res = cpu.RunProgram(program);

var duration = DateTime.Now - start;
Console.WriteLine($"\nResult: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");