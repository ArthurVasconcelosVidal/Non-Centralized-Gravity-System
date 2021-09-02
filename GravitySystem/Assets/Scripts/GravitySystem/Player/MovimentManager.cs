using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MovimentManager : MonoBehaviour{
    #region Movimentation
    Vector2 leftStick;
    Vector2 rightStick;
    Vector3 leftStickDirection;
    Vector3 rightStickDirection;
    [SerializeField] float moveSpeed = 10; //Default
    [SerializeField] float rotationSpeed = 5; //Default
    [SerializeField] bool useCamera = true; //Utilitario
    #endregion

    void Update(){
        leftStick = PlayerManager.instance.inputManager.leftStick;
        Movimentation();
    }

    void Movimentation(){
        leftStick = Vector2.ClampMagnitude(leftStick, 1);
        rightStick = Vector2.ClampMagnitude(rightStick, 1);

        if (useCamera)
            leftStickDirection = RelativeToCameraDirection(PlayerManager.instance.mainCamera, leftStick);
        else
            leftStickDirection = (transform.forward * leftStick.y + transform.right * leftStick.x);
        
        rightStickDirection = (transform.forward * rightStick.y + transform.right * rightStick.x);

        //Movimentar
        if (leftStickDirection != Vector3.zero) {
            PlayerManager.instance.rigidyBody.MovePosition(PlayerManager.instance.rigidyBody.position + leftStickDirection * moveSpeed * Time.fixedDeltaTime); 
        }

        //Rotacionar o personagem
        if (rightStickDirection != Vector3.zero)
            MeshRotation(rightStickDirection, PlayerManager.instance.meshObject);
        else
            MeshRotation(leftStickDirection, PlayerManager.instance.meshObject);
    }

    public void MeshRotation(Vector3 direction, GameObject mesh){
        if (direction != Vector3.zero){
            var quaternionLook = Quaternion.LookRotation(direction, transform.up);
            mesh.transform.rotation = Quaternion.Slerp(mesh.transform.rotation, quaternionLook, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    public Vector3 RelativeToCameraDirection(GameObject camera, Vector2 stickDirection){
        Vector3 camToPLayerDirectionF = Vector3.ProjectOnPlane(camera.transform.forward.normalized, transform.up.normalized);
        Vector3 camToPlayerDirectionR = Vector3.ProjectOnPlane(camera.transform.right.normalized, transform.up.normalized);
        Vector3 axisDir = camToPLayerDirectionF.normalized * stickDirection.y + camToPlayerDirectionR.normalized * stickDirection.x;
        return axisDir;
    }

    //InputSystem
    #region InputSystem
    public void RightStick(Vector2 stickValue){
        rightStick = stickValue;
    }

    public void LeftStick(Vector2 stickValue){
        leftStick = stickValue;
    }
    #endregion
}