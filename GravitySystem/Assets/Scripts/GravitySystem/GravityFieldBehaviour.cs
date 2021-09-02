using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFieldBehaviour : MonoBehaviour{
    public GameObject meshPlanetReferece;
    [Header("Only if the script is atatched on Collider Object")]
    [SerializeField] bool switchOnTouch;
    [SerializeField] string playerTag;

    void OnCollisionEnter(Collision collision){
        if (switchOnTouch && collision.gameObject.CompareTag(playerTag)){
            SwitchingPlanet(collision.gameObject);
        }
    }

    public void SwitchingPlanet(GameObject player) {
        player.GetComponent<PlayerManager>().fauxGravity.AlternativeChangingPlanet(meshPlanetReferece);
    }
}
