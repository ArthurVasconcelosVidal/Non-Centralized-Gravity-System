using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour{
    #region Cinemachine Camera
    [SerializeField] CinemachineFreeLook freeLookCM;
    [SerializeField] float camSpeedY = 1; //Default
    [SerializeField] float camSpeedX = 80; //Default
    Vector2 stick;
    #endregion

    void FixedUpdate(){
        CameraMoviment();
    }

    void CameraMoviment(){
        freeLookCM.m_XAxis.Value += stick.x * camSpeedX * Time.fixedDeltaTime;
        freeLookCM.m_YAxis.Value += stick.y * camSpeedY * Time.fixedDeltaTime;
    }

    public void HorizontalValue(float stickValue) {
        stick.x = stickValue;
    }

    public void VerticalValue(float stickValue){
        stick.y = stickValue;
    }
}
