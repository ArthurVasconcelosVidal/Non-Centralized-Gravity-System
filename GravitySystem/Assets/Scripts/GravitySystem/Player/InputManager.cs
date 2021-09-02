using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour{
    public Vector2 leftStick; //TESTE
    public Vector2 rightStick; //TESTE

    public void OnSouthButton(){
        PlayerManager.instance.actionManager.Jump();
    }

    public void OnRightStick(InputValue inputValue){
        rightStick = inputValue.Get<Vector2>();
        PlayerManager.instance.cameraManager.HorizontalValue(rightStick.x);
        PlayerManager.instance.cameraManager.VerticalValue(rightStick.y);
    }

    public void OnLeftStick(InputValue inputValue){
        leftStick = inputValue.Get<Vector2>();
    }

    public void OnDpad(InputValue inputValue){
    }

    public void OnLeftButton() {
    }

    public void OnNorthButton(InputValue ctx){
    }

    public void OnRightButton(InputValue ctx) {
    }

    public void OnStartButton(){
    }
}
