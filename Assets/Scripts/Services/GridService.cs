using System.Collections;
using System.Collections.Generic;
using CandyCrush.Extensions;
using UnityEngine;

namespace CandyCrush.Services
{

    public class GridService
    {
        private CandyService _candyService;
        private int _xSize;
        private int _ySize;

        public GridService(int xSize, int ySize)
        {
            _xSize = xSize;
            _ySize = ySize;
            _candyService = new CandyService();
        }

        public ItemComponent[,] Generate(GameObject candysGameObject)
        {
            var _itens = new ItemComponent[_xSize, _ySize];

            for (int x = 0; x < _xSize; x++)
            {
                for (int y = 0; y < _ySize; y++)
                {
                    _itens[x, y] = _candyService.Create(candysGameObject, x, y);
                }
            }

            _candyService.CreateCandysColider(candysGameObject);

            return _itens;
        }

        public List<ItemComponent> SearchEqualsItensHorizontally(ItemComponent item, ItemComponent[,] itens)
        {
            var list = new List<ItemComponent>();

            list.Add(item);

            var leftIndex = item.X - 1;
            var rightIndex = item.X + 1;

            while (leftIndex >= 0 && itens[leftIndex, item.Y].type == item.type)
            {
                list.Add(itens[leftIndex, item.Y]);
                leftIndex -= 1;
            }

            while (rightIndex < _xSize && itens[rightIndex, item.Y].type == item.type)
            {
                list.Add(itens[rightIndex, item.Y]);
                rightIndex += 1;
            }

            return list;
        }

        public List<ItemComponent> SearchEqualsItensVertically(ItemComponent item, ItemComponent[,] itens)
        {
            var list = new List<ItemComponent>();
            list.Add(item);
            var lowerIndex = item.Y - 1;
            var upperIndex = item.Y + 1;

            while (lowerIndex >= 0 && itens[item.X, lowerIndex].type == item.type)
            {
                list.Add(itens[item.X, lowerIndex]);
                lowerIndex -= 1;
            }

            while (upperIndex < _ySize && itens[item.X, upperIndex].type == item.type)
            {
                list.Add(itens[item.X, upperIndex]);
                upperIndex += 1;
            }

            return list;
        }

    }
}