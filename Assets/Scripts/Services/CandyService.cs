using System.Collections;
using System.Collections.Generic;
using CandyCrush.Extensions;
using UnityEngine;

namespace CandyCrush.Services
{

    public class CandyService
    {
        public GameObject[] CandiesTypes { get; private set; }

        public CandyService()
        {
            CandiesTypes = LoadCandiesTypes();
        }


        public ItemComponent Create(GameObject candysGameObject, int x, int y)
        {
            var randomType = Random.Range(0, CandiesTypes.Length);

            var candComponent = MonoBehaviour.Instantiate(CandiesTypes[randomType], candysGameObject.transform);

            candComponent.transform.localPosition = new Vector3(x, y, 0);

            candComponent.transform.localRotation = Quaternion.identity;

            var gridItem = candComponent.GetComponent<ItemComponent>();

            gridItem.OnItemPositionChanged(x, y);

            return gridItem;
        }

        public void CreateCandysColider(GameObject parent)
        {
            // Cria o novo objeto a partir do prefab
            GameObject colider = new GameObject("Colider");

            Vector3 coliderScale = new Vector3(1000, colider.transform.localScale.y, 0);

            colider.transform.localScale = coliderScale;

            colider.transform.SetParent(parent.transform);

            colider.AddComponent<BoxCollider2D>();

            var prefabType = GetCandyPrefab();

            Vector3 targetPosition = prefabType.transform.position;

            float parentHeight = prefabType.transform.GetParentHeight();

            // Move o componente atual para baixo com base no tamanho do componente de destino
            colider.transform.position = new Vector3(targetPosition.x, parentHeight - 1 - targetPosition.y, targetPosition.z);

        }

        private GameObject GetCandyPrefab()
        {
            return CandiesTypes[0];
        }

        private GameObject[] LoadCandiesTypes()
        {
            return Resources.LoadAll<GameObject>("Prefabs");
        }

    }
}