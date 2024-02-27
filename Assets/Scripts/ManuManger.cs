using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManuManger : MonoBehaviour
{
    // Start is called before the first frame update
    public static ManuManger Instance;

    public string playerName;
    public int playerScore;

    public TextMeshProUGUI[] scoreTextList = new TextMeshProUGUI[5];
    public string[] nameList = new string[5];
    public int[] scoreList = new int[5];

    void Start()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        LoadPlayerDate();
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateTextList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene()
    {
        
        if(GameObject.Find("InputName").GetComponent<InputField>().text != "")
        {
            playerName = GameObject.Find("InputName").GetComponent<InputField>().text;
            SceneManager.LoadScene(1);
        }
        
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif

        SavePlayerDate();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "manu" )
        {
            GameObject.Find("StartButton").GetComponent<Button>().onClick.AddListener(LoadScene);
            GameObject.Find("QuitButton").GetComponent<Button>().onClick.AddListener(QuitGame);

            UpdateScore();
            StartCoroutine(WaitAndUpdate());
        }

       
        
    }

    [Serializable]
    class PlayerDate
    {
        public string[] playerName = new string[5];
        public int[] playerScore = new int[5];
    }

    public void SavePlayerDate()
    {
        PlayerDate data = new PlayerDate();
        data.playerName = nameList;
        data.playerScore = scoreList;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

        Debug.Log(Application.persistentDataPath + "/savefile.json");
    }

    public void LoadPlayerDate()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerDate data = JsonUtility.FromJson<PlayerDate>(json);

            nameList = data.playerName;
            scoreList = data.playerScore;
        }
    }

    public void UpdateScore()
    {
        int index = scoreList.Length ;


        while (scoreList[index - 1] < playerScore && index >= 0)
        {
            index--;
        }

        if (index == scoreList.Length - 1)
        {
            scoreList[index] = playerScore;
            nameList[index] = playerName;
        }
        else if (index < scoreList.Length - 1)
        {
            for (int i = scoreList.Length - 1; i > index ; i--)
            {
                scoreList[i] = scoreList[i - 1];
                nameList[i] = nameList[i - 1];
            }

            scoreList[index] = playerScore;
            nameList[index] = playerName;
        }
    }

    public void UpdateTextList()
    {
        

        for (int i = 0; i < nameList.Length; i++) 
        {
            scoreTextList[i] = GameObject.Find("Canvas").transform.Find("NameAndScore" + i).GetComponent<TextMeshProUGUI>();


            string name = nameList[i];
            int score = scoreList[i];

            int totalLength = 20;

            string formattedString = string.Format("{0,-10}{1,10}", name, score);
            scoreTextList[i].SetText(formattedString.PadRight(totalLength));
        }

    }

    IEnumerator WaitAndUpdate()
    {
        yield return new WaitForSeconds(1);
        UpdateTextList();
    }
}
