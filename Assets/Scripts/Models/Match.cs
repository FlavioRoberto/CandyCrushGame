using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace CandyCrush.Models
{
    public class MatchPosition
    {
        public int Start { get; private set; }
        public int End { get; private set; }

        public MatchPosition(int start, int end)
        {
            Start = start;
            End = end;
        }
    }

    public class MatchInfo
    {
        public List<ItemComponent> Matchs { get; private set; }
        public MatchPosition matchAtX { get; private set; }
        public MatchPosition matchAtY { get; private set; }

        public MatchInfo()
        {
            Matchs = new List<ItemComponent>();
        }

        public bool IsValid()
        {
            return this.Matchs.Any();
        }

        public void AddMatchAtX(List<ItemComponent> matchs, int positionAtY)
        {
            this.Matchs = matchs.OrderBy(item => item.X).ToList();
            var firstItem = this.Matchs.FirstOrDefault();
            var lastItem = this.Matchs.LastOrDefault();
            this.matchAtX = new MatchPosition(firstItem.X, lastItem.X);
            this.matchAtY = new MatchPosition(positionAtY, positionAtY);
        }

        public void AddMatchAtY(List<ItemComponent> matchs, int positionAtX)
        {
            this.Matchs = matchs.OrderBy(item => item.Y).ToList();
            var firstItem = this.Matchs.FirstOrDefault();
            var lastItem = this.Matchs.LastOrDefault();
            this.matchAtY = new MatchPosition(firstItem.Y, lastItem.Y);
            this.matchAtX = new MatchPosition(positionAtX, positionAtX);
        }
    }
}