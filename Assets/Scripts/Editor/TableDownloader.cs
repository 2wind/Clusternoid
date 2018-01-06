using System.IO;
using UnityEditor;
using UnityEngine;

public static class TableDownloader
{
    const string downloadPath =
        "https://docs.google.com/spreadsheets/d/1iOS5X_h2er0lhocMC93S2iB_oz_JMDFOyUn-sICc1sU/export?format=tsv&gid=0";

    [MenuItem("Tools/Download Table")]
    public static void DownloadTable()
    {
        Download(downloadPath, $"{Application.dataPath}/Resources/Data/AttackTable.txt");
    }

    [MenuItem("Tools/Test DataTables")]
    public static void TestTable()
    {
        Debug.Log(DataTables.attacks["mini_bullet"].damage);
    }

    static void Download(string path, string writePath)
    {
        var download = new WWW(path);
        while (!download.isDone) EditorUtility.DisplayProgressBar("Downloading", path, download.progress);
        EditorUtility.ClearProgressBar();
        var writeDirectory = Path.GetDirectoryName(writePath);
        if (!Directory.Exists(writeDirectory))
        {
            Debug.Assert(writeDirectory != null, nameof(writeDirectory) + " != null");
            Directory.CreateDirectory(writeDirectory);
        }
        File.WriteAllText(writePath, download.text);
    }
}