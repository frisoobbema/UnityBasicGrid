using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    // Grootte van de kaart.
    public static int gridSizeX = 10;
    public static int gridSizeZ = 10;

    // Return het maximaal aantal aanwezige nodes.
    public int MaxSize {
        get {
            return gridSizeX * gridSizeZ;
        }
    }

    // Ruimte om de nodes in op te slaan.
    private static Node[,] nodes;

    // Unity start.
    void Start () {

        nodes = new Node[gridSizeX, gridSizeZ];

        // Nodes aanmaken en positie in de kaart meegeven.
        for (int y = 0; y < gridSizeZ; y++) {

            for (int x = 0; x < gridSizeX; x++) {

                nodes[x, y] = new Node(x, y);
                nodes[x, y].type = 1;

            }
        }

        // Als de nodes aangemaakt deze zichtbaar maken.
        RenderNodes();
    }

    // Return de toegankelijkheid van de meegegeven node.
    public static bool IsAccessible(Node node) {
        return node.accessible;
    }

    // Return de toegankelijkheid een aangegeven gebied.
    public static bool IsAccessible(Node node, int stepsX, int stepsZ) {
		
        foreach (Node n in SelectNodesInSquare(node.gridX, node.gridZ, stepsX, stepsZ) ) {
            if (!n.accessible) {
                return false;
            }
        }

        return true;
    }

    // Set de toegankelijkheid van de meegegeven node.
    public static void SetAccessible(Node node, bool value) {

        node.accessible = value;
    }

    // Set de toegankelijkheid een aangegeven gebied.
    public static void SetAccessible(Node node, int stepsX, int stepsZ, bool value) {

        foreach (Node n in SelectNodesInSquare(node.gridX, node.gridZ, stepsX, stepsZ)) {
            n.accessible = value;
        }
    }

    // Return de onderliggende node vanuit de meegegeven coordinaten.
    public static Node NodeFromWorldPoint(Vector3 worldPoint) {
		
        return nodes[Mathf.FloorToInt(worldPoint.x), Mathf.FloorToInt(worldPoint.z)];
    }

    // Return de naastgelegen nodes van de meegegeven node.
    public List<Node> GetNeighbours(Node node) {

        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++) {

            for (int z = -1; z <= 1; z++) {

                if (x == 0 && z == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkZ = node.gridZ + z;

                if (checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ) {

                    neighbours.Add(nodes[checkX, checkZ]);

                }
            }
        }

        return neighbours;
    }

    // Voeg een gameObject toe aan de nodes zodat de nodes zichtbaar worden in de game engine.
    private void RenderNodes() {

        foreach (Node n in nodes) {

            if (n.type == 1) {
                n.SetGameObject(Instantiate(Resources.Load("GameObjects/World/landNode") as GameObject), transform);
                continue;
            }
            n.SetGameObject(Instantiate(Resources.Load("GameObjects/World/waterNode") as GameObject), transform);

        }
    }

    // Return meerdere nodes d.m.v. de startpositie en aangegven grootte.
    private static List<Node> SelectNodesInSquare (int startPosX, int startPosZ, int stepsX, int stepsZ) {

        List<Node> nodeList = new List<Node>();

        for (int z = 0; z < stepsZ; z++) {
            for (int x = 0; x < stepsX; x++) {

                if (startPosX + x >= gridSizeX || startPosZ + z >= gridSizeZ) {
                    continue;
                }

                nodeList.Add(nodes[startPosX + x, startPosZ + z]);
            }
        }

        return nodeList;
    }

    void OnDrawGizmosSelected() {

        foreach(Node n in nodes) {
            Gizmos.color = Color.grey;

            if(!n.accessible)
                Gizmos.color = Color.red;

            Gizmos.DrawCube(n.GetWorldPosition(), new Vector3(.5f,.5f, .5f));
        }
    }

}