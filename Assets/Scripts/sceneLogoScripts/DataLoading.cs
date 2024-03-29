﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public class DataLoading : MonoBehaviour
{

    [SerializeField]
    private float waitTime;
    private bool nextSceneIsLoading = false;
    [SerializeField]
    private GameObject logo;

    void Awake()
    {
        DOTween.Init();
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        // Tween to make the logo fade in and then fade out after a while
        Sequence logoDisplaySequence = DOTween.Sequence();
        logoDisplaySequence
                .Append(logo.gameObject.transform.DOScale(new Vector3(1, 1, 1), waitTime * 1.5f / 5))
                .Join(logo.gameObject.GetComponent<Image>().DOFade(1, waitTime * 2.5f / 5))
                .AppendInterval(1)
                .Append(logo.gameObject.GetComponent<Image>().DOFade(0, waitTime * 1.5f / 5))
                .OnComplete(() => LoadNextScene());
        // Load the localization if a save file exists and a language has already been set.
        if (!String.IsNullOrEmpty(DataController.instance.gameData.localizationLanguage))
        {
            LocalizationManager.instance.LoadLocalizedText("localizedText_" + DataController.instance.gameData.localizationLanguage + ".json");
        }
    }

    void Update()
    {

        // If clicking on the screen, skip the logo presentation
        if (Lean.Touch.LeanTouch.Fingers.Count > 0 && !nextSceneIsLoading)
        {
            LoadNextScene();
            nextSceneIsLoading = true;
        }
    }

    /// <summary>
    /// If a save file exists and a language has been set, wait for the localization to be loaded to load the menu
    /// otherwise, load the language selection scene.
    /// </summary>
    void LoadNextScene()
    {
        /* 
        If a save file exists and a language has been set, wait for the localization to be loaded to load the menu
        otherwise, load the language selection scene.
        */
        if (!String.IsNullOrEmpty(DataController.instance.gameData.localizationLanguage))
        {
            StartCoroutine("checkLocalizationReadyAndLoadMenuScene");
        }
        else
        {
            SceneManager.LoadScene("languageSelection", LoadSceneMode.Single);
        }
    }

    /// <summary>
    /// Wait for the localization to be loaded before loading the menu scene
    /// </summary>    
    private IEnumerator checkLocalizationReadyAndLoadMenuScene()
    {
        while (!LocalizationManager.instance.GetIsReady())
        {
            yield return null;
        }

        StartCoroutine(LoadingScreenController.instance.loadScene("menu"));
    }
}
