
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

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

            public class OneshotParticleSystemsManager : ParticleManager
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                public LayerMask mouseRaycastLayerMask;
                List<ParticleSystem[]> spawnedPrefabs;

                // Don't allow spawning if true.
                // Used for button clicks vs. empty-space clicks.

                public bool disableSpawn { get; set; }

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

                    disableSpawn = false;
                    spawnedPrefabs = new List<ParticleSystem[]>();
                }

                // Get rid of spawned systems when re-activated.

                void OnEnable()
                {
                    //clear();
                }

                // ...

                public void Clear()
                {
                    if (spawnedPrefabs != null)
                    {
                        for (int i = 0; i < spawnedPrefabs.Count; i++)
                        {
                            if (spawnedPrefabs[i][0])
                            {
                                Destroy(spawnedPrefabs[i][0].gameObject);
                            }
                        }

                        spawnedPrefabs.Clear();
                    }
                }

                // ...

                protected override void Update()
                {
                    base.Update();
                }

                // ...

                public void InstantiateParticlePrefab(Vector2 mousePosition, float maxDistance)
                {
                    if (spawnedPrefabs != null)
                    {
                        if (!disableSpawn)
                        {
                            Vector3 position = mousePosition;

                            position.z = maxDistance;
                            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(position);

                            Vector3 directionToWorldMouse = worldMousePosition - Camera.main.transform.position;

                            RaycastHit rayHit;

                            // Start the raycast a little bit ahead of the camera because the camera starts right where a cube's edge is
                            // and that causes the raycast to hit... spawning a prefab right at the camera position. It's fixed by moving the camera,
                            // or I can just add this forward to prevent it from happening at all.

                            Physics.Raycast(Camera.main.transform.position + Camera.main.transform.forward * 0.01f, directionToWorldMouse, out rayHit, maxDistance);

                            Vector3 spawnPosition;

                            if (rayHit.collider)
                            {
                                spawnPosition = rayHit.point;
                            }
                            else
                            {
                                spawnPosition = worldMousePosition;
                            }

                            ParticleSystem[] prefab = particlePrefabs[currentParticlePrefabIndex];
                            ParticleSystem newParticlePrefab = Instantiate(prefab[0], spawnPosition, prefab[0].transform.rotation);

                            newParticlePrefab.gameObject.SetActive(true);

                            // Parent to spawner.

                            newParticlePrefab.transform.parent = transform;

                            spawnedPrefabs.Add(newParticlePrefab.GetComponentsInChildren<ParticleSystem>());
                        }
                    }
                }

                // ...

                //public void instantiateParticlePrefabRandom()
                //{
                //    if (!disableSpawn)
                //    {
                //        instantiateParticlePrefab(new Vector3(
                //            Random.Range(0.0f, Screen.width), Random.Range(0.0f, Screen.height), 0.0f));
                //    }
                //}

                // ...

                public void Randomize()
                {
                    currentParticlePrefabIndex = Random.Range(0, particlePrefabs.Count);
                }

                // Get particle count from all spawned.

                public override int GetParticleCount()
                {
                    int particleCount = 0;

                    if (spawnedPrefabs != null)
                    {
                        for (int i = 0; i < spawnedPrefabs.Count; i++)
                        {
                            if (spawnedPrefabs[i][0])
                            {
                                for (int j = 0; j < spawnedPrefabs[i].Length; j++)
                                {
                                    particleCount += spawnedPrefabs[i][j].particleCount;
                                }
                            }
                            else
                            {
                                spawnedPrefabs.RemoveAt(i);
                            }
                        }
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
// --END-- //
// =================================
