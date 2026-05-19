using Xunit;
using MilehighWorld.Core;
using MilehighWorld.World.Core;
using UnityEngine;

namespace MilehighWorld.Tests
{
    public class CombatManagerTests
    {
        [Theory]
        [InlineData(100f, 1.0f, 1.0f, 125.0f)] // 100 * 1.0 * (1.0 * 1.25)
        [InlineData(100f, 2.0f, 0.5f, 125.0f)] // 100 * 2.0 * (0.5 * 1.25)
        [InlineData(50f, 1.5f, 0.8f, 75.0f)]   // 50 * 1.5 * (0.8 * 1.25) = 75 * 1.0 = 75
        [InlineData(100f, 1.0f, 0.0f, 0.0f)]   // 100 * 1.0 * (0.0 * 1.25)
        public void CalculateVanguardDamage_ReturnsCorrectCalculation(float baseDamage, float vanguardMultiplier, float resonanceFactor, float expected)
        {
            // Arrange
            var resonanceManager = new GlobalResonanceManager();
            resonanceManager.resonanceFactor = resonanceFactor;
            GlobalResonanceManager.Instance = resonanceManager;

            var combatManager = new CombatManager();

            var attacker = new MilehighWorld.World.Core.CharacterData
            {
                vanguardMultiplier = vanguardMultiplier
            };

            // Act
            float actual = combatManager.CalculateVanguardDamage(attacker, baseDamage);

            // Assert
            Assert.Equal(expected, actual, 3);

            // Cleanup
            GlobalResonanceManager.Instance = null;
        }

        [Fact]
        public void CalculateVanguardDamage_HandlesMissingResonanceManager()
        {
            // Arrange
            GlobalResonanceManager.Instance = null;
            var combatManager = new CombatManager();

            var attacker = new MilehighWorld.World.Core.CharacterData
            {
                vanguardMultiplier = 1.5f
            };
            float baseDamage = 100f;

            // Act
            float actual = combatManager.CalculateVanguardDamage(attacker, baseDamage);

            // Assert
            // Should use 1.0f as default multiplier if Instance is null
            Assert.Equal(150f, actual, 3);
        }
    }
}
