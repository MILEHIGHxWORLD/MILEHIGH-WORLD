using System;
using System.Reflection;
using NUnit.Framework;

namespace Milehigh.Tests
{
    class Program
    {
        static int Main(string[] args)
        {
            int failed = 0;
            int passed = 0;
            var testClass = new OmegaOneControllerTests();

            var methods = typeof(OmegaOneControllerTests).GetMethods();
            foreach (var method in methods)
            {
                if (Attribute.IsDefined(method, typeof(TestAttribute)))
                {
                    try
                    {
                        Console.WriteLine($"Running {method.Name}...");
                        testClass.SetUp();
                        method.Invoke(testClass, null);
                        Console.WriteLine($"PASSED: {method.Name}");
                        passed++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"FAILED: {method.Name}");
                        Console.WriteLine(ex.InnerException ?? ex);
                        failed++;
                    }
                }
            }

            Console.WriteLine($"\nTest Results: {passed} passed, {failed} failed.");
            return failed > 0 ? 1 : 0;
        }
    }
}
