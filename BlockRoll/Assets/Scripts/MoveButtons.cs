using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveButtons : MonoBehaviour {

	public static MoveButtons Instance;

	public List<MovingCube> MovingCubes;

	public UpButton Up;
	public UpButton Down;
	public UpButton Left;
	public UpButton Right;

	void Awake(){

		if(Instance == null){
			Instance = this;
		}

		//	DontDestroyOnLoad(gameObject);
	}

	void Update(){
		if(Up.isPressed){
			PlaneController.Instance.TiltUp();
			MoveCubesUp();
		}else if(Down.isPressed){
			PlaneController.Instance.TiltDown();
			MoveCubesDown();
		}else if(Left.isPressed){
			PlaneController.Instance.TiltLeft();
			MoveCubesLeft();
		}else if(Right.isPressed){
			PlaneController.Instance.TiltRight();
			MoveCubesRight();
		}else{
			PlaneController.Instance.TiltEven();
			StopCubes();
		}

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            PlaneController.Instance.TiltUp();
            MoveCubesUp();
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            PlaneController.Instance.TiltDown();
            MoveCubesDown();
        }

        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            PlaneController.Instance.TiltLeft();
            MoveCubesLeft();
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            PlaneController.Instance.TiltRight();
            MoveCubesRight();
        }
    }

	public void MoveCubesUp(){
		foreach (MovingCube cube in MovingCubes){
			cube.x = 0;
			cube.y = 1;
		}
	}

	public void MoveCubesDown(){
		foreach (MovingCube cube in MovingCubes){
			cube.x = 0;
			cube.y = -1;
		}
	}

	public void MoveCubesLeft(){
		foreach (MovingCube cube in MovingCubes){
			cube.x = -1;
			cube.y = 0;
		}
	}

	public void MoveCubesRight(){
		foreach (MovingCube cube in MovingCubes){
			cube.x = 1;
			cube.y = 0;
		}
	}

	public void StopCubes(){
		foreach (MovingCube cube in MovingCubes){
			if(cube == null){
				MovingCubes.Remove(cube);
				break;
			}
			cube.x = 0;
			cube.y = 0;
		}
	}

	public void AssignMovingCubes(float newX, float newY){
		
		foreach (MovingCube cube in MovingCubes){
			cube.x = newX;
			cube.y = newY;
		}
	}
}
