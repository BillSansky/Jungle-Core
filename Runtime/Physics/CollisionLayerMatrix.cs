using System;
using UnityEngine;

namespace Jungle.Physics
{
    /// <summary>
    /// Represents the Unity physics layer collision matrix.
    /// </summary>
    [Serializable]
    public class CollisionLayerMatrix
    {
        public const int LayerCount = 32;

        [SerializeField]
        private bool[] collisions = CreateDefaultCollisions();

        /// <summary>
        /// Creates a collision matrix based on the current Unity physics configuration.
        /// </summary>
        public static CollisionLayerMatrix FromPhysicsSettings()
        {
            var matrix = new CollisionLayerMatrix();
            for (int layerA = 0; layerA < LayerCount; layerA++)
            {
                for (int layerB = layerA; layerB < LayerCount; layerB++)
                {
                    bool collides = !Physics.GetIgnoreLayerCollision(layerA, layerB);
                    matrix.Set(layerA, layerB, collides);
                }
            }

            return matrix;
        }

        /// <summary>
        /// Returns whether the supplied layers are configured to collide.
        /// </summary>
        public bool Get(int layerA, int layerB)
        {
            ValidateLayerIndex(layerA);
            ValidateLayerIndex(layerB);
            EnsureArray();
            return collisions[GetIndex(layerA, layerB)];
        }

        /// <summary>
        /// Sets whether the supplied layers are configured to collide.
        /// </summary>
        public void Set(int layerA, int layerB, bool collides)
        {
            ValidateLayerIndex(layerA);
            ValidateLayerIndex(layerB);
            EnsureArray();

            int indexAB = GetIndex(layerA, layerB);
            int indexBA = GetIndex(layerB, layerA);

            collisions[indexAB] = collides;
            collisions[indexBA] = collides;
        }

        /// <summary>
        /// Applies the stored configuration to Unity physics.
        /// </summary>
        public void ApplyToPhysics()
        {
            EnsureArray();
            for (int layerA = 0; layerA < LayerCount; layerA++)
            {
                for (int layerB = layerA; layerB < LayerCount; layerB++)
                {
                    bool collides = Get(layerA, layerB);
                    Physics.IgnoreLayerCollision(layerA, layerB, !collides);
                }
            }
        }

        private static bool[] CreateDefaultCollisions()
        {
            var data = new bool[LayerCount * LayerCount];
            for (int layer = 0; layer < LayerCount; layer++)
            {
                data[GetIndex(layer, layer)] = true;
            }

            return data;
        }

        private void EnsureArray()
        {
            if (collisions == null || collisions.Length != LayerCount * LayerCount)
            {
                collisions = CreateDefaultCollisions();
            }
        }

        private static void ValidateLayerIndex(int layer)
        {
            if (layer < 0 || layer >= LayerCount)
            {
                throw new ArgumentOutOfRangeException(nameof(layer));
            }
        }

        private static int GetIndex(int layerA, int layerB)
        {
            return layerA * LayerCount + layerB;
        }
    }
}
