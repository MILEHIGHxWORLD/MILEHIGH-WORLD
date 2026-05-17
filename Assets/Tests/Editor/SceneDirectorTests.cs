using NUnit.Framework;
using UnityEngine;
using Milehigh.Core;
using Milehigh.Data;
using Milehigh.Characters;
using System.Collections.Generic;
using System.Reflection;

namespace Milehigh.Tests
{
    public class SceneDirectorTests
    {
        private SceneDirector _sceneDirector;
        private GameObject _directorGo;

        [SetUp]
        public void SetUp()
        {
            _directorGo = new GameObject("SceneDirector");
            _sceneDirector = _directorGo.AddComponent<SceneDirector>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_directorGo);
        }

        [Test]
        public void SetupScene_ClearsCaches()
        {
            // Use reflection to access private cache dictionaries
            var objectCacheField = typeof(SceneDirector).GetField("_objectCache", BindingFlags.NonPublic | BindingFlags.Instance);
            var controllerCacheField = typeof(SceneDirector).GetField("_controllerCache", BindingFlags.NonPublic | BindingFlags.Instance);

            var objectCache = (Dictionary<string, GameObject?>)objectCacheField.GetValue(_sceneDirector);
            var controllerCache = (Dictionary<int, CharacterControllerBase?>)controllerCacheField.GetValue(_sceneDirector);

            // Populate caches manually
            objectCache.Add("TestObject", new GameObject());
            controllerCache.Add(1, null);

            Assert.AreEqual(1, objectCache.Count);
            Assert.AreEqual(1, controllerCache.Count);

            // Execute SetupScene with a dummy scenario
            var scenario = new SceneScenario { scenarioId = "test_scenario" };
            _sceneDirector.SetupScene(scenario);

            // Verify caches are cleared
            Assert.AreEqual(0, objectCache.Count, "Object cache should be cleared at start of SetupScene");
            Assert.AreEqual(0, controllerCache.Count, "Controller cache should be cleared at start of SetupScene");
        }
    }
}
