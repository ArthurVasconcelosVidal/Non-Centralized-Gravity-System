using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour{
    static public PlayerManager instance;

    public Rigidbody rigidyBody;
    public FauxGravity fauxGravity;
    public InputManager inputManager;
    public MovimentManager movimentManager;
    public ActionManager actionManager;
    public CameraManager cameraManager;
    public AnimationManager animationManager;
    public GameObject meshObject; //inspector
    public GameObject mainCamera; //inspector

    void Awake(){
        if (instance == null){
            instance = this;
        }
        else{
            Destroy(this);
        }
        DontDestroyOnLoad(instance);
    }
}
