using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Komaru : MonoBehaviour
{
    string truthBullet;
    public float jumpTimeCounter;
    public float chargeTimeCounter;
    private float animTimeCounter;
    public float slideTimeCounter;
    public float jumpSpeed;
    public float teleportSpeed = 100f;
    public float checkRadius = 0.5f;
    public float gravity = 100f;
    public float jumpTime;
    public float chargeTime;
    public float animTime;
    public float slideTime;
    public float chargeMod = 0.5f;
    public float climbPercentage;
    int bullets = 0;
    public bool isGrounded;
    public bool onLadder;
    public bool canShoot;
    public bool canSlide = false;
    public bool canJump = true;
    public bool isJumping;
    public bool sliding;
    public bool isDown;
    public bool isTeleporting;
    public bool isTakingDamage;
    public bool isInvincible;
    public bool hitSideRight;
    public bool freezeInput;
    public bool freezePlayer;
    bool freezeBullets;
    public int speed = 150;
    public int currentHealth;
    public int maxHealth = 28;
    private Rigidbody2D body2D;
    private SpriteRenderer renderer2D;
    private Animator animator;
    public LayerMask whatIsGround;
    public LayerMask whatIsLadder;
    public LayerMask whatIsRising;
    public Vector2 velocity;
    public Health bar;
    public string currentState;

    RigidbodyConstraints2D body2Dconstraints;

    public groundCheck grounded;

    GameObject explodeEffect;

    [SerializeField] Object bulletRef;
    [SerializeField] Object weakRef;
    [SerializeField] Object chargeRef;
    [SerializeField] Transform GroundCheck;
    [SerializeField] Transform UndergroundCheck;
    [SerializeField] enum teleportState { Idle, Landed, Descending };
    [SerializeField] teleportState TeleportState;

    [SerializeField] AudioClip jumpLandedClip;
    [SerializeField] AudioClip shootBulletClip;
    [SerializeField] AudioClip takingDamageClip;
    [SerializeField] AudioClip explodeEffectClip;
    [SerializeField] AudioClip chargingClip;
    [SerializeField] AudioClip chargedShotClip;
    [SerializeField] AudioClip teleportClip;

    [SerializeField] GameObject explodeEffectPrefab;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Messenger.AddListener<bool>("Freeze", Freeze);
    }
    // Start is called before the first frame update
    void Start()
    {
        truthBullet = "Break";
        body2D = GetComponent<Rigidbody2D>();
        renderer2D = GetComponent<SpriteRenderer>();
        bar = GetComponent<Health>();
        bulletRef = Resources.Load("Bullets/" + truthBullet);
        weakRef = Resources.Load("Bullets/weakChargeBullet");
        chargeRef = Resources.Load("Bullets/chargedBullet");
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        //isTeleporting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.CompareTag("Player"))
        {
            //isGrounded = grounded.getGround();
            isGrounded = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, whatIsGround) ||
                Physics2D.OverlapCircle(GroundCheck.position, checkRadius, whatIsRising);
            if (!freezeInput)
            {
                movement();
                jump();
                airborne();
                if (!canShoot && Input.GetButton("Fire1"))
                {
                    StartCoroutine(FireShot());
                }
                rise();
                slide();
                chargeShot();
                if (isTakingDamage)
                {
                    //animator.SetInteger(truthBullet, 26);
                    changeState("hit");
                    animator.speed = 1;
                    return;
                }
            }
            
            if (isTeleporting)
            {
                switch(TeleportState)
                {
                    case teleportState.Descending:
                        isJumping = false;
                        animator.speed = 0;
                        if (isGrounded)
                        {
                            TeleportState = teleportState.Landed;
                        }
                        break;
                    case teleportState.Landed:
                        animator.speed = 1;
                        break;
                    case teleportState.Idle:
                        teleport(false);
                        break;
                }
            }
            if(isGrounded && isJumping && body2D.velocity.y < 0.5)
            {
                SoundManager.Instance.EffectsSource.PlayOneShot(jumpLandedClip);
                isJumping = false;
            }
            //body2D.gravityScale = gravity;
        }
    }

    public void changeState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }

        currentState = newState;

        animator.Play(newState);
    }

    //Sets which truth bullet Komaru is shooting;
    public void setBullet(string truth)
    {
        truthBullet = truth;
    }

    public void teleport(bool state)
    {
        if (state)
        {
            isTeleporting = true;
            FreezeInput(true);
            //animator.SetInteger(truthBullet, 27);
            changeState("teleport-01");
            TeleportState = teleportState.Descending;
            body2D.velocity = new Vector2(0, -teleportSpeed);
        }
        else
        {
            isTeleporting = false;
            FreezeInput(false);
        }
    }

    public void TeleportAnimationEnd()
    {
        TeleportState = teleportState.Idle;
    }

    public void TeleportAnimationSound()
    {
        SoundManager.Instance.Play(teleportClip);
    }

    //Gets truth bullet selected
    public string getBullet()
    {
        return truthBullet;
    }

    //Gets direction Komaru is facing
    public int getFlip()
    {
        if (renderer2D.flipX)
        {
            return -1;
        }
        return 1;
    }

    void jump()
    {
        StartCoroutine(checkJump());
        if (canJump)
        {
            if (Input.GetButton("Jump") && isGrounded)
            {
                isJumping = true;
                jumpTimeCounter = jumpTime;
                body2D.velocity = new Vector2(body2D.velocity.x, jumpSpeed);
            }

            if (Input.GetButton("Jump") && isJumping)
            {
                if (jumpTimeCounter > 0)
                {
                    body2D.velocity = new Vector2(body2D.velocity.x, jumpSpeed);
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }

            if (Input.GetButtonUp("Jump"))
            {
                isJumping = false;
            }
            if (!isGrounded)
            {
                isJumping = true;
            }
        }
        
    }

    void movement()
    {
        if (!isTakingDamage)
        {
            if (Input.GetButton("Horizontal") && slideTimeCounter <= 0)
            {
                if (Input.GetAxis("Horizontal") > 0)
                {
                    renderer2D.flipX = false;
                    body2D.velocity = new Vector2(speed, body2D.velocity.y);
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    body2D.velocity = new Vector2(speed * -1, body2D.velocity.y);
                    renderer2D.flipX = true;
                }
                if (isGrounded)
                {
                    if (!Input.GetButton("Fire2"))
                    {
                        if (!Input.GetButton("Fire1"))
                        {
                            //animator.SetInteger(truthBullet, 2);
                            changeState("run");
                        }
                        else
                        {
                            //animator.SetInteger(truthBullet, 5);
                            changeState("shootrun");
                        }
                    }
                    
                }
            }
            else if (Input.GetButton("Fire1"))
            {
                if (isGrounded)
                {
                    changeState("shoot");
                }
                //animator.SetInteger(truthBullet, 4);


            }
            if (isGrounded && !Input.anyKey && animTimeCounter <= 0 && slideTimeCounter <= 0 && isTeleporting == false)
            {
                changeState("idle");
            }
        }
        
    }

    void airborne()
    {
        if (!isTakingDamage)
        {
            if (!isGrounded && !onLadder)
            {
                if (!Input.GetButton("Fire1"))
                {
                    //animator.SetInteger(truthBullet, 3);
                    changeState("jump");

                }
                else
                {
                    //animator.SetInteger(truthBullet, 6);
                    changeState("shootjump");
                }

            }
        }
    }

    void rise()
    {
        if (Physics2D.OverlapCircle(GroundCheck.position, checkRadius, whatIsRising) != null)
        {
            body2D.mass = 0;
            body2D.gravityScale = 0;
        }
        else
        {
            body2D.mass = 0.04469959f;
            body2D.gravityScale = gravity;
        }
    }

    void shoot(string TB)
    {
        GameObject bullet;
        switch(TB)
        {
            case "Weak":
                bullet = (GameObject)Instantiate(weakRef);
                SoundManager.Instance.Play(chargedShotClip);
                bullet.transform.position = new Vector3(transform.position.x + ((float)getFlip() * 20f), transform.position.y + 5f, 1);
                bullet.GetComponent<weakChargeBullet>().setBulletDirection((!this.renderer2D.flipX) ? Vector2.right : Vector2.left);
                bullet.GetComponent<weakChargeBullet>().setBulletSpeed(270f);
                bullet.GetComponent<weakChargeBullet>().shoot();
                break;
            case "Charged":
                bullet = (GameObject)Instantiate(chargeRef);
                //SoundManager.Instance.Play(chargedShotClip);
                bullet.transform.position = new Vector3(transform.position.x + ((float)getFlip() * 20f), transform.position.y + 5f, 1);
                bullet.GetComponent<chargedBullet>().setBulletDirection((!this.renderer2D.flipX) ? Vector2.right : Vector2.left);
                bullet.GetComponent<chargedBullet>().setBulletSpeed(270f);
                bullet.GetComponent<chargedBullet>().shoot();
                break;
            default:
                bullet = (GameObject)Instantiate(bulletRef);
                bullet.transform.position = new Vector3(transform.position.x + ((float)getFlip() * 20f), transform.position.y + 5f, 1);
                bullet.GetComponent<Bullet>().setBulletDirection((!this.renderer2D.flipX) ? Vector2.right : Vector2.left);
                bullet.GetComponent<Bullet>().setBulletSpeed(270f);
                bullet.GetComponent<Bullet>().shoot();
                SoundManager.Instance.Play(shootBulletClip);
                break;
        }
    }

    void chargeShot()
    {
        bool weak = false;
        bool charging = false;
        if (Input.GetButtonDown("Fire2"))
        {
            chargeTimeCounter = chargeTime;
            animTimeCounter = animTime;
            shoot(truthBullet);
        }
        if (Input.GetButton("Fire2"))
        {
            chargeTimeCounter -= Time.deltaTime;
        }
        if (chargeTimeCounter < chargeTime && chargeTimeCounter > chargeTime * chargeMod && Input.GetButtonUp("Fire2"))
        {
            animTimeCounter = animTime;
            chargeTimeCounter = chargeTime;
            shoot(truthBullet);
        }
        if (chargeTimeCounter < (chargeTime * chargeMod) && chargeTimeCounter > 0 && Input.GetButtonUp("Fire2"))
        {
            animTimeCounter = animTime;
            shoot("Weak");
            chargeTimeCounter = chargeTime;
        }
        if (chargeTimeCounter < 0 && Input.GetButtonUp("Fire2"))
        {
            animTimeCounter = animTime;
            shoot("Charged");
            chargeTimeCounter = chargeTime;
            charging = true;
        }
        if (chargeTimeCounter < (chargeTime * chargeMod) && chargeTimeCounter > 0)
        {
            weak = true;
        }
        if (weak)
        {
            if (!SoundManager.Instance.EffectsSource.isPlaying)
            {
                SoundManager.Instance.Play(chargingClip);
            }
            else
            {
                if (!SoundManager.Instance.EffectsSource.clip == chargingClip)
                {
                    SoundManager.Instance.Play(chargingClip);
                }
                if (Input.GetButtonUp("Fire2"))
                {
                    SoundManager.Instance.Play(chargedShotClip);
                }
            }
            
            if (Input.GetButton("Horizontal") && isGrounded)
            {
                //animator.SetInteger(truthBullet, 14);
                changeState("weakrun");
                

            }
            else if (!isGrounded && !onLadder)
            {
                //animator.SetInteger(truthBullet, 16);
                changeState("weakjump");
            }
            else if (canSlide)
            {
                //animator.SetInteger(truthBullet, 18);
                changeState("weakslide");
            }
            else if (onLadder)
            {
                changeState("weakclimb");
                if (body2D.velocity.y != 0)
                {
                    //animator.SetInteger(truthBullet, 20);
                    animator.speed = 1;
                    
                }
                else
                {
                    //animator.SetInteger(truthBullet, 21);
                    //changeState("weakclimbidle");
                    animator.speed = 0;
                }
            }
            else
            {
                //animator.SetInteger(truthBullet, 13);
                changeState("weakcharge");
            }
        }
        if (chargeTimeCounter < 0)
        {
            charging = true;
        }
        if (charging)
        {
            if (Input.GetButtonUp("Fire2"))
            {
                SoundManager.Instance.Play(chargedShotClip);
            }
            if (Input.GetButton("Horizontal") && isGrounded)
            {
                //animator.SetInteger(truthBullet, 15);
                changeState("chargerun");
            }
            else if (!isGrounded && !onLadder)
            {

                //animator.SetInteger(truthBullet, 17);
                changeState("chargejump");
            }
            else if (canSlide)
            {
                //animator.SetInteger(truthBullet, 19);
                changeState("chargeslide");
            }
            else if (onLadder)
            {
                changeState("chargeclimb");
                if (body2D.velocity.y != 0)
                {
                    animator.speed = 1;
                    //animator.SetInteger(truthBullet, 22);
                }
                else
                {
                    animator.speed = 0;
                    //animator.SetInteger(truthBullet, 23);
                }
            }
            else
            {
                //animator.SetInteger(truthBullet, 12);
                changeState("chargeidle");
            }
        }
        if (Input.GetButtonUp("Fire2"))
        {
            animTimeCounter = animTime;
            chargeTimeCounter = chargeTime;
        }
        if (animTimeCounter > 0)
        {
            if (!isGrounded && !onLadder)
            {
                //animator.SetInteger(truthBullet, 6);
                changeState("shootjump");
            }
            else if (Input.GetAxis("Horizontal") != 0 && isGrounded)
            {
                //animator.SetInteger(truthBullet, 5);
                changeState("shootrun");
            }
            else if (onLadder)
            {
                //animator.SetInteger(truthBullet, 10);
                changeState("climbshoot");
            }
            else
            {
                //animator.SetInteger(truthBullet, 4);
                changeState("shoot");
            }
            animTimeCounter -= Time.deltaTime;
        }
    }

    IEnumerator FireShot()
    {
        if (!canSlide)
        {
            canShoot = true;
            shoot(truthBullet);
            bullets += 1;
            if (bullets == 3)
            {
                yield return new WaitForSeconds(0.2f);
                bullets = 0;
                canShoot = false;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                canShoot = false;
            }
            if (Input.GetButtonUp("Fire1"))
            {
                bullets = 0;
            }
        }
        


    }

    void slide()
    {
        slideCheck();
        if (canSlide)
        {
            if (Input.GetButton("slide") || Physics2D.OverlapCircle(UndergroundCheck.position, checkRadius, whatIsGround))
            {
                //animator.SetInteger(truthBullet, 11);
                animator.speed = 1;
                if (slideTimeCounter > 0)
                {
                    if (!Input.GetButton("Fire2"))
                    {
                        changeState("slide");
                    }

                }
                body2D.velocity = new Vector2(200 * getFlip(), body2D.velocity.y);
            }

        }
        /*else if (Input.GetButton("slide") && !canSlide)
        {
            changeState("idle");
        }*/
        if (onLadder)
        {
            body2D.gravityScale = 0;
        }
        else
        {
            gravity = 100;
        }
    }
    
    void slideCheck()
    {
        if (Input.GetButtonDown("slide"))
        {
            slideTimeCounter = slideTime;
        }
        if (Input.GetButton("slide"))
        {
            if (canSlide)
            {
                slideTimeCounter -= Time.deltaTime;
            }
            
        }
        if (Input.GetButtonUp("slide"))
        {
            slideTimeCounter = 0;
        }
        if (slideTimeCounter > 0 && isGrounded)
        {
            canSlide = true;
        }
        else
        {
            if (checkUnder())
            {
                canSlide = true;
            }
            else
            {
                canSlide = false;
            }
        }

    }

    IEnumerator checkJump()
    {
        if (true)
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (canJump && isGrounded)
                {
                    isJumping = false;

                }
                if (!canJump)
                {
                    isJumping = true;
                    yield return new WaitForSeconds(.3f);
                    canJump = true;
                }

            }
        }
    }

    bool checkUnder()
    {
        if (!Physics2D.OverlapCircle(UndergroundCheck.position, checkRadius, whatIsGround) && isGrounded)
        {
            return false;
        }
        else if (isGrounded)
        {
            return true;
        }
        return false;
    }

    public void hitSide(bool rightSide)
    {
        hitSideRight = rightSide;
    }

    public void Invincible(bool invincibility)
    {
        isInvincible = invincibility;
    }

    public void takeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            Health.instance.setValue(currentHealth / (float)maxHealth);
            if (currentHealth <= 0)
            {
                Defeat();
            }
            else
            {
                StartDamageAnimation();
            }
            if (!freezePlayer)
            {
                SoundManager.Instance.Play(takingDamageClip);
            }
            else
            {
                if (SoundManager.Instance.EffectsSource.clip != takingDamageClip)
                {
                    SoundManager.Instance.Play(takingDamageClip);
                }
            }
        }
    }

    public void StartDamageAnimation()
    {
        if (!isTakingDamage)
        {
            isTakingDamage = true;
            Invincible(true);
            animator.speed = 1;
            float hitForceX = 7f;
            float hitForceY = 5f;
            if (hitSideRight) hitForceX = -hitForceX;
            body2D.velocity = Vector2.zero;
            body2D.AddForce(new Vector2(hitForceX, hitForceY), ForceMode2D.Impulse);
        }
    }

    public void StopDamageAnimation()
    {
        isTakingDamage = false;
        isInvincible = false;
    }

    void StartDefeatAnimation()
    {
        explodeEffect = Instantiate(explodeEffectPrefab);
        explodeEffect.name = explodeEffectPrefab.name;
        explodeEffect.transform.position = renderer2D.bounds.center;
        Destroy(explodeEffect, 2f);
        //gameObject.SetActive(false);
        SoundManager.Instance.Play(explodeEffectClip);
    }

    public void Defeat()
    {
        GameManager.Instance.PlayerDefeated();
        Invoke("StartDefeatAnimation", 0.5f);
        Freeze(true);
        gameObject.SetActive(false);
    }

    public void FreezeInput(bool freeze)
    {
        freezeInput = freeze;
    }

    public void Freeze(bool freeze)
    {
        freezePlayer = freeze;
        if (freeze)
        {
            body2Dconstraints = body2D.constraints;
            animator.speed = 0;
            body2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            body2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            animator.speed = 1;
        }
    }


}
