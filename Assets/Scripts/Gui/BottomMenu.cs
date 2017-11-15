using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomMenu : MonoBehaviour {

    private Transform buildingSubMenu;
    private Transform transportSubMenu;

    private Transform buildingCategoryMenu;
    private Transform buildingPlacementMenu;

    // Use this for initialization
    void Start () {
        
        buildingSubMenu = transform.GetChild(0).GetChild(0);
        transportSubMenu = transform.GetChild(0).GetChild(1);

        buildingCategoryMenu = transform.GetChild(1).GetChild(0);
        buildingPlacementMenu = transform.GetChild(1).GetChild(1);

        ChangeSubMenu(0);
        ChangeMenu(1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeSubMenu (int index) {

        transportSubMenu.gameObject.SetActive(false);
        buildingSubMenu.gameObject.SetActive(false);

        if (index == 0) {
            return;
        }

        if (index == 1) {
            buildingSubMenu.gameObject.SetActive(true);
            return;
        }

		if (index == 2)
		{
			transportSubMenu.gameObject.SetActive(true);
            return;
		}

    }

    public void ChangeMenu (int index) {

        buildingCategoryMenu.gameObject.SetActive(false);
        buildingPlacementMenu.gameObject.SetActive(false);

		if (index == 0)
		{
			return;
		}

		if (index == 1)
		{
			buildingCategoryMenu.gameObject.SetActive(true);
            return;
		}

		if (index == 2)
		{
			buildingPlacementMenu.gameObject.SetActive(true);
            return;
		}

    }

}
