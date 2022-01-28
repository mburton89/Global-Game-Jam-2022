using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneController : MonoBehaviour {

	public static PlaneController Instance;

	public Transform from;
	public Transform to;

	public float targetAngle;
	private Vector3 currentAngle;

	public float speed;

	public enum Direction{
		Up,
		Down,
		Left,
		Right,
		Still
	}

	public bool canGoUp;
	public bool canGoDown;
	public bool canGoLeft;
	public bool canGoRight;

	public bool canMove;

	public Button up;
	public Button down;
	public Button left;
	public Button right;

	public Camera camera;
	public MovingCube movingCube;

	public float xCameraPos;
	public float yCameraPos;

	public bool shouldFollowCube;

	void Awake(){
		if(Instance == null){
			Instance = this;
		}
	}

	void Start(){
		currentAngle = transform.eulerAngles;
		targetAngle = -15;
	}

	void Update() {

		if (shouldFollowCube)
		{
			camera.transform.localPosition = new Vector3(movingCube.transform.position.x + xCameraPos, yCameraPos, 10.5f);
		}
			
		if (Input.GetKey(KeyCode.RightArrow)){
			currentAngle = new Vector3(Mathf.LerpAngle(currentAngle.x, targetAngle, Time.deltaTime * speed), 0, currentAngle.z);
		} 

		if (Input.GetKey(KeyCode.LeftArrow)){
			currentAngle = new Vector3(Mathf.LerpAngle(currentAngle.x, -targetAngle, Time.deltaTime * speed), 0, currentAngle.z);
		} 
		if (Input.GetKey(KeyCode.UpArrow)){
			currentAngle = new Vector3(currentAngle.x, 0, Mathf.LerpAngle(currentAngle.z, targetAngle, Time.deltaTime * speed));
		} 
		if (Input.GetKey(KeyCode.DownArrow)){
			currentAngle = new Vector3(currentAngle.x, 0, Mathf.LerpAngle(currentAngle.z, -targetAngle, Time.deltaTime * speed));
		}

		if(!Input.anyKey){
			currentAngle = new Vector3(
				Mathf.LerpAngle(currentAngle.x, 0, Time.deltaTime * speed ),
				Mathf.LerpAngle(currentAngle.y, 0, Time.deltaTime * speed ),
				Mathf.LerpAngle(currentAngle.z, 0, Time.deltaTime * speed ));
		}

		transform.eulerAngles = currentAngle;
	}

	public void TiltUp(){
		currentAngle = new Vector3(currentAngle.x, 0, Mathf.LerpAngle(currentAngle.z, targetAngle, Time.deltaTime * speed));
		transform.eulerAngles = currentAngle;
	}

	public void TiltDown(){
		currentAngle = new Vector3(currentAngle.x, 0, Mathf.LerpAngle(currentAngle.z, -targetAngle, Time.deltaTime * speed));
		transform.eulerAngles = currentAngle;

	}

	public void TiltLeft(){
		currentAngle = new Vector3(Mathf.LerpAngle(currentAngle.x, -targetAngle, Time.deltaTime * speed), 0, currentAngle.z);
		transform.eulerAngles = currentAngle;

	}

	public void TiltRight(){
		currentAngle = new Vector3(Mathf.LerpAngle(currentAngle.x, targetAngle, Time.deltaTime * speed), 0, currentAngle.z);
		transform.eulerAngles = currentAngle;

	}

	public void TiltEven(){
		currentAngle = new Vector3(
			Mathf.LerpAngle(currentAngle.x, 0, Time.deltaTime * speed ),
			Mathf.LerpAngle(currentAngle.y, 0, Time.deltaTime * speed ),
			Mathf.LerpAngle(currentAngle.z, 0, Time.deltaTime * speed ));

		transform.eulerAngles = currentAngle;
	}
}
