using Xunit;
using Milehigh.Characters;
using UnityEngine;

namespace Milehigh.Tests
{
    [Collection("Sequential")]
    public class AeronAIControllerTests
    {
        [Fact]
        public void AerialCombat_WhenFlappingArmsIsTrue_ShouldMaintainAltitude()
        {
            // Arrange
            var controller = new AeronAIController();
            Debug.Logs.Clear();

            // Act
            controller.AerialCombat(true, false, 5f);

            // Assert
            Assert.Contains("Aeron: Maintaining altitude.", Debug.Logs);
        }

        [Fact]
        public void AerialCombat_WhenThrustersActiveIsTrue_ShouldMaintainAltitude()
        {
            // Arrange
            var controller = new AeronAIController();
            Debug.Logs.Clear();

            // Act
            controller.AerialCombat(false, true, 5f);

            // Assert
            Assert.Contains("Aeron: Maintaining altitude.", Debug.Logs);
        }

        [Fact]
        public void AerialCombat_WhenBothAreTrue_ShouldMaintainAltitude()
        {
            // Arrange
            var controller = new AeronAIController();
            Debug.Logs.Clear();

            // Act
            controller.AerialCombat(true, true, 5f);

            // Assert
            Assert.Contains("Aeron: Maintaining altitude.", Debug.Logs);
        }

        [Fact]
        public void AerialCombat_WhenNoneAreTrueAndDistanceGreaterThanAura_ShouldApplyDefenseDebuff()
        {
            // Arrange
            var controller = new AeronAIController();
            controller.auraRadius = 10f;
            Debug.Logs.Clear();

            // Act
            controller.AerialCombat(false, false, 15f);

            // Assert
            Assert.Contains("Aeron: Applying defense debuff due to distance from Solar Barrier.", Debug.Logs);
        }

        [Fact]
        public void AerialCombat_WhenNoneAreTrueAndDistanceLessThanAura_ShouldDoNothing()
        {
            // Arrange
            var controller = new AeronAIController();
            controller.auraRadius = 10f;
            Debug.Logs.Clear();

            // Act
            controller.AerialCombat(false, false, 5f);

            // Assert
            Assert.Empty(Debug.Logs);
        }

        [Fact]
        public void AerialCombat_WhenNoneAreTrueAndDistanceEqualsAura_ShouldDoNothing()
        {
            // Arrange
            var controller = new AeronAIController();
            controller.auraRadius = 10f;
            Debug.Logs.Clear();

            // Act
            controller.AerialCombat(false, false, 10f);

            // Assert
            Assert.Empty(Debug.Logs);
        }
    }
}
