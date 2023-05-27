using CandyCrush.Helpers;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public int xSize, ySize;
    public GameObject prefab;
    public GameObject candysGroup;

    private GameObject[] _candiesTypes;
    private ItemComponent[,] _itens;

    void Start()
    {
        LoadCandiesTypes();
        Fill();
    }

    private void Fill()
    {
        _itens = new ItemComponent[xSize, ySize];

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                _itens[x, y] = GenerateCandy(candysGroup, x, y);
            }
        }

        CreateColider(candysGroup);
    }

    private ItemComponent GenerateCandy(GameObject parent, int x, int y)
    {
        var randomType = Random.Range(0, _candiesTypes.Length);

        var candComponent = Instantiate(_candiesTypes[randomType], parent.transform);

        candComponent.transform.localPosition = new Vector3(x, y, 0);

        candComponent.transform.localRotation = Quaternion.identity;

        var gridItem = candComponent.GetComponent<ItemComponent>();

        gridItem.OnItemPositionChanged(x, y);

        return gridItem;
    }


    private void CreateColider(GameObject parent)
    {
        // Cria o novo objeto a partir do prefab
        GameObject colider = new GameObject("Colider");

        Vector3 coliderScale = new Vector3(1000, colider.transform.localScale.y, 0);

        colider.transform.localScale = coliderScale;

        colider.transform.SetParent(parent.transform);

        colider.AddComponent<BoxCollider2D>();

        var prefabType = _candiesTypes[0];

        Vector3 targetPosition = prefabType.transform.position;

        float parentHeight = ParentHelper.GetParentHeight(prefabType.transform);

        // Move o componente atual para baixo com base no tamanho do componente de destino
        colider.transform.position = new Vector3(targetPosition.x, parentHeight - 1 - targetPosition.y, targetPosition.z);

    }

    private void LoadCandiesTypes()
    {
        _candiesTypes = Resources.LoadAll<GameObject>("Prefabs");
    }
}
