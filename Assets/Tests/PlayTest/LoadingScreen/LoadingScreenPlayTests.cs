﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class LoadingScreenPlayTests
    {
       [UnityTest]
        public IEnumerator TestSceneIsLoadedWhileLoadingScreenIsDone()
        {
			yield return loadLocalization();
			Debug.Log("This test checks that when loading a scene using the loading screen function, the scene is active at the end");
			GameObject.Instantiate(Resources.Load("Prefabs/Controllers/LoadingScreenController") as GameObject);
			yield return null;
            yield return LoadingScreenController.instance.loadScene("logo");
            Assert.AreEqual(SceneManager.GetActiveScene().name,"logo");
        }


		[TearDown]
		public void destroyAllGameObjects()
		{
			foreach (GameObject go in Object.FindObjectsOfType<GameObject>()) {
            	 Object.Destroy(go);
         	}
		}

		public IEnumerator loadLocalization()
		{
			GameObject.Instantiate(Resources.Load("Prefabs/Controllers/LocalizationManager") as GameObject);
			yield return null;
			LocalizationManager.instance.LoadLocalizedText("localizedText_en.json");
			
			while(!LocalizationManager.instance.GetIsReady())
			{
				yield return null;
			}
		}

    }
}