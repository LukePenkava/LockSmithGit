using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Collections.Generic;

public static class SaveSystem 
{
    static string path = Application.persistentDataPath + "/data.ml";

    public static void SaveData(List<LevelObject> data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static List<LevelObject> LoadData()
    {
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            List<LevelObject> data = formatter.Deserialize(stream) as List<LevelObject>;
            
            stream.Close();           
            return data;
        }
        else
        {
            Debug.Log("Save File not Found");
            return null;
        }
    }

    public static void DeleteData()
    {
        File.Delete(path);
    }
}
