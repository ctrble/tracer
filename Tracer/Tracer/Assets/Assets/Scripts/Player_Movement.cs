using UnityEngine;
using System.Collections;

public class Player_Movement : MonoBehaviour {

	public Input_Controller playerInput;
	public Vehicle_Stats playerVehicle;
	public Rigidbody rigidBody;

	void OnEnable () {

		if (playerInput == null)
			playerInput = GetComponent<Input_Controller> ();

		if (playerVehicle == null)
			playerVehicle = GetComponent<Vehicle_Stats> ();

		if (rigidBody == null)
			rigidBody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate() {

		Accelerate ();
		Decelerate ();
		Yaw ();
		Pitch ();
	}

	void Accelerate () {

		//go go go
		//position along z axis
		if (playerInput.Button1())
			rigidBody.AddForce(transform.forward * playerVehicle.thrust);
	}

	void Decelerate () {

		//stop stop stop
		//position along z axis
		if (playerInput.Button4 ())
			rigidBody.drag = playerVehicle.brake;
		else
			rigidBody.drag = playerVehicle.drag;
	}

	void Yaw () {
	
		//left and right
		//rotates around y axis
		float xTurn = playerInput.Horizontal();
		rigidBody.AddTorque(transform.up * playerVehicle.torque * xTurn);
	}

	void Pitch () {
	
		//up and down
		//rotates around x axis
		float yTurn = playerInput.Vertical();
		rigidBody.AddTorque(transform.right * playerVehicle.torque/3 * yTurn);

		//angles and such, might need to know how rotated we are later
		//float angle = Quaternion.Angle(transform.rotation, Quaternion.identity);
		//Debug.Log (angle);
	}
}
