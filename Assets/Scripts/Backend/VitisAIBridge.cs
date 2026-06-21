using UnityEngine;
using MilehighWorld.Backend;
using Milehigh.World.CoreLogic;

namespace MilehighWorld.Backend
{
    /// <summary>
    /// Simulates the VART X pipeline execution.
    /// Translates hardware configuration parameters into system-level tension.
    /// </summary>
    public class VitisAIBridge : MonoBehaviour
    {
        [SerializeField] private VitisAIConfig config = new VitisAIConfig();

        /// <summary>
        /// Calculates the current system tension based on the quantization factor
        /// and normalization constants of the Vitis AI pipeline.
        /// </summary>
        public double CalculateSystemTension()
        {
            // Simulate VART X Milestone B: Memory Optimization Modes
            // We derive tension from the complexity of the hardware scaling (qt_fctr)
            // and the normalization offset relative to the BaseIterator (9).

            double qFactor = config.preprocess_config.qt_fctr / 64.0;
            double meanOffset = (config.preprocess_config.mean_r + config.preprocess_config.mean_g + config.preprocess_config.mean_b) / 300.0;

            // Baseline tension is aligned with FullSequenceAligned
            double baseTension = RealityConstants.FullSequenceAligned;

            // Add jitter/complexity from the ML pipeline config
            double tension = baseTension + (qFactor * 0.1) + (meanOffset * 0.05);

            Debug.Log($"[VitisAIBridge]: Calculated System Tension: {tension} (qFactor: {qFactor}, meanOffset: {meanOffset})");

            return tension;
        }
    }
}
