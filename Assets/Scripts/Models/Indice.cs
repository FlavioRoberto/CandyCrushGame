using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CandyCrush.Models
{
    public class Indice
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Indice(int x, int y)
        {
            X = x;
            Y = y;
        }

    }
}
