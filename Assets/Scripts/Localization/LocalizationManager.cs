﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class LocalizationManager : MonoBehaviour
{
    // Code taken from this tutorial https://unity3d.com/fr/learn/tutorials/topics/scripting/localized-text-editor-script?playlist=17117
    public static LocalizationManager instance;

    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private string missingTextString = "Localized text not found";

    /*
    Use Singleton pattern for the localization manager object to have it never destroyed and only once instance of it.
    It needs to be present in all the scenes.
     */
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Load a localization text file and create a dictionary used by the UIManager instance to get localized strings
    /// Exemple: LocalizationManager.instance.LoadLocalizedText("localizedText_en.json");
    /// </summary>
    /// <param name="string fileName">The name of the file to localize (located in the StreamingAssets folder</param>
    public void LoadLocalizedText(string fileName)
    {
        localizedText = new Dictionary<string, string>();
        string filePath;
        string dataAsJson;

        /* 
        Cannot just read the files in the streaming path on Android since it is stored in an apk.
        Need a www reader for that specific case.
        */
        if (Application.platform == RuntimePlatform.Android)
        {
            filePath = Path.Combine("jar:file://" + Application.dataPath + "!assets/", fileName);
            // UnityWebRequest reader = new UnityWebRequest(filePath);
            WWW reader = new WWW(filePath);
            while (!reader.isDone) { }

            dataAsJson = reader.text;
        }
        else
        {
            filePath = Path.Combine(Application.streamingAssetsPath, fileName);
            dataAsJson = File.ReadAllText(filePath);
        }

        LocalizationData loadedData = JsonConvert.DeserializeObject<LocalizationData>(dataAsJson);

        for (int i = 0; i < loadedData.items.Length; i++)
        {
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
        }

        isReady = true;
    }

    /// <summary>
    /// Return a localized string from a key
    /// </summary>
    /// <param name="string key">The localization key that is used in the localization files</param>
    public string GetLocalizedValue(string key)
    {
        string result = missingTextString;
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }

        return result;

    }

    /// <summary>
    /// Return trye when the localization is loaded and ready to be used
    /// This is used to avoid calling localization when it is not fully loaded
    /// </summary>
    public bool GetIsReady()
    {
        return isReady;
    }

}