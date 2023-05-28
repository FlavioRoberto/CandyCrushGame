using CandyCrush.Enums;
using CandyCrush.Models;
using UnityEngine;

//namespace CandyCrush.Components
//{

public class ItemComponent : MonoBehaviour
{
    public CandyType type;
    public int X { get; private set; }
    public int Y { get; private set; }

    public delegate void OnMouseOverItem(ItemComponent item);
    public static event OnMouseOverItem OnMouseOverItemEventHandler;

    public void OnItemPositionChanged(int x, int y)
    {
        X = x;
        Y = y;
        gameObject.name = $"Sprite [{X}][{Y}]";
    }

    public void OnMouseDown()
    {
        print($"Click on candy {type} at position {X}/{Y}");
        if (OnMouseOverItemEventHandler == null)
            return;

        OnMouseOverItemEventHandler(this);
    }

    public void ChangeIndice(Indice indice)
    {
        X = indice.X;
        Y = indice.Y;
    }

    public Indice GetIndice()
    {
        return new Indice(X, Y);
    }

}
