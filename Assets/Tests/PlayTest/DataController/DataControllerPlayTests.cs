﻿using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

namespace Tests
{
    public class DataControllerPlayTests
    {

        [UnityTest]
        public IEnumerator TestDataControllerIsInstantiatedInFirstScene()
        {
            Debug.Log("This test checks that the data controller is instantiated on the very first scene loaded");
            SceneManager.LoadScene("logo", LoadSceneMode.Additive);
            yield return null;
            GameObject dataController = GameObject.FindWithTag("DataController");
            Assert.IsNotNull(dataController);
        }

        [UnityTest]
        public IEnumerator TestLoadDataFileOnAnExistingFile()
        {
            Debug.Log("This test checks that a test data save file is correctly loaded using the LoadGameData() of the data controller");
            string testDataPath = "Assets/Tests/TestsData/DataController/data.json";
            GameObject.Instantiate(TestController.instance.dataController as GameObject);
            yield return null;

            DataController.instance.filePath = testDataPath;
            DataController.instance.LoadGameData();
            while (!DataController.instance.GetIsReady())
            {
                yield return null;
            }
            Assert.AreEqual(DataController.instance.gameData.localizationLanguage, "ja");
        }

        [UnityTest]
        public IEnumerator TestLoadDataFileOnANonExistingFileCreatesGameDataObject()
        {
            Debug.Log("This test checks that the LoadGameData() is still creating a gameData object when the loading file has not been created");
            string dummyPath = "Assets/Tests/TestsData/DataController/dummy.json";
            GameObject.Instantiate(TestController.instance.dataController as GameObject);
            yield return null;

            DataController.instance.filePath = dummyPath;
            DataController.instance.LoadGameData();
            while (!DataController.instance.GetIsReady())
            {
                yield return null;
            }
            Assert.IsNotNull(DataController.instance.gameData);
            File.Delete(dummyPath);
        }

        [UnityTest]
        public IEnumerator TestLoadDataFileCreatesSaveFileIfNoneExists()
        {
            Debug.Log("This test checks that the LoadGameData() creates a save file if there is none existing");
            string dummyPath = "Assets/Tests/TestsData/DataController/dummy.json";
            GameObject.Instantiate(TestController.instance.dataController as GameObject);
            yield return null;

            DataController.instance.filePath = dummyPath;
            DataController.instance.LoadGameData();
            while (!DataController.instance.GetIsReady())
            {
                yield return null;
            }
            Assert.IsTrue(File.Exists(dummyPath));
            File.Delete(dummyPath);
        }

        [UnityTest]
        public IEnumerator LoadingAFileAndSavingItDoesNotModifyIt()
        {
            Debug.Log("This test checks that when loading a save file and saving it directly, the file is not modified (Load and Save are consistant)");
            string loadDataPath = "Assets/Tests/TestsData/DataController/data.json";
            string saveDataPath = "Assets/Tests/TestsData/DataController/data2.json";
            GameObject.Instantiate(TestController.instance.dataController as GameObject);
            yield return null;

            string loadFileContent = File.ReadAllText(loadDataPath);
            DataController.instance.filePath = loadDataPath;
            DataController.instance.LoadGameData();
            while (!DataController.instance.GetIsReady())
            {
                yield return null;
            }
            DataController.instance.filePath = saveDataPath;
            DataController.instance.SaveGameData();
            yield return null;
            string saveFileContent = File.ReadAllText(saveDataPath);
            Assert.AreEqual(saveFileContent, loadFileContent);
            File.Delete(saveDataPath);
        }

        [SetUp]
        public void loadTestScene()
        {
            if (SceneManager.GetActiveScene().name != "testScene")
            {
                SceneManager.LoadScene("testScene", LoadSceneMode.Single);
            }
        }

        [TearDown]
        public void unloadTestScene()
        {
            if (SceneManager.GetActiveScene().name != "testScene")
            {
                SceneManager.UnloadSceneAsync("testScene");
            }
        }

        [TearDown]
        public void destroyAllGameObjects()
        {
            foreach (GameObject go in Object.FindObjectsOfType<GameObject>())
            {
                Object.Destroy(go);
            }
        }
    }
}