using System.Collections;
using System.Collections.Generic;
using CandyCrush.Extensions;
using UnityEngine;

namespace CandyCrush.Services
{

    public class GridService
    {
        private GameObject[] _candiesTypes;

        public GridService()
        {
            _candiesTypes = LoadCandiesTypes();
        }

        public ItemComponent[,] Generate(int xSize, int ySize, GameObject candysGameObject)
        {
            var _itens = new ItemComponent[xSize, ySize];

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    _itens[x, y] = CreateCandy(candysGameObject, x, y);
                }
            }

            CreateCandysColider(candysGameObject);

            return _itens;
        }


        private ItemComponent CreateCandy(GameObject candysGameObject, int x, int y)
        {
            var randomType = Random.Range(0, _candiesTypes.Length);

            var candComponent = MonoBehaviour.Instantiate(_candiesTypes[randomType], candysGameObject.transform);

            candComponent.transform.localPosition = new Vector3(x, y, 0);

            candComponent.transform.localRotation = Quaternion.identity;

            var gridItem = candComponent.GetComponent<ItemComponent>();

            gridItem.OnItemPositionChanged(x, y);

            return gridItem;
        }


        private GameObject[] LoadCandiesTypes()
        {
            return Resources.LoadAll<GameObject>("Prefabs");
        }


        private void CreateCandysColider(GameObject parent)
        {
            // Cria o novo objeto a partir do prefab
            GameObject colider = new GameObject("Colider");

            Vector3 coliderScale = new Vector3(1000, colider.transform.localScale.y, 0);

            colider.transform.localScale = coliderScale;

            colider.transform.SetParent(parent.transform);

            colider.AddComponent<BoxCollider2D>();

            var prefabType = _candiesTypes[0];

            Vector3 targetPosition = prefabType.transform.position;

            float parentHeight = prefabType.transform.GetParentHeight();

            // Move o componente atual para baixo com base no tamanho do componente de destino
            colider.transform.position = new Vector3(targetPosition.x, parentHeight - 1 - targetPosition.y, targetPosition.z);

        }

    }
}