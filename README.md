# MethodUpdateChecker
MethodUpdateChecker is a program and library to see if a method changed between two assemblies. This lets you automatically compare two versions of a program and detect if a method is changed.

## Basic Usage
To use MethodUpdateChecker, you can run it with `MethodUpdateChecker.exe Assembly-1.dll Assembly-2.dll methods.txt` (you can use either a .dll or .exe). `methods.txt` is a text file containing every method that will be checked for changes, and looks like:
```
Namespace.Type|Method|System.String,System.Char
System.String|Join|System.Char,System.String[]
```
The second entry is for [String.Join](https://docs.microsoft.com/en-us/dotnet/api/system.string.join?view=netcore-3.1#System_String_Join_System_Char_System_String___), which looks like `String.Join(Char, String[])`.
It is very import to make sure the formatting is correct. Spaces should not be used and names must be fully qualified (`System.String` not `String`). If a namespace is not used, you can use just the type name (`TypeName|Method1|System.String`).

## Library Usage
