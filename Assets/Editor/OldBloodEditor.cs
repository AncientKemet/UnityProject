using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class OldBloodEditor
{

    private static string BeginFileMacro = "#if SERVER";
    private static string EndFileMacro = "#if SERVER";

    [MenuItem("OldBlood/ServerFiles")]
    private static void Do()
    {
        List<string> Directories = new List<string>();
        List<string> Files = new List<string>();
        GetDirectories(Application.dataPath+"/Code/Core/Server", Directories);
        
        foreach (var dir in Directories)
        {
            GetCss(dir, Files);
        }

        foreach (var file in Files)
        {
            EnsureMacro(file);
        }
    }

    private static void EnsureMacro(string file)
    {
        string[] lines = File.ReadAllLines(file);

        string firstLine = lines.First();
        string lastLine = lines.Last();

        if (firstLine != BeginFileMacro)
        {
            List<string> newLines = new List<string>(lines);
            newLines.Insert(0, BeginFileMacro);
            File.WriteAllLines(file, newLines.ToArray());
        }

        if (lastLine != EndFileMacro)
        {
            List<string> newLines = new List<string>(lines);
            newLines.Add(EndFileMacro);
            File.WriteAllLines(file, newLines.ToArray());
        }
    }

    private static void GetDirectories(string path, List<string> all)
    {
        string[] dirs = Directory.GetDirectories(path, "*");
        all.AddRange(dirs);

        foreach (var directory in dirs)
        {
            GetDirectories(directory, all);
        }
    }

    private static void GetCss(string path, List<string> all)
    {
        string[] files = Directory.GetFiles(path, "*.cs");
        all.AddRange(files);
    }
}
