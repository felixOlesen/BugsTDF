using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SaveLevelData(LevelManager levelManager, string levelName) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/"+levelName+".data";
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData data = new LevelData(levelManager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static LevelData LoadLevelData(string levelName) {
        string path = Application.persistentDataPath + "/"+levelName+".data";

        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return data;
        } else {
            Debug.Log("Save file not found " + path);
            return null;
        }
    }

    public static void SaveGameData(GameDataController gameDataController) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/game.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(gameDataController);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadGameData() {
        string path = Application.persistentDataPath + "/game.data";

        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;
        } else {
            Debug.Log("Save file not found " + path);
            return null;
        }
    }

}
