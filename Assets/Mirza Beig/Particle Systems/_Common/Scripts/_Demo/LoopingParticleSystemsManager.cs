
// =================================	
// Namespaces.
// =================================

using UnityEngine;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace ParticleSystems
    {

        namespace Demos
        {

            // =================================	
            // Classes.
            // =================================

            public class LoopingParticleSystemsManager : ParticleManager
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                // =================================	
                // Functions.
                // =================================

                // ...

                protected override void Awake()
                {
                    base.Awake();
                }

                // ...

                protected override void Start()
                {
                    base.Start();

                    // ...

                    particlePrefabs[currentParticlePrefabIndex][0].gameObject.SetActive(true);
                }

                // ...

                public override void Next()
                {
                    particlePrefabs[currentParticlePrefabIndex][0].gameObject.SetActive(false);

                    base.Next();
                    particlePrefabs[currentParticlePrefabIndex][0].gameObject.SetActive(true);
                }
                public override void Previous()
                {
                    particlePrefabs[currentParticlePrefabIndex][0].gameObject.SetActive(false);

                    base.Previous();
                    particlePrefabs[currentParticlePrefabIndex][0].gameObject.SetActive(true);
                }

                // ...

                protected override void Update()
                {
                    base.Update();
                }

                // ...

                public override int GetParticleCount()
                {
                    // Return particle count from active prefab.

                    int particleCount = 0;

                    ParticleSystem[] currentPrefab = particlePrefabs[currentParticlePrefabIndex];

                    for (int i = 0; i < currentPrefab.Length; i++)
                    {
                        particleCount += currentPrefab[i].particleCount;
                    }

                    return particleCount;
                }

                // =================================	
                // End functions.
                // =================================

            }

            // =================================	
            // End namespace.
            // =================================

        }

    }

}

// =================================

// =================================	
// --END-- //
// =================================
