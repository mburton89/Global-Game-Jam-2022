using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour {

	public static PlayManager Instance;

	public List<PortalCube> portals = new List<PortalCube>();
	public PortalCube portalToComeOutOf;

	public GameObject WinText;

	void Awake(){

		if (Instance == null) {
			Instance = this;
		}
	}
		
	void Start(){
		FindAllPortals();
	}

	void Update(){

	}
		
	public void GameOver(){
		ResetPositionsOfBlocks();
	}

	public void HandlePortal(MovingCube blockToTeleport, Collider col){

		if (portals.Count <= 1)return;



		if (blockToTeleport.canGoThruPortals) {

			print("handle portal");

			blockToTeleport.canGoThruPortals = false;

			PortalCube currentPortal = col.GetComponent<PortalCube>();
			List<PortalCube> allPortals = portals;
			List<PortalCube> potentialPortals = new List<PortalCube>();

			for(int i = 0; i < allPortals.Count; i++){
				if(allPortals[i] != currentPortal){
					potentialPortals.Add(allPortals[i]);
				}
			}

			portalToComeOutOf = potentialPortals[Random.Range(0,potentialPortals.Count)];


			StartCoroutine(portalBuffer(blockToTeleport));
		}
	}

	private IEnumerator portalBuffer(MovingCube blockToTeleport){
		yield return new WaitForSeconds(.5f);
		blockToTeleport.transform.position = portalToComeOutOf.transform.position;
		yield return new WaitForSeconds(1f);
		blockToTeleport.canGoThruPortals = true;
	}



	public void FindAllPortals(){

		List<PortalCube> newPortals = new List<PortalCube>();
		newPortals.AddRange(GetComponentsInChildren<PortalCube>());

		portals = newPortals;

		print("Portal Count: " + portals.Count);
	}

	public void ResetPositionsOfBlocks(){
//		blocks = GetComponentsInChildren<BlockBlock>();
//		foreach (BlockBlock block in blocks) {
//			block.rect.anchoredPosition = block.startingPos;
//		}
	}

	public void HandleHazard(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}	

	public void HandleWin(){
		WinText.transform.localScale = Vector3.one;
		StartCoroutine(DelayedLoadNextScene());
	}

	private IEnumerator DelayedLoadNextScene(){
		yield return new WaitForSeconds(3f);
		WinText.transform.localScale = Vector3.zero;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
