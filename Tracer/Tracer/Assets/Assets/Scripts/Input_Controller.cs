using UnityEngine;
using System.Collections;

public class Input_Controller : MonoBehaviour {

/*

SETUP

Attach this script to each player GameObject.
The player objects should be named "Player 1", "Player 2", and so forth.

Reference this script from other scripts with:

//the Input_Controller script
public Input_Controller playerInput;

		void OnEnable () {

			//get the Input_Controller script
			if (playerInput == null)
				playerInput = GetComponent<Input_Controller> ();
		}

		void Update () {

			//check the button bools
			if(playerInput.Button1())
				//do some stuff
		}

*/

	//player names
	public string player;

	//button names
	private string button1 = "Button1"; //cross
	private string button2 = "Button2"; //circle
	private string button3 = "Button3"; //triangle
	private string button4 = "Button4"; //square

	//axis names
	private string horizontal = "Horizontal";
	private string vertical = "Vertical";

	//axis values
	public float xAxis;
	public float yAxis;

	void Awake () {

		//set the correct player controller
		if (gameObject.transform.name == "Player 1") {

			player = "P1_";
		}

		//set the correct player controller
		if (gameObject.transform.name == "Player 2") {

			player = "P2_";
		}

		SetVariables ();
	}

	public void SetVariables () {
	
		//simplify button names
		horizontal = player + horizontal;
		vertical = player + vertical;
		button1 = player + button1;
		button2 = player + button2;
		button3 = player + button3;
		button4 = player + button4;
	}

	public float Horizontal () {

		xAxis = Input.GetAxis (horizontal);

		return xAxis;
	}

	public float Vertical () {

		yAxis = Input.GetAxis (vertical);

		return yAxis;
	}

	public bool Button1 () {

		if (Input.GetButton (button1)) { //use GetButtonDown if needing true on first frame only
			
			return true;
		} else {
			return false;
		}
	}

	public bool Button2 () {

		if (Input.GetButtonDown (button2)) {

			return true;
		} else {
			return false;
		}
	}

	public bool Button3 () {

		if (Input.GetButtonDown (button3)) {

			return true;
		} else {
			return false;
		}
	}

	public bool Button4 () {

		if (Input.GetButton (button4)) { //use GetButtonDown if needing true on first frame only
			
			return true;
		} else {
			return false;
		}
	}
}
