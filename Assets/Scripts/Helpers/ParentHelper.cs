using System;
using UnityEngine;

namespace CandyCrush.Helpers
{
    public static class ParentHelper
    {
        public static float GetParentHeight(Transform parent)
        {
            float totalHeight = 0f;

            foreach (Transform child in parent)
            {
                // Verifica se o componente filho possui um Collider anexado
                Collider childCollider = child.GetComponent<Collider>();
                if (childCollider != null)
                {
                    // Obtém a altura do Collider do componente filho
                    Vector3 colliderSize = childCollider.bounds.size;
                    float childHeight = colliderSize.y;

                    // Soma a altura do componente filho à altura total
                    totalHeight += childHeight;
                }

                // Chama recursivamente para os filhos do componente filho
                totalHeight += GetParentHeight(child);
            }

            return totalHeight;
        }
    }
}

