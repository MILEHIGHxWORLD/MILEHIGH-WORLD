using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using Milehigh.Core;
using Milehigh.Data;

namespace Milehigh.Tests
{
    public class SceneDirectorTests
    {
        private GameObject? _directorGo;
        private SceneDirector? _director;

        [SetUp]
        public void SetUp()
        {
            _directorGo = new GameObject("SceneDirector");
            _director = _directorGo.AddComponent<SceneDirector>();
        }

        [TearDown]
        public void TearDown()
        {
            if (_directorGo != null)
            {
                Object.DestroyImmediate(_directorGo);
            }
        }

        [Test]
        public void SetupScene_ClearsCaches()
        {
            Assert.IsNotNull(_director, "SceneDirector should not be null");
            if (_director == null) return;

            // Use reflection to access private caches and pre-fill them
            var objectCacheField = typeof(SceneDirector).GetField("_objectCache", BindingFlags.NonPublic | BindingFlags.Instance);
            var controllerCacheField = typeof(SceneDirector).GetField("_controllerCache", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(objectCacheField, "Could not find _objectCache field");
            Assert.IsNotNull(controllerCacheField, "Could not find _controllerCache field");
            if (objectCacheField == null || controllerCacheField == null) return;

            var objectCache = (Dictionary<string, GameObject?>?)objectCacheField.GetValue(_director);
            var controllerCache = (Dictionary<int, Milehigh.Characters.CharacterControllerBase?>?)controllerCacheField.GetValue(_director);

            Assert.IsNotNull(objectCache, "_objectCache should not be null");
            Assert.IsNotNull(controllerCache, "_controllerCache should not be null");
            if (objectCache == null || controllerCache == null) return;

            objectCache["TestObject"] = new GameObject("Test");
            controllerCache[1] = null;

            Assert.AreEqual(1, objectCache.Count, "Object cache should have 1 item before SetupScene");
            Assert.AreEqual(1, controllerCache.Count, "Controller cache should have 1 item before SetupScene");

            var scenario = new SceneScenario
            {
                scenarioId = "test_scenario",
                interactiveObjects = new List<ObjectInteraction>(),
                dialogue = new List<Dialogue>()
            };

            _director.SetupScene(scenario);

            Assert.AreEqual(0, objectCache.Count, "Object cache should be empty after SetupScene");
            Assert.AreEqual(0, controllerCache.Count, "Controller cache should be empty after SetupScene");
        }

        [Test]
        public void SetupScene_HandlesNullScenario()
        {
            Assert.IsNotNull(_director, "SceneDirector should not be null");
            if (_director == null) return;
            Assert.DoesNotThrow(() => _director.SetupScene(null!));
        }
    }
}
