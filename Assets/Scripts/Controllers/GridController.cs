using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CandyCrush.Extensions;
using CandyCrush.Models;
using CandyCrush.Services;
using Unity.VisualScripting;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using static UnityEditor.Progress;

public class GridController : MonoBehaviour
{
    public int xSize, ySize;
    public int minimalItensForMatch = 3;
    public GameObject prefab;
    public GameObject candysGroup;

    private GameObject[] _candiesTypes;
    private ItemComponent[,] _itens;
    private ItemComponent _itemSelected;
    private GridService _gridService;

    void Start()
    {
        _gridService = new GridService(xSize, ySize);
        _itens = _gridService.Generate(candysGroup);
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
            StartCoroutine(TryMatch(_itemSelected, item));
        }
        else
        {
            Debug.LogWarning("Não é possível trocar a posição");
        }

        _itemSelected = null;

    }

    IEnumerator DestroyItens(List<ItemComponent> itens)
    {
        foreach(var item in itens)
        {
            if (item != null)
            {
                yield return StartCoroutine(item.transform.Scale(Vector3.zero, 0.05f));
                Destroy(item.gameObject);
            }
        }
    }

    IEnumerator TryMatch(ItemComponent firstItem, ItemComponent secondItem)
    {
        yield return StartCoroutine(Swap(firstItem, secondItem));

        var firstMatchs = GetMatchInformation(firstItem);
        var secondMatchs = GetMatchInformation(secondItem);

        if (!firstMatchs.IsValid() && !secondMatchs.IsValid()){
            yield return StartCoroutine(Swap(firstItem, secondItem));
            yield break;
        }

        if (firstMatchs.IsValid())
        {
          yield return StartCoroutine(DestroyItens(firstMatchs.Matchs));
        }

        else if (secondMatchs.IsValid())
        {
           yield return StartCoroutine(DestroyItens(secondMatchs.Matchs));
        }
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


    //TODO - pensar em uma forma melhor de fazer
    private MatchInfo GetMatchInformation(ItemComponent item)
    {
        var match = new MatchInfo();

        var matchsAtX = _gridService.SearchEqualsItensHorizontally(item, _itens);
        var matchsAtY = _gridService.SearchEqualsItensVertically(item, _itens);

        if(matchsAtX.Count() >= minimalItensForMatch && matchsAtX.Count() > matchsAtY.Count())
        {
            match.AddMatchAtX(matchsAtX, matchsAtY?.FirstOrDefault()?.Y ?? 0);
        }
        
        if(matchsAtY.Count() >= minimalItensForMatch)
        {
            match.AddMatchAtY(matchsAtY, matchsAtX?.FirstOrDefault()?.X ?? 0);
        }

        return match;
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
            if(item != null)
                item.GetComponent<Rigidbody2D>().isKinematic = !change;
        }
    }  
}
