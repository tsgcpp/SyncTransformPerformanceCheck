using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PerformanceTest
{
    public static string[] PerformanceCheckScenePathList
    {
        get
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            var scenePathList = new List<string>(capacity: sceneCount);

            for (int i = 0; i < sceneCount; ++i)
            {
                scenePathList.Add(SceneUtility.GetScenePathByBuildIndex(i));
            }

            return scenePathList
                .Where(path => path.StartsWith(PerformanceCheckSceneDirectory))
                .ToArray();
        }
    }

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        yield return SceneManager.LoadSceneAsync(EmptyScenePath);
        System.GC.Collect();
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        yield return SceneManager.LoadSceneAsync(EmptyScenePath);
        System.GC.Collect();
        yield return null;
    }

    [UnityTest, Performance]
    public IEnumerator ScenePerformanceCheck([ValueSource("PerformanceCheckScenePathList")] string scenePath)
    {
        yield return SceneManager.LoadSceneAsync(scenePath);
        yield return Measure
            .Frames()
            .WarmupCount(10)
            .MeasurementCount(50)
            .Run();
    }

    public const string PerformanceCheckSceneDirectory = "Assets/Scenes/PerformanceCheck/";
    public const string EmptyScenePath = "Assets/Scenes/Empty.unity";
}
