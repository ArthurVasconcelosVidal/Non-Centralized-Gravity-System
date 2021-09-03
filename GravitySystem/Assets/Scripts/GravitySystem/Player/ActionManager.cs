using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour{
    #region Jump Variables
    bool inJump = false;
    [SerializeField] float jumpForce;
    #endregion

    #region JumpFunctions
    public void Jump(){
        if (PlayerManager.instance.fauxGravity.IsGrounded()){
            StartCoroutine("InJump");
        }
    }

    IEnumerator InJump(){
        inJump = true;
        if (PlayerManager.instance.movimentManager.ActualDirection() != Vector3.zero) PlayerManager.instance.rigidyBody.AddForce((transform.up + PlayerManager.instance.movimentManager.ActualDirection()).normalized * jumpForce, ForceMode.Impulse);
        else PlayerManager.instance.rigidyBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        PlayerManager.instance.animationManager.CallJump(true);
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => PlayerManager.instance.fauxGravity.IsGrounded());
        inJump = false;
        PlayerManager.instance.animationManager.CallJump(false);
    }
    #endregion

    #region Utilities
    public bool IsJumping(){
        return inJump;
    }
    #endregion


}
