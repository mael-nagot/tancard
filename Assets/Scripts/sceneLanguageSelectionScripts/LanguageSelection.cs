﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LanguageSelection : MonoBehaviour
{
    public Button ButtonFrench;
    public Button ButtonEnglish;
    [SerializeField]
    private GameObject TextLanguage;

    void Awake()
    {
        // Make the flag buttons loading the localization when clicking on them
        ButtonFrench.onClick.AddListener(() => loadLocalization("fr"));
        ButtonEnglish.onClick.AddListener(() => loadLocalization("en"));
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);

        // Tween to make the flags and the text fade in when the scene is loaded
        Sequence flagFadeIn = DOTween.Sequence();
        flagFadeIn
                .Append(TextLanguage.GetComponent<Text>().DOFade(1, 1))
                .Join(ButtonFrench.gameObject.GetComponent<Image>().DOFade(1, 1))
                .Join(ButtonEnglish.gameObject.GetComponent<Image>().DOFade(1, 1));
        // Tween for looping the flag animation
        Sequence flagAnimation = DOTween.Sequence();
        flagAnimation
                .Append(ButtonFrench.gameObject.GetComponent<Image>().transform.DOScale(new Vector3(0.8f, 0.8f, 1), 1))
                .Join(ButtonEnglish.gameObject.GetComponent<Image>().transform.DOScale(new Vector3(0.8f, 0.8f, 1), 1))
                .SetLoops(-1, LoopType.Yoyo);
        StartCoroutine(fadeOutFlagsAndLoadScene());
    }

    /// <summary>
    /// This method make the flag fade out before loading the menu.
    /// </summary>  
    IEnumerator fadeOutFlagsAndLoadScene()
    {
        while (!LocalizationManager.instance.GetIsReady())
        {
            yield return null;
        }
        // Tween to fade out the flags and the text once the localization is fully loaded
        Sequence flagFadeOut = DOTween.Sequence();
        flagFadeOut
            .Append(TextLanguage.GetComponent<Text>().DOFade(0, 1))
            .Join(ButtonFrench.gameObject.GetComponent<Image>().DOFade(0, 1))
            .Join(ButtonEnglish.gameObject.GetComponent<Image>().DOFade(0, 1));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(LoadingScreenController.instance.loadScene("menu"));
    }

    /// <summary>
    /// Loading Localization and saving the language choice in the game data json file
    /// </summary>  
    private void loadLocalization(string language)
    {
        LocalizationManager.instance.LoadLocalizedText("localizedText_" + language + ".json");
        DataController.instance.gameData.localizationLanguage = language;
        DataController.instance.SaveGameData();
    }

}
