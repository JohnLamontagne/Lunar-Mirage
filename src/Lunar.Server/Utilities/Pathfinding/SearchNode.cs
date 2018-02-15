﻿using Lunar.Core.Utilities.Data;

namespace Lunar.Server.Utilities.Pathfinding
{
    public class SearchNode
    {
        /// <summary>
        /// A reference to the node that transfered this node to
        /// the open list. This will be used to trace our path back
        /// from the goal node to the start node.
        /// </summary>
        public SearchNode Parent { get; set; }

        /// <summary>
        /// Provides an easy way to check if this node
        /// is in the open list.
        /// </summary>id
        public bool InOpenList { get; set; }
        /// <summary>
        /// Provides an easy way to check if this node
        /// is in the closed list.
        /// </summary>
        public bool InClosedList { get; set; }

        /// <summary>
        /// The approximate distance from the start node to the
        /// goal node if the path goes through this node. (F)
        /// </summary>
        public float DistanceToGoal { get; set; }
        /// <summary>
        /// Distance traveled from the spawn point. (G)
        /// </summary>
        public float DistanceTraveled { get; set; }

        private SearchNode[] _neighbors;

        public Vector Position { get; private set; }

        public bool Walkable { get; set; }

        public SearchNode(Vector position)
        {
            this.Position = position;
        }

        public void SetNeighbors(SearchNode[] neighbors)
        {
            _neighbors = neighbors;
        }

        public SearchNode[] GetNeighbors()
        {
            return _neighbors;
        }
    }
}