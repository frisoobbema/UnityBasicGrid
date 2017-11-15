using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBlock {

	// Ruimte om de nodes in op te slaan.
	private static Node[,] nodes;
    private int positionX;
    private int positionZ;

    public NodeBlock (int _positionX, int _positionZ) {

        positionX = _positionX;
        positionZ = _positionZ;

    }

    public void ShowNodes () {
        
    }

	public void HideNodes() {

	}

}
