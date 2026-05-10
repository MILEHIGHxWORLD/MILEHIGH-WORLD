using Xunit;
using Milehigh.Core;
using UnityEngine;

namespace Milehigh.Tests
{
    public class CampaignManagerTests
    {
        [Fact]
        public void IncreaseVoidSaturation_ShouldIncreaseWithinRange()
        {
            // Arrange
            var campaignManager = new CampaignManager();
            campaignManager.currentVoidSaturationLevel = 0.2f;

            // Act
            campaignManager.IncreaseVoidSaturation(0.3f);

            // Assert
            Assert.Equal(0.5f, campaignManager.currentVoidSaturationLevel, precision: 3);
        }

        [Fact]
        public void IncreaseVoidSaturation_ShouldClampAtUpperBound()
        {
            // Arrange
            var campaignManager = new CampaignManager();
            campaignManager.currentVoidSaturationLevel = 0.9f;

            // Act
            campaignManager.IncreaseVoidSaturation(0.2f);

            // Assert
            Assert.Equal(1.0f, campaignManager.currentVoidSaturationLevel);
        }

        [Fact]
        public void IncreaseVoidSaturation_ShouldClampAtLowerBound_WhenNegativeAmountPassed()
        {
            // Arrange
            var campaignManager = new CampaignManager();
            campaignManager.currentVoidSaturationLevel = 0.1f;

            // Act
            campaignManager.IncreaseVoidSaturation(-0.2f);

            // Assert
            Assert.Equal(0.0f, campaignManager.currentVoidSaturationLevel);
        }

        [Fact]
        public void IncreaseVoidSaturation_MultipleIncreases_ShouldAccumulateCorrectly()
        {
            // Arrange
            var campaignManager = new CampaignManager();
            campaignManager.currentVoidSaturationLevel = 0.0f;

            // Act
            campaignManager.IncreaseVoidSaturation(0.1f);
            campaignManager.IncreaseVoidSaturation(0.1f);
            campaignManager.IncreaseVoidSaturation(0.1f);

            // Assert
            Assert.Equal(0.3f, campaignManager.currentVoidSaturationLevel, precision: 3);
        }
    }
}
