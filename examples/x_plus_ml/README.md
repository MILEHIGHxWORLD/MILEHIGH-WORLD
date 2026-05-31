# Vitis AI 6.1 x_plus_ml Example Integration

This directory represents the expected location for custom C++ VART X applications and hardware IP configurations.

## Unity Bridge Integration
The `Assets/Scripts/Backend/VitisAIBridge.cs` in the Unity project acts as the high-level orchestrator for these low-level components.

### Memory Optimization Mapping
- **Format 0 (Non-Native)**: Simulated via standard virtual memory buffers.
- **Format 1 (Native Virtual)**: Used for tensor alignment in the `VitisAIConfig` models.
- **Format 2 (Zero-Copy)**: Targeted via direct pointer mapping in a real VART X environment.

## Deployment
1. Ensure the `dpu.xclbin` is located at the path specified in `vitis_config.json`.
2. Use the `-l 6` flag for complete DEBUG printouts during the convergence sequence.
