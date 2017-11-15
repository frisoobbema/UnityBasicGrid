using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {

    private int heapIndex;
    public Node parent;

    private GameObject gameObject;
    public bool accessible = true;
    public int type = 0;

    public int gridX;
    public int gridZ;

    public int gCost;
    public int hCost;

    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    public int HeapIndex {
        get  {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    // Constructor, Node aanmaken 
    public Node (int _posX, int _posZ) {
        gridX = _posX;
        gridZ = _posZ;

        Debug.Log("NodeCreated!");
    }

    // Return de positie in de map van de node
    public Vector3 GetWorldPosition() {
        return new Vector3(gridX + 0.5f, 0f, gridZ + 0.5f);
    }

    // Unity gameObject toevoegen aan de node
    public void SetGameObject(GameObject _gameObject, Transform _parent) {

        _gameObject.transform.position = new Vector3(GetWorldPosition().x, 0f, GetWorldPosition().z);
        _gameObject.name = "node";
        _gameObject.transform.parent = _parent;
        gameObject = _gameObject;
    }

    public int CompareTo(Node nodeToCompare) {

        int compare = fCost.CompareTo(nodeToCompare.fCost);

        if (compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }
}
