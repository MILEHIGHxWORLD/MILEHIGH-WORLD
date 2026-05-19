using Xunit;
using MilehighWorld.Data;
using System.Collections.Generic;

namespace MilehighWorld.Tests
{
    public class HorizonGameDataTests
    {
        [Theory]
        [InlineData(0.0f, true)]
        [InlineData(0.5f, true)]
        [InlineData(1.0f, true)]
        [InlineData(-0.1f, false)]
        [InlineData(1.1f, false)]
        public void Metadata_IsValid_ValidatesVoidSaturationLevel(float level, bool expected)
        {
            var metadata = new Metadata { environment = "Test", voidSaturationLevel = level };
            Assert.Equal(expected, metadata.IsValid());
        }

        [Fact]
        public void HorizonGameData_IsValid_FailsIfMetadataMissing()
        {
            var data = new HorizonGameData { sceneId = "Test", metadata = null };
            Assert.False(data.IsValid());
        }

        [Fact]
        public void HorizonGameData_IsValid_FailsIfMetadataInvalid()
        {
            var data = new HorizonGameData
            {
                sceneId = "Test",
                metadata = new Metadata { environment = "Test", voidSaturationLevel = 2.0f }
            };
            Assert.False(data.IsValid());
        }

        [Fact]
        public void HorizonGameData_IsValid_FailsIfCharactersMissing()
        {
            var data = CreateValidBaseData();
            data.characters = null;
            Assert.False(data.IsValid());
        }

        [Fact]
        public void HorizonGameData_IsValid_FailsIfCharactersEmpty()
        {
            var data = CreateValidBaseData();
            data.characters = new List<CharacterProfile>();
            Assert.False(data.IsValid());
        }

        [Fact]
        public void HorizonGameData_IsValid_FailsIfScenariosMissing()
        {
            var data = CreateValidBaseData();
            data.scenarios = null;
            Assert.False(data.IsValid());
        }

        [Fact]
        public void HorizonGameData_IsValid_SucceedsForValidData()
        {
            var data = CreateValidBaseData();
            Assert.True(data.IsValid());
        }

        private HorizonGameData CreateValidBaseData()
        {
            return new HorizonGameData
            {
                sceneId = "TestScene",
                metadata = new Metadata { environment = "TestEnv", voidSaturationLevel = 0.5f },
                characters = new List<CharacterProfile>
                {
                    new CharacterProfile { name = "Test", role = "Tester" }
                },
                scenarios = new List<SceneScenario>
                {
                    new SceneScenario { scenarioId = "TestScenario" }
                }
            };
        }
    }
}
