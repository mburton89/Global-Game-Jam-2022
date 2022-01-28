using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{

	public bool isPressed;

	public void OnPointerDown(PointerEventData eventData){
		Debug.Log(this.gameObject.name + " Was Clicked.");
		isPressed = true;
	}

	public void OnPointerUp(PointerEventData eventData){
		Debug.Log(this.gameObject.name + " Was Let Go.");
		isPressed = false;
	}
}
