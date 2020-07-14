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

namespace MethodUpdateChecker
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ICSharpCode.Decompiler;
    using ICSharpCode.Decompiler.CSharp;
    using ICSharpCode.Decompiler.TypeSystem;

    class MethodUpdateChecker
    {
        private static string getMethod(string id, CSharpDecompiler decompiler)
        {
            string[] split = id.Split("|");
            string type = split[0];
            string method = split[1];
            string[] param = split[2].Split(",");

            try
            {
                return decompiler.DecompileAsString(decompiler.TypeSystem.MainModule.Compilation.FindType(new FullTypeName(type)).GetDefinition().Methods.Where(x => x.Name.Equals(method) && (!x.Parameters.Any() || (x.Parameters.ToList().Select(y => y.Type.ReflectionName).SequenceEqual(param)))).First().MetadataToken);
            }
            catch(InvalidOperationException)
            {
                return "-1";
            }
        }

        public static Boolean compare(string id, CSharpDecompiler decompiler1, CSharpDecompiler decompiler2)
        {
            try
            {
                string method1 = getMethod(id, decompiler2);
                string method2 = getMethod(id, decompiler1);
                if (method1 == "-1" || method2 == "-1")
                {
                    return false;
                }

                return method1.Equals(method2);
            }
            catch(Exception)
            {
                return false;
            }
        }

        public static Boolean[] compare(string[] ids, CSharpDecompiler decompiler1, CSharpDecompiler decompiler2)
        {
            List<Boolean> list = new List<Boolean>();

            for (int i = 0; i < ids.Length; i++)
            {
                list.Add(compare(ids[i], decompiler1, decompiler2));
            }

            return list.ToArray();
        }

        public static Boolean compare(string id, string decompiler1t, string decompiler2t)
        {
            CSharpDecompiler decompiler1 = new CSharpDecompiler(decompiler1t, new DecompilerSettings());
            CSharpDecompiler decompiler2 = new CSharpDecompiler(decompiler2t, new DecompilerSettings());

            return compare(id, decompiler1, decompiler2);
        }

        public static Boolean[] compare(string[] ids, string decompiler1t, string decompiler2t)
        {
            CSharpDecompiler decompiler1 = new CSharpDecompiler(decompiler1t, new DecompilerSettings());
            CSharpDecompiler decompiler2 = new CSharpDecompiler(decompiler2t, new DecompilerSettings());

            List<Boolean> list = new List<Boolean>();

            for (int i = 0; i < ids.Length; i++)
            {
                list.Add(compare(ids[i], decompiler1, decompiler2));
            }

            return list.ToArray();
        }

        public static string[] compareRich(string[] ids, string decompiler1t, string decompiler2t)
        {
            CSharpDecompiler decompiler1 = new CSharpDecompiler(decompiler1t, new DecompilerSettings());
            CSharpDecompiler decompiler2 = new CSharpDecompiler(decompiler2t, new DecompilerSettings());

            List<string> list = new List<string>();

            for (int i = 0; i < ids.Length; i++)
            {
                if (!compare(ids[i], decompiler1, decompiler2))
                {
                    string[] split = ids[0].Split("|");
                    list.Add(split[0] + "." + split[1] + "(" + String.Join(", ", split[2].Split(",")) + ")");
                }
            }

            return list.ToArray();
        }

        public static string compareRich(string id, string decompiler1t, string decompiler2t)
        {
            CSharpDecompiler decompiler1 = new CSharpDecompiler(decompiler1t, new DecompilerSettings());
            CSharpDecompiler decompiler2 = new CSharpDecompiler(decompiler2t, new DecompilerSettings());

            if (!compare(id, decompiler1, decompiler2))
            {
                string[] split = id.Split("|");
                return split[0] + "." + split[1] + "(" + String.Join(", ", split[2].Split(",")) + ")";
            }
            return "";
        }

        public static string[] compareRich(string[] ids, CSharpDecompiler decompiler1, CSharpDecompiler decompiler2)
        {
            List<string> list = new List<string>();

            for (int i = 0; i < ids.Length; i++)
                if (!compare(ids[i], decompiler1, decompiler2))
                {
                    string[] split = ids[0].Split("|");
                    list.Add(split[0] + "." + split[1] + "(" + String.Join(", ", split[2].Split(",")) + ")");
                }

            return list.ToArray();
        }

        public static string compareRich(string id, CSharpDecompiler decompiler1, CSharpDecompiler decompiler2)
        {
            if (!compare(id, decompiler1, decompiler2))
            {
                string[] split = id.Split("|");
                return split[0] + "." + split[1] + "(" + String.Join(", ", split[2].Split(",")) + ")";
            }
            return "";
        }

        private static void Main(string[] args)
        {
            if(args.Length != 3)
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
                Console.WriteLine(compareRich(text, decompiler, decompiler2));
                counter++;
            }
            file.Close();
        }
    }
}
