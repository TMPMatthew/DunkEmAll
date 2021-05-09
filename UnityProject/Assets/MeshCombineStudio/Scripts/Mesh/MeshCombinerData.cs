using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeshCombineStudio
{
    public class MeshCombinerData : MonoBehaviour
    {
        [HideInInspector] public List<GameObject> combinedGameObjects = new List<GameObject>();
        [HideInInspector] public List<CachedGameObject> foundObjects = new List<CachedGameObject>();
        [HideInInspector] public List<CachedLodGameObject> foundLodObjects = new List<CachedLodGameObject>();
        [HideInInspector] public List<LODGroup> foundLodGroups = new List<LODGroup>();
        [HideInInspector] public List<Collider> foundColliders = new List<Collider>();

        void OnValidate()
        {
            hideFlags = HideFlags.HideInInspector;
        }

        public void ClearFound()
        {
            foundObjects.Clear();
            foundLodObjects.Clear();
            foundLodGroups.Clear();
            foundColliders.Clear();
        }
    }
}
