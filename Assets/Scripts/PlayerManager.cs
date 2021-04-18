using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public enum PlayerClass
{
    Commando,
    Assault,
    Scout,
    Demo
}
public class PlayerManager : MonoBehaviour
{

    enum Weapon
    {
        Primary,
        Secondary
    }

    //these will only contain one weapon...for now
    public List<SingleBulletFire> CommandoWeapons = new List<SingleBulletFire>();
    public List<SingleBulletFire> AssaultWeapons = new List<SingleBulletFire>();
    public List<SingleBulletFire> ScoutWeapons = new List<SingleBulletFire>();
    public List<SingleBulletFire> DemoWeapons = new List<SingleBulletFire>();
    public SingleBulletFire Pistol;

    //A list that contains all weapons. Used to store
    //the prefabs so we can just switch them on and off.
    List<List<SingleBulletFire>> weapons = new List<List<SingleBulletFire>>();
    public SingleBulletFire currentActiveWeapon;


    //These scriptable objects contain information like
    //health, armour, potentially class abilities in the future?
    public PlayerScriptableObject CommandoInfo;
    public PlayerScriptableObject AssaultInfo;
    public PlayerScriptableObject ScoutInfo;
    public PlayerScriptableObject DemoInfo;
    List<PlayerScriptableObject> classInfo = new List<PlayerScriptableObject>();
    public PlayerClass currentPlayerClass;
    PlayerScriptableObject currentClassInfo;


    //Player Controller members
    Vector2 moveVec = new Vector2();
    public float moveSpeed = 5.0f;
    public float movementFalloffRate = 2.0f;
    public Rigidbody pRigidBody;
    public float jumpForceScalar;

    public GameObject pCamera;
    public GameObject lookAt;
    Vector2 mouseVec = new Vector2();
    public float yaw = 1.0f;
    public float pitch = 1.0f;

    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CommandoWeapons.Add(Pistol);
        AssaultWeapons.Add(Pistol);
        ScoutWeapons.Add(Pistol);
        DemoWeapons.Add(Pistol);

        weapons.Add(CommandoWeapons);
        weapons.Add(AssaultWeapons);
        weapons.Add(ScoutWeapons);
        weapons.Add(DemoWeapons);

        classInfo.Add(CommandoInfo);
        classInfo.Add(AssaultInfo);
        classInfo.Add(ScoutInfo);
        classInfo.Add(DemoInfo);

        currentPlayerClass = (PlayerClass)PlayerPrefs.GetInt("Player Class");
        currentClassInfo = classInfo[(int)currentPlayerClass];

        SelectPrimary();
    }

    public void SelectPrimary()
    {
        foreach (var weapon in weapons[(int)currentPlayerClass])
            weapon.gameObject.SetActive(false);

        weapons[(int)currentPlayerClass][(int)Weapon.Primary].gameObject.SetActive(true);
        currentActiveWeapon = weapons[(int)currentPlayerClass][(int)Weapon.Primary];
    }
    public void SelectSecondary()
    {
        foreach (var weapon in weapons[(int)currentPlayerClass])
            weapon.gameObject.SetActive(false);

        weapons[(int)currentPlayerClass][(int)Weapon.Secondary].gameObject.SetActive(true);
        currentActiveWeapon = weapons[(int)currentPlayerClass][(int)Weapon.Secondary];
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveVec = ctx.ReadValue<Vector2>();
    }

    public bool canJump = true;
    public void OnJump(InputAction.CallbackContext ctx)
    {
        float pressed = ctx.ReadValue<float>();
        if (pressed >= 0.5f && canJump)
        {
            pRigidBody.AddForce(new Vector3(0.0f, jumpForceScalar, 0.0f));
            canJump = false;
        }
    }

    bool isShooting = false;
    public void OnShoot(InputAction.CallbackContext ctx)
    {
        float pressed = ctx.ReadValue<float>();
        isShooting = pressed >= 0.5f;
        Debug.Log(isShooting);
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        mouseVec = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (isShooting)
            currentActiveWeapon.Shoot();

        pRigidBody.gameObject.transform.RotateAround(pRigidBody.gameObject.transform.position, Vector3.up, mouseVec.x * yaw);
        lookAt.transform.position += new Vector3(0.0f, mouseVec.y * pitch, 0.0f) * Time.deltaTime;

        if (lookAt.transform.localPosition.y <= 0.0f)
            lookAt.transform.localPosition = new Vector3(lookAt.transform.localPosition.x, 0.0f, lookAt.transform.localPosition.z);
        else if (lookAt.transform.localPosition.y >= 10.0f)
            lookAt.transform.localPosition = new Vector3(lookAt.transform.localPosition.x, 10.0f, lookAt.transform.localPosition.z);

        pCamera.transform.LookAt(lookAt.transform);
        //pCamera.transform.RotateAround(pCamera.transform.position,pCamera.transform.up,mouseVec.x / rotationSpeed);
        //pCamera.transform.RotateAround(pCamera.transform.position,pCamera.transform.right,mouseVec.y / rotationSpeed);
    }

    private void FixedUpdate()
    {
        var force = pCamera.transform.forward * moveVec.y;
        force += pCamera.transform.right * moveVec.x;
        force.y = 0.0f;
        force = force.normalized * moveSpeed;
        pRigidBody.AddForce(force);
        var xzVel = pRigidBody.velocity;
        xzVel.y = 0.0f;
        if (xzVel.magnitude > 0.0f)
        {
            pRigidBody.velocity -= (xzVel / movementFalloffRate);
        }
    }
}
