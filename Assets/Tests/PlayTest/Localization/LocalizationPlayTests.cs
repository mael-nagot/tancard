﻿using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tests
{
    public class LocalizationPlayTests
    {

		// This test checks that the Localization Manager is instantiated from the very first scene.
        [UnityTest]
        public IEnumerator TestLocalizationManagerIsInstantiatedInFirstScene()
        {
            SceneManager.LoadScene("logo", LoadSceneMode.Single);
            yield return null;
            GameObject localizationManager = GameObject.FindWithTag("LocalizationManager");
			Assert.IsNotNull(localizationManager);
			destroyAllGameObjects();
        }

		// This test checks that the LoadLocalizedText actually loads the right localization file and that the GetLocalizedValue retrieve the expected localization value from a key.
        [UnityTest]
		[TestCase("localizedText_en.json","new_game_menu","New Game", ExpectedResult = null)]
		[TestCase("localizedText_fr.json","continue_game_menu","Continuer", ExpectedResult = null)]
        public IEnumerator TestLoadingLocalizationFileAndGettingValue(string file, string key, string value)
        {
			yield return null;
			GameObject.Instantiate(Resources.Load("Prefabs/Controllers/LocalizationManager") as GameObject);
			yield return new WaitForSeconds(1);
			LocalizationManager.instance.LoadLocalizedText(file);
			
			while(!LocalizationManager.instance.GetIsReady())
			{
				yield return null;
			}
			Assert.AreEqual(LocalizationManager.instance.GetLocalizedValue(key),value);
			destroyAllGameObjects();
        }

		// This test checks that a text object instantiated with the LocalizedText script and a key is automatically localized.
        [UnityTest]
		[TestCase("localizedText_en.json","new_game_menu","New Game", ExpectedResult = null)]
		[TestCase("localizedText_fr.json","continue_game_menu","Continuer", ExpectedResult = null)]
        public IEnumerator TestTextObjectBeingLocalized(string file, string key, string value)
        {
			yield return null;
			GameObject.Instantiate(Resources.Load("Prefabs/Controllers/LocalizationManager") as GameObject);
			yield return new WaitForSeconds(1);
			LocalizationManager.instance.LoadLocalizedText(file);
			
			while(!LocalizationManager.instance.GetIsReady())
			{
				yield return null;
			}
			GameObject textGameObject = new GameObject("TextObject");
			textGameObject.AddComponent<Text>();
			textGameObject.GetComponent<Text>().text = "Test";
			textGameObject.AddComponent<LocalizedText>();
			textGameObject.GetComponent<LocalizedText>().key=key;
			yield return null;
			Assert.AreEqual(textGameObject.GetComponent<Text>().text,value);
			destroyAllGameObjects();
        }

		public void destroyAllGameObjects()
		{
			foreach (GameObject go in Object.FindObjectsOfType<GameObject>()) {
            	 Object.Destroy(go);
         	}
		}

    }

}
