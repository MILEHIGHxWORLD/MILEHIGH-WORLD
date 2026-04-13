using System;
using System.Collections.Generic;
using Milehigh.Characters;

namespace AnastasiaTests
{
    public class AnastasiaAIControllerMock : AnastasiaAIController
    {
        public bool SnapMemoryFragmentsCalled { get; private set; }
        public bool TriggerEnvironmentCrashCalled { get; private set; }
        public List<string> ReverieCommands { get; } = new List<string>();

        protected override void SnapMemoryFragments()
        {
            SnapMemoryFragmentsCalled = true;
        }

        protected override void TriggerEnvironmentCrash()
        {
            TriggerEnvironmentCrashCalled = true;
        }

        protected override void CommandReverie(string command)
        {
            ReverieCommands.Add(command);
        }
    }

    public class TestRunner
    {
        public static int Main(string[] args)
        {
            int failedTests = 0;

            failedTests += RunTest("ProcessDreamscape_WhenStateIsBrittle_SnapsMemoryFragments", () => {
                var controller = CreateController();
                controller.ProcessDreamscape("brittle", 0);
                return controller.SnapMemoryFragmentsCalled;
            });

            failedTests += RunTest("ProcessDreamscape_WhenStateIsGray_SnapsMemoryFragments", () => {
                var controller = CreateController();
                controller.ProcessDreamscape("gray", 0);
                return controller.SnapMemoryFragmentsCalled;
            });

            failedTests += RunTest("ProcessDreamscape_WhenStateIsStable_DoesNotSnapMemoryFragments", () => {
                var controller = CreateController();
                controller.ProcessDreamscape("stable", 10);
                return !controller.SnapMemoryFragmentsCalled;
            });

            failedTests += RunTest("ProcessDreamscape_WhenParityFailsExceedsThreshold_TriggersCrash", () => {
                var controller = CreateController();
                controller.ProcessDreamscape("brittle", 6);
                return controller.TriggerEnvironmentCrashCalled;
            });

            failedTests += RunTest("ProcessDreamscape_WhenParityFailsDoesNotExceedThreshold_DoesNotTriggerCrash", () => {
                var controller = CreateController();
                controller.ProcessDreamscape("brittle", 5);
                return !controller.TriggerEnvironmentCrashCalled;
            });

            failedTests += RunTest("ProcessDreamscape_AlwaysCommandsReverieToToggleForm", () => {
                var controller = CreateController();
                controller.ProcessDreamscape("stable", 0);
                controller.ProcessDreamscape("brittle", 10);
                return controller.ReverieCommands.Count == 2 &&
                       controller.ReverieCommands[0] == "toggle_form" &&
                       controller.ReverieCommands[1] == "toggle_form";
            });

            if (failedTests == 0)
            {
                Console.WriteLine("\nAll tests passed!");
                return 0;
            }
            else
            {
                Console.WriteLine($"\n{failedTests} tests failed!");
                return 1;
            }
        }

        private static AnastasiaAIControllerMock CreateController()
        {
            var controller = new AnastasiaAIControllerMock();
            controller.parityFailsThreshold = 5f;
            return controller;
        }

        private static int RunTest(string name, Func<bool> test)
        {
            try
            {
                if (test())
                {
                    Console.WriteLine($"[PASS] {name}");
                    return 0;
                }
                else
                {
                    Console.WriteLine($"[FAIL] {name}");
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {name}: {ex.Message}");
                return 1;
            }
        }
    }
}
