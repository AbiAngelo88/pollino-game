using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerSaver
{
    private static string fileName = "/player.data";

    public delegate void PlayerSaved(PlayerData data);
    public static event PlayerSaved OnPlayerSaved;

    public delegate void LoadedPlayer(PlayerData data);
    public static event LoadedPlayer OnLoadedPlayer;

    public static void Save(PlayerData data)
    {
        Debug.Log("SAVE PLAYER INIT");
        BinaryFormatter bf = new BinaryFormatter();
        Debug.Log("PATH ---------------------> " + Application.persistentDataPath.ToString());
        string path = Application.persistentDataPath + "/" + fileName;
        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, data); // scrive in formato binario
        fileStream.Close();
        Debug.Log("SAVE PLAYER FINISH");

        //PlayerData data = new PlayerData(score);
        //StreamWriter writer = new StreamWriter(fileStream);
        //writer.Write(data);
        //writer.Close();
        OnPlayerSaved?.Invoke(data);
    }

    public static PlayerData Load()
    {
        
        string path = Application.persistentDataPath + "/" + fileName;
        Debug.Log("PATH ---------------------> " + Application.persistentDataPath.ToString());
        PlayerData data;

        if(File.Exists(path))
        {
            Debug.Log("FILE EXISTS");
            BinaryFormatter bf = new BinaryFormatter();

            FileStream fileStream = new FileStream(path, FileMode.Open);

            if (fileStream != null && fileStream.Length > 0) {
                data = bf.Deserialize(fileStream) as PlayerData;
                //StreamReader reader = new StreamReader(fileStream);
                //data = reader.ReadToEnd() as PlayerData;
            } else
                data = new PlayerData();

            fileStream.Close();

        } else
        {
            Debug.Log("Non c'è alcun file!");
            data =  new PlayerData();
        }

        OnLoadedPlayer?.Invoke(data);
        return data;
    }
}
