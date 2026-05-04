using System.Collections.Generic;
using NUnit.Framework;
using Milehigh.Data;
using UnityEngine;
using UnityEngine.TestTools;

namespace Milehigh.Tests
{
    public class HorizonGameDataTests
    {
        private HorizonGameData CreateValidData()
        {
            return new HorizonGameData
            {
                sceneId = "test_scene",
                metadata = new Metadata
                {
                    lighting = LightingState.Day,
                    environment = "Sunny Forest",
                    systemParity = 1,
                    voidSaturationLevel = 0.5f
                },
                characters = new List<CharacterProfile>
                {
                    new CharacterProfile { name = "Hero", role = "Protagonist" }
                },
                scenarios = new List<SceneScenario>
                {
                    new SceneScenario { scenarioId = "intro", description = "The beginning" }
                }
            };
        }

        [Test]
        public void IsValid_WithValidData_ReturnsTrue()
        {
            var data = CreateValidData();
            Assert.IsTrue(data.IsValid());
        }

        [Test]
        public void IsValid_WithMissingMetadata_ReturnsFalse()
        {
            var data = CreateValidData();
            data.metadata = null;
            Assert.IsFalse(data.IsValid());
        }

        [Test]
        public void IsValid_WithInvalidVoidSaturation_ReturnsFalse()
        {
            var data = CreateValidData();
            data.metadata.voidSaturationLevel = 1.5f;
            Assert.IsFalse(data.IsValid(), "Should fail for high saturation");

            data.metadata.voidSaturationLevel = -0.1f;
            Assert.IsFalse(data.IsValid(), "Should fail for negative saturation");
        }

        [Test]
        public void IsValid_WithEnvironmentStringTooLong_ReturnsFalse()
        {
            var data = CreateValidData();
            data.metadata.environment = new string('A', 129);
            Assert.IsFalse(data.IsValid());
        }

        [Test]
        public void IsValid_WithInvalidCharacterProfiles_ReturnsFalse()
        {
            var data = CreateValidData();
            data.characters[0].name = ""; // Invalid name
            Assert.IsFalse(data.IsValid(), "Should fail for empty character name");

            data = CreateValidData();
            data.characters[0].name = new string('A', 65); // Too long
            Assert.IsFalse(data.IsValid(), "Should fail for too long character name");

            data = CreateValidData();
            data.characters = new List<CharacterProfile>();
            for(int i = 0; i < 51; i++) data.characters.Add(new CharacterProfile { name = "C" + i });
            Assert.IsFalse(data.IsValid(), "Should fail for too many character profiles");
        }

        [Test]
        public void IsValid_WithInvalidScenarios_ReturnsFalse()
        {
            var data = CreateValidData();
            data.scenarios[0].scenarioId = "";
            Assert.IsFalse(data.IsValid(), "Should fail for empty scenario ID");

            data = CreateValidData();
            data.scenarios = new List<SceneScenario>();
            for(int i = 0; i < 101; i++) data.scenarios.Add(new SceneScenario { scenarioId = "S" + i });
            Assert.IsFalse(data.IsValid(), "Should fail for too many scenarios");
        }

        [Test]
        public void IsValid_WithEmptyLists_ReturnsFalse()
        {
            var data = CreateValidData();
            data.characters = new List<CharacterProfile>();
            Assert.IsFalse(data.IsValid(), "Should fail for empty character list");

            data = CreateValidData();
            data.scenarios = new List<SceneScenario>();
            Assert.IsFalse(data.IsValid(), "Should fail for empty scenario list");
        }
    }
}
