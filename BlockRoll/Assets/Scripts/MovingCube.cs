using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MovingCube : MonoBehaviour {

	public float rotationPeriod = 0.3f;		// 隣に移動するのにかかる時間
	public float sideLength = 1f;			// Cubeの辺の長さ

	bool isRotate = false;					// Cubeが回転中かどうかを検出するフラグ
	float directionX = 0;					// 回転方向フラグ
	float directionZ = 0;					// 回転方向フラグ

	Vector3 startPos;						// 回転前のCubeの位置
	float rotationTime = 0;					// 回転中の時間経過
	float radius;							// 重心の軌道半径
	Quaternion fromRotation;				// 回転前のCubeのクォータニオン
	Quaternion toRotation;					// 回転後のCubeのクォータニオン

	public Rigidbody rigidBody;
	public float ratio;

	public bool canMoveUp;
	public bool canMoveDown;
	public bool canMoveLeft;
	public bool canMoveRight;

	public bool canGoThruPortals;

	public UpButton up;
	public UpButton down;
	public UpButton left;
	public UpButton right;

	public float x;
	public float y;

	void Awake(){
		rigidBody = GetComponent<Rigidbody>();
	}

	// Use this for initialization
	void Start () {
		canMoveUp = true;
		canMoveDown = true;
		canMoveLeft = true;
		canMoveRight = true;

		// 重心の回転軌道半径を計算
		radius = sideLength * Mathf.Sqrt (2f) / 2f;

		canGoThruPortals = true;

		up = MoveButtons.Instance.Up;
		down = MoveButtons.Instance.Down;
		left = MoveButtons.Instance.Left;
		right = MoveButtons.Instance.Right;

		MoveButtons.Instance.MovingCubes.Add(this);
	}

	// Update is called once per frame
	void Update () {

		if((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && !canMoveUp) return;
		if((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !canMoveDown) return;
		if((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !canMoveLeft) return;
		if((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))&& !canMoveRight) return;

		if(up.isPressed && !canMoveUp) return;
		if(down.isPressed && !canMoveDown) return;
		if(left.isPressed && !canMoveLeft) return;
		if(right.isPressed && !canMoveRight) return;
//		x = 0;
//		y = 0;
//
//		x = Input.GetAxisRaw ("Horizontal");
//		if (x == 0) {
//			y = Input.GetAxisRaw ("Vertical");
//		}
//
//		print("x: " + x);
//		print("y: " + y);

//		if(x != 0){
//			print("X: " + x);
//		}
//
//		if(y != 0){
//			print("Y: " + y);
//		}
//
//		print("x" + Input.gyro.attitude.eulerAngles.x);
//		print("Y" + Input.gyro.attitude.eulerAngles.y);
//		print("Z" + Input.gyro.attitude.eulerAngles.z);
//
//		print("x" + Input.acceleration.x);
//		print("y" + Input.acceleration.y);
//		print("z" + Input.acceleration.z);
//
//		if(Input.acceleration.x < -.35f){
//			if(!canMoveLeft)return;
//			x = -1;
//		}else if(Input.acceleration.x > .35f){
//			if(!canMoveRight)return;
//			x = 1;
//		}else if(Input.acceleration.y < -.35f){
//			if(!canMoveDown)return;
//			y = -1;
//		}else if(Input.acceleration.y > .35f){
//			if(!canMoveUp)return;
//			y = 1;
//		}
//

		//UNCOMMENT FOR TILT DEVICE
//		if(Input.acceleration.x < -.35f){
//			if(!canMoveLeft)return;
//			x = -1;
//		}else if(Input.acceleration.x > .35f){
//			if(!canMoveRight)return;
//			x = 1;
//		}else if(Input.acceleration.y < -.35f){
//			if(!canMoveDown)return;
//			y = -1;
//		}else if(Input.acceleration.y > .35f){
//			if(!canMoveUp)return;
//			y = 1;
//		}


		// キー入力がある　かつ　Cubeが回転中でない場合、Cubeを回転する。
		if ((x != 0 || y != 0) && !isRotate) {
			directionX = y;																// 回転方向セット (x,yどちらかは必ず0)
			directionZ = x;																// 回転方向セット (x,yどちらかは必ず0)
			startPos = transform.position;												// 回転前の座標を保持
			fromRotation = transform.rotation;											// 回転前のクォータニオンを保持
			transform.Rotate (directionZ * 90, 0, directionX * 90, Space.World);		// 回転方向に90度回転させる
			toRotation = transform.rotation;											// 回転後のクォータニオンを保持
			transform.rotation = fromRotation;											// CubeのRotationを回転前に戻す。（transformのシャローコピーとかできないんだろうか…。）
			rotationTime = 0;															// 回転中の経過時間を0に。
			isRotate = true;															// 回転中フラグをたてる。
		}

		if (transform.position.y < -25) {
			PlayManager.Instance.HandleHazard();
		}
	}

	void OnTriggerStay (Collider col)
	{
		if(col.gameObject.name == "Bottom"){
			canMoveUp = false;
		}

		if(col.gameObject.name == "Top"){
			canMoveDown = false;
		}

		if(col.gameObject.name == "Right"){
			canMoveLeft = false;
		}

		if(col.gameObject.name == "Left"){
			canMoveRight = false;
		}
	}

	void OnTriggerExit (Collider col)
	{
		if(col.gameObject.name == "Bottom"){
			canMoveUp = true;
		}

		if(col.gameObject.name == "Top"){
			canMoveDown = true;
		}

		if(col.gameObject.name == "Right"){
			canMoveLeft = true;
		}

		if(col.gameObject.name == "Left"){
			canMoveRight = true;
		}
	}

	void FixedUpdate() {

		if (isRotate) {

			rotationTime += Time.fixedDeltaTime;									// 経過時間を増やす
			ratio = Mathf.Lerp(0, 1, rotationTime / rotationPeriod);			// 回転の時間に対する今の経過時間の割合

			// 移動
			float thetaRad = Mathf.Lerp(0, Mathf.PI / 2f, ratio);					// 回転角をラジアンで。
			float distanceX = -directionX * radius * (Mathf.Cos (45f * Mathf.Deg2Rad) - Mathf.Cos (45f * Mathf.Deg2Rad + thetaRad));		// X軸の移動距離。 -の符号はキーと移動の向きを合わせるため。
			float distanceY = radius * (Mathf.Sin(45f * Mathf.Deg2Rad + thetaRad) - Mathf.Sin (45f * Mathf.Deg2Rad));						// Y軸の移動距離
			float distanceZ = directionZ * radius * (Mathf.Cos (45f * Mathf.Deg2Rad) - Mathf.Cos (45f * Mathf.Deg2Rad + thetaRad));			// Z軸の移動距離
			transform.position = new Vector3(startPos.x + distanceX, startPos.y + distanceY, startPos.z + distanceZ);						// 現在の位置をセット

			// 回転
			transform.rotation = Quaternion.Lerp(fromRotation, toRotation, ratio);		// Quaternion.Lerpで現在の回転角をセット（なんて便利な関数）

			// 移動・回転終了時に各パラメータを初期化。isRotateフラグを下ろす。
			if (ratio == 1) {
				isRotate = false;
				directionX = 0;
				directionZ = 0;
				rotationTime = 0;
			}
		}
	}

	void OnTriggerEnter (Collider col){
		if (col.GetComponent<PortalCube>()) {
			PlayManager.Instance.HandlePortal(this, col);			
		}else if (col.GetComponent<HazardCube>()) {
			PlayManager.Instance.HandleHazard();			
		}else if (col.GetComponent<MovingCubeCollisionDetector>()) {
			PlayManager.Instance.HandleWin();			
		}
	}
}