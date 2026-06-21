using NUnit.Framework;
using MilehighWorld.Core;
using UnityEngine;

namespace MilehighWorld.Tests
{
    public class GlobalResonanceManagerTests
    {
        private GlobalResonanceManager _manager;
        private GameObject _go;

        [SetUp]
        public void SetUp()
        {
            _go = new GameObject();
            _manager = _go.AddComponent<GlobalResonanceManager>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_go);
        }

        [Theory]
        [TestCase(0.0f, 1.0f)]
        [TestCase(0.5f, 0.5f)]
        [TestCase(1.0f, 0.0f)]
        [TestCase(-0.5f, 1.0f)] // Clamped
        [TestCase(1.5f, 0.0f)]  // Clamped
        public void GetIntegrityMultiplier_ReturnsCorrectCalculation(float variance, float expected)
        {
            // Act
            _manager.UpdateResonance(variance);
            float actual = _manager.GetIntegrityMultiplier();

            // Assert
            Assert.AreEqual(expected, actual, 0.001f);
        }

        [Test]
        public void UpdateResonance_UpdatesVariance()
        {
            // Arrange
            float newState = 0.75f;

            // Act
            _manager.UpdateResonance(newState);

            // Assert
            Assert.AreEqual(newState, _manager.CurrentVoidVariance);
        }
    }
}
