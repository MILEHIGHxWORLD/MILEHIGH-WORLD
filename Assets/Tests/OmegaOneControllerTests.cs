#if UNITY_INCLUDE_TESTS || !UNITY_5_3_OR_NEWER
using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Milehigh.Characters;

namespace Milehigh.Tests
{
    [TestFixture]
    public class OmegaOneControllerTests
    {
        private OmegaOneController _controller;
        private GameObject _shard;

        [SetUp]
        public void SetUp()
        {
            Debug.Logs.Clear();
            var go = new GameObject();
            _controller = go.AddComponent<OmegaOneController>();
            _shard = new GameObject { name = "TestShard" };
        }

        [Test]
        public void CheckStability_BelowThreshold_TriggersLogic()
        {
            // Act
            _controller.CheckStability(0.29f, _shard);

            // Assert
            Assert.IsTrue(Debug.Logs.Any(l => l.Contains("Running Gemini Logic")), "Gemini logic should run");
            Assert.IsTrue(Debug.Logs.Any(l => l.Contains("Reconciling corrupted files in TestShard")), "Reconciliation should run");
        }

        [Test]
        public void CheckStability_AtThreshold_DoesNotTriggerLogic()
        {
            // Act
            _controller.CheckStability(0.3f, _shard);

            // Assert
            Assert.IsFalse(Debug.Logs.Any(l => l.Contains("Running Gemini Logic")), "Gemini logic should NOT run at threshold");
        }

        [Test]
        public void CheckStability_AboveThreshold_DoesNotTriggerLogic()
        {
            // Act
            _controller.CheckStability(0.5f, _shard);

            // Assert
            Assert.IsFalse(Debug.Logs.Any(l => l.Contains("Running Gemini Logic")), "Gemini logic should NOT run above threshold");
        }

        [Test]
        public void CheckStability_NullShard_BelowThreshold_LogsWarning()
        {
            // Act
            _controller.CheckStability(0.1f, null);

            // Assert
            Assert.IsTrue(Debug.Logs.Any(l => l.Contains("Running Gemini Logic")), "Gemini logic should still run");
            Assert.IsTrue(Debug.Logs.Any(l => l.Contains("Reconciling corrupted files in null shard")), "Should log null shard handling");
        }
    }
}
#endif
