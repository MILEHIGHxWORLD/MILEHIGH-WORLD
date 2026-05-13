using Xunit;
using Milehigh.Core;

namespace Milehigh.Tests
{
    public class GlobalResonanceManagerTests
    {
        [Theory]
        [InlineData(1.0f, 1.25f)]
        [InlineData(0.0f, 0.0f)]
        [InlineData(2.0f, 2.5f)]
        [InlineData(-1.0f, -1.25f)]
        [InlineData(0.5f, 0.625f)]
        public void GetIntegrityMultiplier_ReturnsCorrectCalculation(float factor, float expected)
        {
            // Arrange
            var manager = new GlobalResonanceManager();
            manager.resonanceFactor = factor;

            // Act
            float actual = manager.GetIntegrityMultiplier();

            // Assert
            Assert.Equal(expected, actual, 3);
        }

        [Fact]
        public void UpdateResonance_UpdatesFactor()
        {
            // Arrange
            var manager = new GlobalResonanceManager();
            float newState = 0.75f;

            // Act
            manager.UpdateResonance(newState);

            // Assert
            Assert.Equal(newState, manager.resonanceFactor);
        }
    }
}
