using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuManager {

    // Properties
    private static BottomMenu bottomMenu;

    // Accessors
	public static BottomMenu BottomMenu
	{
		get
		{
			return bottomMenu;
		}
	}

    // Methods
	public static void StartMenuManager () {

        bottomMenu = GameObject.Find("Gui/BottomMenu").GetComponent<BottomMenu>();

    }
}
