using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DataTableGeneratorMenu
{
    [MenuItem("Tools/Generate DataTables")]
    private static void GenerateDataTables()
    {
        foreach (string dataTableName in GetDataTableNames())
        {
            DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(dataTableName);
            if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
            {
                Debug.LogError(Utility.TextUtility.Format("Check raw data failure. DataTableName='{0}'",
                    dataTableName));
                break;
            }

            DataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName);
            DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
        }

        AssetDatabase.Refresh();
    }

    private static string[] GetDataTableNames()
    {
        string dataTablesPath = Application.dataPath + @"/FastDev/DataTables";
        DirectoryInfo directoryInfo = new DirectoryInfo(dataTablesPath);
        FileInfo[] fis = directoryInfo.GetFiles("*.txt", SearchOption.AllDirectories);
        string[] dataTableNames = new string[fis.Length];
        for (int i = 0; i < fis.Length; i++)
        {
            dataTableNames[i] = Path.GetFileNameWithoutExtension(fis[i].Name);
        }

        return dataTableNames;
    }
}