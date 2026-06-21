using NUnit.Framework;
using UnityEngine;
using MilehighWorld.Core;
using MilehighWorld.Data;

namespace MilehighWorld.Tests
{
    public class CombatManagerTests
    {
        private CombatManager _combatManager;
        private GlobalResonanceManager _resonanceManager;
        private GameObject _combatGo;
        private GameObject _resonanceGo;

        [SetUp]
        public void SetUp()
        {
            _combatGo = new GameObject("CombatManager");
            _combatManager = _combatGo.AddComponent<CombatManager>();

            _resonanceGo = new GameObject("GlobalResonanceManager");
            _resonanceManager = _resonanceGo.AddComponent<GlobalResonanceManager>();
            GlobalResonanceManager.Instance = _resonanceManager;
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(_combatGo);
            UnityEngine.Object.DestroyImmediate(_resonanceGo);
            GlobalResonanceManager.Instance = null;
        }

        [Test]
        public void CalculateVanguardDamage_BaseDamageOnly()
        {
            // Arrange
            var attacker = new RuntimeCharacterData { TechAlignment = "Cybernetic", IsVoidCorrupted = false };
            var target = new RuntimeCharacterData { IsVoidCorrupted = false };
            _resonanceManager.UpdateResonance(0.0f); // Multiplier will be 1.0

            // Act
            float result = _combatManager.CalculateVanguardDamage(attacker, target, 100f);

            // Assert
            Assert.AreEqual(100f, result);
        }

        [Test]
        public void CalculateVanguardDamage_ArcaneResonanceBonus()
        {
            // Arrange
            var attacker = new RuntimeCharacterData { TechAlignment = "Arcane", IsVoidCorrupted = false };
            var target = new RuntimeCharacterData { IsVoidCorrupted = true };
            _resonanceManager.UpdateResonance(0.0f); // Multiplier will be 1.0

            // Act
            float result = _combatManager.CalculateVanguardDamage(attacker, target, 100f);

            // Assert
            Assert.AreEqual(150f, result);
        }

        [Test]
        public void CalculateVanguardDamage_HybridResonanceBonus()
        {
            // Arrange
            var attacker = new RuntimeCharacterData { TechAlignment = "Hybrid", IsVoidCorrupted = false };
            var target = new RuntimeCharacterData { IsVoidCorrupted = true };
            _resonanceManager.UpdateResonance(0.0f); // Multiplier will be 1.0

            // Act
            float result = _combatManager.CalculateVanguardDamage(attacker, target, 100f);

            // Assert
            Assert.AreEqual(150f, result);
        }

        [Test]
        public void CalculateVanguardDamage_GlobalResonanceImpact()
        {
            // Arrange
            var attacker = new RuntimeCharacterData { TechAlignment = "Cybernetic", IsVoidCorrupted = false };
            var target = new RuntimeCharacterData { IsVoidCorrupted = false };
            _resonanceManager.UpdateResonance(0.5f); // Multiplier will be 0.5

            // Act
            float result = _combatManager.CalculateVanguardDamage(attacker, target, 100f);

            // Assert
            Assert.AreEqual(50f, result);
        }

        [Test]
        public void CalculateVanguardDamage_CombinedEffects()
        {
            // Arrange
            var attacker = new RuntimeCharacterData { TechAlignment = "Arcane", IsVoidCorrupted = false };
            var target = new RuntimeCharacterData { IsVoidCorrupted = true };
            _resonanceManager.UpdateResonance(0.2f); // Multiplier will be 0.8

            // Act
            float result = _combatManager.CalculateVanguardDamage(attacker, target, 100f);

            // Assert
            // 100 * 1.5 (Resonance) * 0.8 (Integrity) = 120
            Assert.AreEqual(120f, result);
        }
    }
}
