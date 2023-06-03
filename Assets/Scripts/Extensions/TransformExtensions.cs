using System.Collections;
using UnityEngine;

namespace CandyCrush.Extensions
{
    public static class TransformExtensions
    {

        public static IEnumerator Move(this Transform transform, Vector3 target, float duration)
        {
            var distance = target - transform.position;

            var distanceLength = distance.magnitude;

            distance.Normalize();

            float timer = 0;

            while(timer < duration)
            {
                float movAmount = (Time.deltaTime * distanceLength)/duration;
                transform.position += distance * movAmount;
                timer += Time.deltaTime;
                yield return null;
            }

            transform.position = target;
        }

        public static IEnumerator Scale(this Transform transform, Vector3 target, float duration)
        {
            var distance = target - transform.localScale;

            var distanceLength = distance.magnitude;

            distance.Normalize();

            float timer = 0;

            while (timer < duration)
            {
                float movAmount = (Time.deltaTime * distanceLength) / duration;
                transform.localScale += distance * movAmount;
                timer += Time.deltaTime;
                yield return null;
            }

            transform.localScale = target;
        }

        public static float GetParentHeight(this Transform parent)
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