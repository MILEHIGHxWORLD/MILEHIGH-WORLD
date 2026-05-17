using UnityEngine;

namespace Milehigh.World.Core
{
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            else { Destroy(gameObject); }
        }

        public float CalculateVanguardDamage(CharacterData attacker, float baseDamage)
        {
            float integrityMult = GlobalResonanceManager.Instance.GetIntegrityMultiplier();
            // Formula: $D_{total} = (D_{base} \cdot M_{vanguard}) \cdot I_{multiplier}$
            return (baseDamage * attacker.vanguardMultiplier) * integrityMult;
        }

        private static readonly int GlitchIntensityId = Shader.PropertyToID("_GlitchIntensity");
        private static MaterialPropertyBlock? _glitchPropertyBlock;

        public void TriggerEnemyGlitch(GameObject target)
        {
            if (target.TryGetComponent<Renderer>(out Renderer ren))
            {
                // ⚡ Bolt: Use MaterialPropertyBlock to prevent material instantiation and preserve draw call batching.
                if (_glitchPropertyBlock == null) _glitchPropertyBlock = new MaterialPropertyBlock();

                ren.GetPropertyBlock(_glitchPropertyBlock);
                _glitchPropertyBlock.SetFloat(GlitchIntensityId, 1.0f);
                ren.SetPropertyBlock(_glitchPropertyBlock);
            }
        }
    }
}
