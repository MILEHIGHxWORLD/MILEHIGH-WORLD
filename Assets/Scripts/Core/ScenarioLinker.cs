using UnityEngine;

namespace Milehigh.Core
{
    public class ScenarioLinker : MonoBehaviour
    {
        public string linkedScenarioId;

        public void LinkScenario(string scenarioId)
        {
            linkedScenarioId = scenarioId;
            Debug.Log($"Scenario Linker: Linked to {scenarioId}");
        }
    }
}
