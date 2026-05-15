using System;
using System.Collections.Generic;
using Milehigh.Data;

namespace Tests
{
    class Program
    {
        static int failedTests = 0;

        static void Main(string[] args)
        {
            RunTest("Valid Data", TestValid);
            RunTest("Null Metadata", TestNullMetadata);
            RunTest("Invalid Void Saturation High", TestInvalidVoidSaturationHigh);
            RunTest("Invalid Void Saturation Low", TestInvalidVoidSaturationLow);
            RunTest("Empty Characters", TestEmptyCharacters);
            RunTest("Null Characters", TestNullCharacters);
            RunTest("Empty Scenarios", TestEmptyScenarios);
            RunTest("Null Scenarios", TestNullScenarios);

            if (failedTests == 0)
            {
                Console.WriteLine("\nALL TESTS PASSED");
            }
            else
            {
                Console.WriteLine($"\n{failedTests} TESTS FAILED");
                Environment.Exit(1);
            }
        }

        static void RunTest(string name, Func<bool> test)
        {
            Console.Write($"Running {name}... ");
            try
            {
                if (test())
                {
                    Console.WriteLine("PASSED");
                }
                else
                {
                    Console.WriteLine("FAILED");
                    failedTests++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                failedTests++;
            }
        }

        static bool TestValid()
        {
            var data = CreateValidData();
            return data.IsValid();
        }

        static bool TestNullMetadata()
        {
            var data = CreateValidData();
            data.metadata = null;
            return !data.IsValid();
        }

        static bool TestInvalidVoidSaturationHigh()
        {
            var data = CreateValidData();
            data.metadata.voidSaturationLevel = 1.1f;
            return !data.IsValid();
        }

        static bool TestInvalidVoidSaturationLow()
        {
            var data = CreateValidData();
            data.metadata.voidSaturationLevel = -0.1f;
            return !data.IsValid();
        }

        static bool TestEmptyCharacters()
        {
            var data = CreateValidData();
            data.characters = new List<CharacterProfile>();
            return !data.IsValid();
        }

        static bool TestNullCharacters()
        {
            var data = CreateValidData();
            data.characters = null;
            return !data.IsValid();
        }

        static bool TestEmptyScenarios()
        {
            var data = CreateValidData();
            data.scenarios = new List<SceneScenario>();
            return !data.IsValid();
        }

        static bool TestNullScenarios()
        {
            var data = CreateValidData();
            data.scenarios = null;
            return !data.IsValid();
        }

        static HorizonGameData CreateValidData()
        {
            return new HorizonGameData
            {
                sceneId = "test_scene",
                metadata = new Metadata { voidSaturationLevel = 0.5f },
                characters = new List<CharacterProfile> { new CharacterProfile { name = "Hero" } },
                scenarios = new List<SceneScenario> { new SceneScenario { scenarioId = "intro" } }
            };
        }
    }
}
