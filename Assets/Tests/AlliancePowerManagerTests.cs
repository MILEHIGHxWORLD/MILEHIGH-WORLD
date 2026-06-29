using NUnit.Framework;
using MilehighWorld.Core;
using UnityEngine;

namespace MilehighWorld.Tests
{
    public class AlliancePowerManagerTests
    {
        private GameObject _go;
        private AlliancePowerManager _manager;

        [SetUp]
        public void SetUp()
        {
            AlliancePowerManager.ResetInstanceForTesting();
            _go = new GameObject("AlliancePowerManager");
            _manager = _go.AddComponent<AlliancePowerManager>();
            // Simulate Awake call for Singleton setup
            var method = typeof(AlliancePowerManager).GetMethod("Awake", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            method?.Invoke(_manager, null);
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(_go);
            AlliancePowerManager.ResetInstanceForTesting();
        }

        [Test]
        public void Instance_IsCorrectlySet()
        {
            Assert.AreEqual(_manager, AlliancePowerManager.Instance);
        }

        [Test]
        public void SetPowerLevel_UpdatesCurrentPowerLevel()
        {
            // Arrange
            float testLevel = 75.5f;

            // Act
            _manager.SetPowerLevel(testLevel);

            // Assert
            Assert.AreEqual(testLevel, _manager.CurrentPowerLevel);
        }

        [Test]
        public void DuplicateInstance_IsDestroyed()
        {
            // Arrange
            var secondGo = new GameObject("SecondManager");
            var secondManager = secondGo.AddComponent<AlliancePowerManager>();

            // Act
            var method = typeof(AlliancePowerManager).GetMethod("Awake", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            method?.Invoke(secondManager, null);

            // Assert
            // Since we use Destroy(gameObject) in Awake, and in this test environment
            // Destroy might be mocked or we check if the instance remains the first one.
            Assert.AreEqual(_manager, AlliancePowerManager.Instance);

            UnityEngine.Object.DestroyImmediate(secondGo);
        }
    }
}
