/*----------------------
Copyright 2020 PintTheDragon
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
    http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
----------------------*/

using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using System;
using System.IO;

class Program
{
    private static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.Write("Usage: MethodUpdateChecker.exe Assembly-1.dll Assembly-2.dll methods.txt");
            return;
        }

        CSharpDecompiler decompiler = new CSharpDecompiler(args[0], new DecompilerSettings());
        CSharpDecompiler decompiler2 = new CSharpDecompiler(args[1], new DecompilerSettings());
        int counter = 0;
        string line;
        StreamReader file = new StreamReader(args[2]);
        while ((line = file.ReadLine()) != null)
        {
            string text = line;
            Console.WriteLine(MethodUpdateChecker.MethodUpdateChecker.compareRich(text, decompiler, decompiler2));
            counter++;
        }
        file.Close();
    }
}
