using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UIElements;

public class FauxGravity : MonoBehaviour {

    [Header("Gravity System")]
    [SerializeField] LayerMask gravity_Layer;
    [SerializeField] string gravityTag;
    [SerializeField] string groundTag;
    [SerializeField] float magnitudeForce = 50;
    [SerializeField] Collider gameObjectCollider;
    [SerializeField] GameObject gravityUser = null;
    [SerializeField] GameObject planetReference = null;

    [Header("Utility")]
    [SerializeField] float groundDistanceOffset = 0f; //Default
    [SerializeField] [Range(0, 1)] float rotationInterpolation = 1f;
    [SerializeField] bool applyGravity = true; //Utilitario
    [SerializeField] bool applyRotation = true; //Utilitario
    [SerializeField] bool canChangePlanets = false; //Utilitarios
    [SerializeField] bool setRigidyBodyRotation = true; //Utilitarios

    [Header("Better, But heavy ")]
    [SerializeField] bool useClosestPoint;

    GameObject auxPlanetReference = null;
    float distToGround;
    Rigidbody gameObject_rb;
    Vector3 gravityHit;
    Vector3 gravityForceDirection;
    Vector3 rayDirection;
    Quaternion gravityRotation;
    RaycastHit hit;

    // Use this for initialization
    void Start() {
        #region RigidyBody Config
        gameObject_rb = GetComponent<Rigidbody>();
        gameObject_rb.useGravity = false;
        gameObject_rb.freezeRotation = setRigidyBodyRotation;
        gameObject_rb.interpolation = RigidbodyInterpolation.Interpolate;
        gameObject_rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        #endregion

        #region Collider Config
        if (gameObjectCollider == null) gameObjectCollider = GetComponent<Collider>();
        distToGround = gameObjectCollider.bounds.extents.y;
        #endregion

        if (gravityUser == null) {
            gravityUser = this.gameObject;
        }

    }

    void FixedUpdate(){
        NewGravityRay();
        GravityRotation();
        GravityForceApplication();
    }

    //Gravity system
    #region Gravity System
    void NewGravityRay() {
        if (useClosestPoint) {
            rayDirection = planetReference.GetComponent<MeshTrianglePoints>().NearestTriangleCenter(transform.position);
            rayDirection = rayDirection - transform.position;
        }
        else {
            rayDirection = -gravityUser.transform.up;
        }

        if (Physics.Raycast(gravityUser.transform.position, rayDirection.normalized, out hit, Mathf.Infinity, gravity_Layer)) {
            gravityHit = hit.normal;
        }
    }

    public void ChangingPlanet(GameObject newPlanet) {
        if (!IsGrounded() && newPlanet != auxPlanetReference && canChangePlanets) {
            auxPlanetReference = planetReference;
            planetReference = newPlanet;
        }
    }

    public void AlternativeChangingPlanet(GameObject newPlanet) {
        if (newPlanet != auxPlanetReference && canChangePlanets){
            auxPlanetReference = planetReference;
            planetReference = newPlanet;
        }
    }

    void ClearPreviousPlanet() {
        auxPlanetReference = null;
    }

    void ClearForce() {
        gameObject_rb.velocity = Vector3.zero;
        gameObject_rb.angularVelocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag(gravityTag)) {
            ClearPreviousPlanet();
            ClearForce();
        }
    }

    void OnTriggerEnter(Collider other) {
        GravityFieldBehaviour gravityField;
        if (other.TryGetComponent(out gravityField)) {
            ChangingPlanet(gravityField.meshPlanetReferece);
        }
    }

    void GravityRotation() {
        if (applyRotation) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up.normalized, out hit, distToGround + groundDistanceOffset, gravity_Layer)) {
                gravityRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                gameObject_rb.MoveRotation(Quaternion.Slerp(transform.rotation, gravityRotation, rotationInterpolation));
            }
            else {
                gravityRotation = Quaternion.FromToRotation(transform.up, gravityHit) * transform.rotation;
                gameObject_rb.MoveRotation(Quaternion.Slerp(transform.rotation, gravityRotation, rotationInterpolation));
            }
        }
    }

    void GravityForceApplication() {
        if (applyGravity) {
            gravityForceDirection = -gravityHit;
            gravityForceDirection = gravityForceDirection * magnitudeForce;
            gameObject_rb.AddForce(gravityForceDirection);
        }
    }
    //Gravity System
    #endregion

    //Public Utilities
    #region Public Utilities
    public bool IsGrounded() {
        RaycastHit hitGround;
        if (Physics.Raycast(transform.position, -transform.up, out hitGround, distToGround + 0.1f + groundDistanceOffset) && hitGround.transform.gameObject.CompareTag(groundTag))
            return true;
        else
            return false;
    }

    public RaycastHit ReturnGravityRaycastHit() {
        return hit;
    }

    public void SetApplyGravity(bool value) {
        applyGravity = value;
    }

    public void SetCanChangePlanets(bool value) {
        canChangePlanets = value;
    }

    void SetFreezeRotation(bool value) {
        gameObject_rb.freezeRotation = value;
    }

    public void SetPlanetReference(GameObject newPlanet) {
        auxPlanetReference = planetReference;
        planetReference = newPlanet;
    }

    public GameObject ReturnPlanetReference() {
        return planetReference;
    }

    public void SetGroundDistanceOffset(float groundOffset){
        groundDistanceOffset = groundOffset;
    }

    //Public Utilities
    #endregion
}