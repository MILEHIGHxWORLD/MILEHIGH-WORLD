using NUnit.Framework;
using UnityEngine;
using Milehigh.Core;
using System.Reflection;

namespace Milehigh.Tests
{
    [TestFixture]
    public class CampaignManagerTests
    {
        private GameObject _testObject = null!;
        private CampaignManager _campaignManager = null!;

        [SetUp]
        public void SetUp()
        {
            // Clear singleton instance via reflection to ensure test isolation
            ResetCampaignManagerInstance();

            _testObject = new GameObject("TestCampaignManager");
            _campaignManager = _testObject.AddComponent<CampaignManager>();
            _campaignManager.currentVoidSaturationLevel = 0.5f;
        }

        [TearDown]
        public void TearDown()
        {
            if (_testObject != null)
            {
                UnityEngine.Object.DestroyImmediate(_testObject);
            }
            ResetCampaignManagerInstance();
        }

        private void ResetCampaignManagerInstance()
        {
            var field = typeof(CampaignManager).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic);
            if (field != null)
            {
                field.SetValue(null, null);
            }
        }

        [Test]
        public void IncreaseVoidSaturation_IncreasesLevel()
        {
            float initialValue = _campaignManager.currentVoidSaturationLevel;
            float increaseAmount = 0.1f;

            _campaignManager.IncreaseVoidSaturation(increaseAmount);

            Assert.AreEqual(initialValue + increaseAmount, _campaignManager.currentVoidSaturationLevel, 0.001f);
        }

        [Test]
        public void IncreaseVoidSaturation_DecreasesLevel_WhenNegativeAmount()
        {
            float initialValue = _campaignManager.currentVoidSaturationLevel;
            float decreaseAmount = -0.1f;

            _campaignManager.IncreaseVoidSaturation(decreaseAmount);

            Assert.AreEqual(initialValue + decreaseAmount, _campaignManager.currentVoidSaturationLevel, 0.001f);
        }

        [Test]
        public void IncreaseVoidSaturation_ClampsAtOne()
        {
            _campaignManager.currentVoidSaturationLevel = 0.9f;
            _campaignManager.IncreaseVoidSaturation(0.2f);

            Assert.AreEqual(1.0f, _campaignManager.currentVoidSaturationLevel, 0.001f);
        }

        [Test]
        public void IncreaseVoidSaturation_ClampsAtZero()
        {
            _campaignManager.currentVoidSaturationLevel = 0.1f;
            _campaignManager.IncreaseVoidSaturation(-0.2f);

            Assert.AreEqual(0.0f, _campaignManager.currentVoidSaturationLevel, 0.001f);
        }

        [Test]
        public void IncreaseVoidSaturation_HandlesLargePositiveAmount()
        {
            _campaignManager.currentVoidSaturationLevel = 0.0f;
            _campaignManager.IncreaseVoidSaturation(5.0f);

            Assert.AreEqual(1.0f, _campaignManager.currentVoidSaturationLevel, 0.001f);
        }

        [Test]
        public void IncreaseVoidSaturation_HandlesLargeNegativeAmount()
        {
            _campaignManager.currentVoidSaturationLevel = 1.0f;
            _campaignManager.IncreaseVoidSaturation(-5.0f);

            Assert.AreEqual(0.0f, _campaignManager.currentVoidSaturationLevel, 0.001f);
        }
    }
}
