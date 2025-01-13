using System;
using System.Collections;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;

    [SerializeField] LayerMask ignoreMask;

    [SerializeField] int speed;

    [SerializeField] int sprintMod;

    [SerializeField] int jumpMax;

    [SerializeField] int jumpSpeed;

    [SerializeField] int gravity;

    [SerializeField] int shootDamage;

    [SerializeField] int shootDistance;

    [SerializeField] int HP;

    Vector3 moveDir;

    Vector3 playerVelocity;

    bool isSprinting;

    int jumpCount;

    int HPOriginal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOriginal = HP;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);

        movement();
        sprint();
    }

    void movement()
    {
        //moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //transform.position += moveDir * speed * Time.deltaTime;

        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVelocity = Vector3.zero;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) +
                    (Input.GetAxis("Vertical") * transform.forward);

        controller.Move(moveDir * speed * Time.deltaTime);

        jump();

        controller.Move(playerVelocity * Time.deltaTime);
        playerVelocity.y -= gravity * Time.deltaTime;

        if (Input.GetButtonDown("Shoot"))
        {
            shoot();
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVelocity.y = jumpSpeed;
        }
    }

    void shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance, ~ignoreMask))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        updatePlayerUI();

        StartCoroutine(flashDamagePanel());

        if (HP <= 0)
        {
            GameManager.instance.youLose();
        }
    }

    IEnumerator flashDamagePanel()
    {
        GameManager.instance.damagePanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.damagePanel.SetActive(false);
    }

    void updatePlayerUI()
    {
        GameManager.instance.PlayerHPBar.fillAmount = (float)HP / HPOriginal;
    }
}
