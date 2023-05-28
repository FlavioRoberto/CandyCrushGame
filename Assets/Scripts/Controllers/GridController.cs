using System.Collections;
using CandyCrush.Extensions;
using CandyCrush.Services;
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
        var service = new GridService();
        _itens = service.Generate(xSize, ySize, candysGroup);
        ItemComponent.OnMouseOverItemEventHandler += OnMouseOverItem;
    }

    void OnDisable()
    {
        ItemComponent.OnMouseOverItemEventHandler -= OnMouseOverItem;
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

    private void SwapIndices(ItemComponent firstItem, ItemComponent secondItem)
    {
        var firstIndice = firstItem.GetIndice();
        var secondIndice = secondItem.GetIndice();

        firstItem.OnItemPositionChanged(secondIndice);
        secondItem.OnItemPositionChanged(firstIndice);

        _itens.ChangeIndice(firstIndice.X, firstIndice.Y, secondItem);
        _itens.ChangeIndice(secondIndice.X, secondIndice.Y, firstItem);
    }

    private void ActiveRigidbodyStatus(bool change)
    {
        foreach (ItemComponent item in _itens)
        {
            item.GetComponent<Rigidbody2D>().isKinematic = !change;
        }
    }
}
