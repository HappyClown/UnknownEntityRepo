using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector2 worldPos;
    public int gCost;
    public int hCost;
    public int gridX, gridY;
    public Node parent;

    // Constructor for a Node.
    public Node(bool _walkable, Vector2 _worldPos, int _gridX, int _gridY) {
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }
    // Everytime fCost is called it is equal to gCost + hCost.
    public int fCost {
        get {
            return gCost + hCost;
        }
    }
}
