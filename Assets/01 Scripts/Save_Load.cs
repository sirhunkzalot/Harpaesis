using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Save_Load
{
    public static bool SaveGame(string fileName, Game_Data data)
    {
        string filePath = Application.persistentDataPath + "/" + fileName + ".Hrp";
        Debug.Log(filePath);
        FileStream FS = new FileStream(filePath, FileMode.Create);
        BinaryFormatter BF = new BinaryFormatter();
        BF.Serialize(FS, data);
        FS.Close();
        return true;
    }
}
