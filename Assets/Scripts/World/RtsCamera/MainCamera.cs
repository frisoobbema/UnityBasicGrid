using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {
	
	public GameObject cameraTarget;

	public Texture2D selectionHighlight = null;
	public static Rect selection = new Rect(0, 0, 0, 0);
	private Vector3 startClick = -Vector3.one;

    // Start waarden camera
	private float _cameraRotation = 1;
	private float _cameraZoom = 2;
	
	private float _xMin;
	private float _zMin;
	private float _xMax;
	private float _zMax;
	
	private Vector3 offset;

    private float previousMousePositionHorizontal = 0;
    private float previousMousePositionVertical = 0;

    // Use this for initialization
    void Start () {
        
		construct (0, 0, Grid.gridSizeX, Grid.gridSizeZ);
	}
	
	// Input verwerken
	void Update () {

        float mouseDragHorizontal = 0;
        float mouseDragVertical = 0;

        float desiredAngle = this.cameraTarget.transform.eulerAngles.y;
		Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
		transform.position = this.cameraTarget.transform.position - (rotation * offset);
		Vector3 newPosition;

        // Move camara vertical
        if (Input.GetAxis("Vertical") != 0) {
			newPosition = new Vector3 (this.cameraTarget.transform.position.x + (Input.GetAxis("Vertical") * this.cameraTarget.transform.forward.x), 0, this.cameraTarget.transform.position.z + (Input.GetAxis("Vertical") * this.cameraTarget.transform.forward.z));
			this.newCameraPosition(newPosition);
		}
		
        // Move camara horizontal
		if (Input.GetAxis("Horizontal") != 0) {
			newPosition =  new Vector3 (this.cameraTarget.transform.position.x + (Input.GetAxis("Horizontal") * this.cameraTarget.transform.right.x), 0, this.cameraTarget.transform.position.z + (Input.GetAxis("Horizontal") * this.cameraTarget.transform.right.z));
			this.newCameraPosition(newPosition);
		}

        // Get mouseDrag values when middle mouse button is pressed
        if ( Input.GetMouseButton(2) ) {

            if (previousMousePositionHorizontal != 0) {
                mouseDragHorizontal = (previousMousePositionHorizontal - Input.mousePosition.x) / 100;
            }

            if (previousMousePositionVertical != 0) {
                mouseDragVertical = (previousMousePositionVertical - Input.mousePosition.y) / 100;
            }

            previousMousePositionHorizontal = Input.mousePosition.x;
            previousMousePositionVertical = Input.mousePosition.y;

            if (mouseDragHorizontal != 0) {
                this._cameraRotation += mouseDragHorizontal;
            }

            if (mouseDragVertical != 0) {
                this._cameraZoom = Mathf.Clamp(this._cameraZoom += mouseDragVertical, 1f, 4f);
            }

        }

        if (Input.GetMouseButtonUp(2)) {
            previousMousePositionHorizontal = 0;
            previousMousePositionVertical = 0;
        }

        this.offset = new Vector3(0f, -(this._cameraZoom * 4f) ,(this._cameraZoom * 4f) );
		this.cameraTarget.transform.localEulerAngles = new Vector3(0, (this._cameraRotation * 45f) ,0);
		transform.position = this.cameraTarget.transform.position - (rotation * offset);

		CheckCamera ();
		
	}
	
	void LateUpdate() {
        
		transform.LookAt(this.cameraTarget.transform);
	}

	// Instelling propertys aanmaken
	public void construct ( float startPosX, float startPosZ, int sizeX, int sizeZ ) {
		
		this._xMin = startPosX;
		this._zMin = startPosZ;
		this._xMax = sizeX;
		this._zMax = sizeZ;
		
		this.centerCameraToGridBlock ();
	}

	// Beweegt camera doel over map en zorgt ervoor dat deze niet over de limieten gaat
	private void newCameraPosition ( Vector3 newPosition ) {
		
		float xMinSpeed = -1;
		float xMaxSpeed = 1;
		float zMinSpeed = -1;
		float zMaxSpeed = 1;
		
		if(this.cameraTarget.transform.position.x > newPosition.x){
			if ( newPosition.x < this._xMin + 1f ) {
				xMinSpeed = this._xMin - this.cameraTarget.transform.position.x;
			}
		}
		
		if(this.cameraTarget.transform.position.x < newPosition.x){
			if ( newPosition.x > this._xMax - 1f ) {
				xMaxSpeed = this._xMax - this.cameraTarget.transform.position.x;
			}
		}
		
		if(this.cameraTarget.transform.position.z > newPosition.z){
			if ( newPosition.z < this._zMin + 1f ) {
				zMinSpeed = this._zMin - this.cameraTarget.transform.position.z;
			}
		}
		
		if(this.cameraTarget.transform.position.z < newPosition.z){
			if ( newPosition.z > this._zMax - 1f ) {
				zMaxSpeed = this._zMax - this.cameraTarget.transform.position.z;
			}
		}
		
		float xMovement = Mathf.Clamp(newPosition.x - this.cameraTarget.transform.position.x, xMinSpeed, xMaxSpeed);
		float zMovement = Mathf.Clamp(newPosition.z - this.cameraTarget.transform.position.z, zMinSpeed, zMaxSpeed);
		
		newPosition =  new Vector3 (this.cameraTarget.transform.position.x + xMovement, 0, this.cameraTarget.transform.position.z + zMovement);
		
		this.cameraTarget.transform.position = newPosition;
	}

	// Centreert Camera in map
	private void centerCameraToGridBlock ( ) {
        
		this.cameraTarget.transform.position = new Vector3 ( ((this._xMax / 2 ) + this._xMin), 0, ((this._zMax / 2 ) + this._zMin) );
	}

	// Maar selectie rectangle aan
	private void CheckCamera () {
        
		if (Input.GetMouseButtonDown (0)) {
			startClick = Input.mousePosition;
		} 
		else if (Input.GetMouseButtonUp (0)) {
			startClick = -Vector3.one;
		}
		if (Input.GetMouseButton (0)) {
			selection = new Rect(startClick.x, InvertMouseY(startClick.y), Input.mousePosition.x - startClick.x, InvertMouseY( Input.mousePosition.y ) - InvertMouseY( startClick.y ) );
		
			if(selection.width < 0) {
				selection.x += selection.width;
				selection.width = -selection.width;
			}
			if (selection.height < 0) {
				selection.y += selection.height;
				selection.height = -selection.height;
			}
		
		} 
	}

	private void OnGUI () {
        
		if (startClick != -Vector3.one) {
			GUI.color = new Color(1, 1, 1, 0.5f);
			GUI.DrawTexture(selection, selectionHighlight);
		}
	}

	public static float InvertMouseY (float y) {
        
		return Screen.height - y;
	}
}
