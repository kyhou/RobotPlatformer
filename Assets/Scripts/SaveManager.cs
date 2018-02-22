using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{

    public static void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "save.sav", FileMode.Create);

        DataSave dataSave = new DataSave(GameManager.Instance);
        bf.Serialize(stream, dataSave);

        stream.Close();
    }

    public static DataSave LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "save.sav")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "save.sav", FileMode.Open);

            DataSave dataSave = bf.Deserialize(stream) as DataSave;

            stream.Close();
            return dataSave;
        }
        else
        {
            Debug.LogError("File does not exist.");
            return new DataSave();
        }
    }

}