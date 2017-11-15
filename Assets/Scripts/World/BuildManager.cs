using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour {

    private GameObject tempBuilding;
    private bool placeNewBuilding = false;
    private Vector3 mousePosition;

    // Update is called once per frame
    void Update() {

		// If a structure is being placed set the structure position and color based on node availability
		if (placeNewBuilding) {

            Node node = null;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			// Cancel the raycast if the mouse is over UI elements
			if(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()){
                return;
            }

            // If raycasts hits and mouse has been moved, replace the structure
            if (Physics.Raycast(ray, out hit, 1000) ) {

                // Find the node under the mouse position, and clamp it by building size and grid size to avoid errors.
                node = GetBuildingStartNode(tempBuilding.GetComponent<Structure>(), hit.point);

				if(mousePosition != Input.mousePosition){
                    
					// Set the position of the structure in the grid at the mouse position
					MoveStructureInGrid(tempBuilding.GetComponent<Structure>(), hit.point);
					mousePosition = Input.mousePosition;
				}

			}

            // Place building if user clicks the mouse button.
            if (Input.GetMouseButtonDown(0) && node != null) {

                // Check if occupied
                if (Grid.IsAccessible(node, tempBuilding.GetComponent<Structure>().structureSizeX, tempBuilding.GetComponent<Structure>().structureSizeZ)) {

                    // Set occupied
                    Grid.SetAccessible(node, tempBuilding.GetComponent<Structure>().structureSizeX, tempBuilding.GetComponent<Structure>().structureSizeZ, false);

                    placeNewBuilding = false;
                    tempBuilding = null;

                    MenuManager.BottomMenu.ChangeMenu(1);
                }
            }
        }
    }

	// Start the structure placement.
	public void StartBuildingPlacement (int buildingId) {

        mousePosition = Input.mousePosition;
		placeNewBuilding = true;
        tempBuilding = Instantiate(Resources.Load("GameObjects/Buildings/Building " + buildingId) as GameObject);

        MoveStructureInGrid(tempBuilding.GetComponent<Structure>(), Camera.main.GetComponent<MainCamera>().cameraTarget.transform.position);

        MenuManager.BottomMenu.ChangeSubMenu(0);
        MenuManager.BottomMenu.ChangeMenu(2);
    }

    public void FinishBuildingPlacement () {
        
    }

    // Stop the structure placement.
    public void CancelBuildingPlacement () {

        Destroy(tempBuilding);
        placeNewBuilding = false;

        MenuManager.BottomMenu.ChangeMenu(1);
    }

    // Moves the struccture in the grid and makes sure the structure dont get out of bounds.
    private void MoveStructureInGrid (Structure structure, Vector3 position) {

        Node node = GetBuildingStartNode(structure, position);

        SetStructurePlacementColor(structure, position);

		// Set building position, based on building size and mouse position.
		if (structure.GetComponent<Structure>().structureSizeX % 2 == 0 && structure.GetComponent<Structure>().structureSizeZ % 2 == 0)
		{
            structure.GetGameObject().transform.position = new Vector3(node.gridX + (structure.GetComponent<Structure>().structureSizeX / 2), 0f, node.gridZ + (structure.GetComponent<Structure>().structureSizeZ / 2));
            return;
        }
		else if (structure.GetComponent<Structure>().structureSizeX % 2 == 0 && structure.GetComponent<Structure>().structureSizeZ % 2 != 0)
		{
			structure.GetGameObject().transform.position = new Vector3(node.gridX + (structure.GetComponent<Structure>().structureSizeX / 2), 0f, node.gridZ + (structure.GetComponent<Structure>().structureSizeZ / 2) + 0.5f);
            return;		
        }
		else if (structure.GetComponent<Structure>().structureSizeX % 2 != 0 && structure.GetComponent<Structure>().structureSizeZ % 2 == 0)
		{
			structure.GetGameObject().transform.position = new Vector3(node.gridX + (structure.GetComponent<Structure>().structureSizeX / 2) + 0.5f, 0f, node.gridZ + (structure.GetComponent<Structure>().structureSizeZ / 2));
		    return;
        }
		else
		{
			structure.GetGameObject().transform.position = new Vector3(node.gridX + (structure.GetComponent<Structure>().structureSizeX / 2) + 0.5f, 0f, node.gridZ + (structure.GetComponent<Structure>().structureSizeZ / 2) + 0.5f);
		    return;
        }
    }

	// Set building color to red if one of the nodes are occupied.
	private void SetStructurePlacementColor (Structure structure, Vector3 position) {
		
        Node node = GetBuildingStartNode(structure, position);

		if (!Grid.IsAccessible(node, tempBuilding.GetComponent<Structure>().structureSizeX, tempBuilding.GetComponent<Structure>().structureSizeZ))
		{
			structure.GetGameObject().GetComponent<Renderer>().material.color = Color.red;
		}
		else
		{
			structure.GetGameObject().GetComponent<Renderer>().material.color = Color.white;
		}
    }

	// Find the node under the position, and clamp it by building size and grid size.
	private Node GetBuildingStartNode (Structure structure, Vector3 position) {
        
        return Grid.NodeFromWorldPoint(new Vector3(Mathf.Clamp(position.x, 0f, (Grid.gridSizeX * 1f) - structure.structureSizeX), 0f, Mathf.Clamp(position.z, 0f, (Grid.gridSizeZ * 1f) - structure.structureSizeZ)));
    }

}