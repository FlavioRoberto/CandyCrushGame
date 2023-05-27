using System;
using UnityEngine;

namespace CandyCrush.Services
{
	public class GridItemService
	{
		public GridItemService()
		{
		}

        public GameObject[] LoadCandiesTypes()
        {
            return Resources.LoadAll<GameObject>("Prefabs");
        }
    }
}

