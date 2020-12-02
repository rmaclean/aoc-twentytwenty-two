using System;
using System.IO;
using System.Linq;

var count = File.ReadLines("input.txt")
    .Select(line => new PasswordRecord(line, PasswordPolicy.Toboggan))
    .Where(password => password.Valid())
    .Count();
    
Console.WriteLine($"Valid password {count}");

