using System.Collections;
using CandyCrush.Extensions;
using UnityEngine;
using static UnityEditor.Progress;

public class GridController : MonoBehaviour
{
    public int xSize, ySize;
    public GameObject prefab;
    public GameObject candysGroup;

    private GameObject[] _candiesTypes;
    private ItemComponent[,] _itens;
    private ItemComponent _itemSelected;

    void Start()
    {
        LoadCandiesTypes();
        Fill();
        ItemComponent.OnMouseOverItemEventHandler += OnMouseOverItem;
    }

    void OnDisable()
    {
        ItemComponent.OnMouseOverItemEventHandler -= OnMouseOverItem;
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

        float parentHeight = prefabType.transform.GetParentHeight();

        // Move o componente atual para baixo com base no tamanho do componente de destino
        colider.transform.position = new Vector3(targetPosition.x, parentHeight - 1 - targetPosition.y, targetPosition.z);

    }

    void OnMouseOverItem(ItemComponent item)
    {
        if (_itemSelected == item)
            return;

        if (_itemSelected == null)
        {
            _itemSelected = item;
            return;
        }

        var xDistance = Mathf.Abs(item.X - _itemSelected.X);
        var yDistance = Mathf.Abs(item.Y - _itemSelected.Y);

        if (xDistance + yDistance == 1)
        {
            StartCoroutine(Swap(_itemSelected, item));
        }
        else
        {
            Debug.LogWarning("Não é possível trocar a posição");
        }

        _itemSelected = null;

    }

    private void SwapIndices(ItemComponent firstItem, ItemComponent secondItem)
    {
        var firstIndice = firstItem.GetIndice();
        var secondIndice = secondItem.GetIndice();

        firstItem.ChangeIndice(secondIndice);
        secondItem.ChangeIndice(firstIndice);

        _itens.ChangeIndice(firstIndice.X, firstIndice.Y, secondItem);
        _itens.ChangeIndice(secondIndice.X, secondIndice.Y, firstItem);
    }

    IEnumerator Swap(ItemComponent firstItem, ItemComponent secondItem)
    {
        var duration = 0.1f;

        var firstItemPosition = firstItem.transform.position;

        ActiveRigidbodyStatus(false);

        //Swap
        StartCoroutine(
            firstItem.transform.Move(secondItem.transform.position, duration)
        );
        StartCoroutine(
            secondItem.transform.Move(firstItemPosition, duration)
        );

        SwapIndices(firstItem, secondItem);

        yield return new WaitForSeconds(duration);

        ActiveRigidbodyStatus(true);
    }

    private void LoadCandiesTypes()
    {
        _candiesTypes = Resources.LoadAll<GameObject>("Prefabs");
    }

    private void ActiveRigidbodyStatus(bool change)
    {
        foreach (ItemComponent item in _itens)
        {
            item.GetComponent<Rigidbody2D>().isKinematic = !change;
        }
    }
}
