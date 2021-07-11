using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioSource gameAudio;

    public static GameManager Instance;
    public string playerName = "";
    public int highScore;
    public string highScorePlayer = "";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadDetailsFromFile();
    }

    public void SaveName()
    {
        playerName = GameObject.Find("Name Input").GetComponent<TMP_InputField>().text;
    }

    public void LoadName()
    {
        GameObject.Find("Name Input").GetComponent<TMP_InputField>().text = playerName;
    }

    public void LoadHighScore()
    {
        GameObject.Find("Current High Score").GetComponent<TextMeshProUGUI>().text = highScorePlayer + " currently has the high score: " + highScore;
    }

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int highScore;
        public string highScorePlayer;
    }

    public void SaveDetailsToFile()
    {
        SaveData data = new SaveData();
        data.playerName = playerName;
        data.highScore = highScore;
        data.highScorePlayer = highScorePlayer;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadDetailsFromFile()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playerName = data.playerName;
            highScore = data.highScore;
            highScorePlayer = data.highScorePlayer;
        }
    }
}
