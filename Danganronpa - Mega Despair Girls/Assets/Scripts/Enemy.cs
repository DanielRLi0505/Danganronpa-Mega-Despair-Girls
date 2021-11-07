using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]


public class Enemy : MonoBehaviour
{

    public Animator anim;
    public BoxCollider2D box;
    public Rigidbody2D body2D;
    public RigidbodyConstraints2D body2Dconstraints;
    public string currentState;

    public SpriteRenderer sprite;

    bool isInvincible;
    public bool freezeEnemy;
    public int currentHealth;
    public int maxHealth;
    public int contactDamage;
    public int scorePoints;
    public int speed;

    GameObject explodeEffect;
    [SerializeField] GameObject explodeEffectPrefab;
    [SerializeField] AudioClip enemyDamageClip;
    [SerializeField] AudioClip enemyBlockClip;

    private void Awake()
    {
        Messenger.AddListener<bool>("Freeze", Freeze);
    }

    // Start is called before the first frame update
    public void Start()
    {
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        body2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        Messenger.AddListener<bool>("Freeze", Freeze);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Invincible(bool invincibility)
    {
        isInvincible = invincibility;
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            Mathf.Clamp(currentHealth, 0, maxHealth);
            SoundManager.Instance.Play(enemyDamageClip);
            if (currentHealth <= 0)
            {
                Defeat();
            }
        }
        else
        {
            SoundManager.Instance.Play(enemyBlockClip);
        }
    }

    public void Freeze(bool freeze)
    {
        if (freeze)
        {
            freezeEnemy = true;
            if (anim)
            {
                anim.speed = 0;
            }
            if (body2D)
            {
                body2Dconstraints = body2D.constraints;
                body2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        else
        {
            freezeEnemy = false;
            if (anim)
            {
                anim.speed = 1;
            }
            if (body2D)
            {
                body2D.constraints = body2Dconstraints;
            }
        }
    }

    public void Defeat()
    {
        StartDefeatAnimation();
        Destroy(gameObject);
        GameManager.Instance.AddScorePoints(this.scorePoints);
    }

    public void StartDefeatAnimation()
    {
        explodeEffect = Instantiate(explodeEffectPrefab);
        explodeEffect.name = explodeEffectPrefab.name;
        explodeEffect.transform.position = sprite.bounds.center;
        Destroy(explodeEffect, .2f);

    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Komaru player = other.GetComponent<Komaru>();
            player.hitSide(transform.position.x > player.transform.position.x);
            player.takeDamage(getContactDamage());
            Debug.Log("Hit");
        }
        if (other.CompareTag("Danger"))
        {
            Destroy(gameObject);
        }
    }

    protected void setContactDamage(int contact)
    {
        contactDamage = contact;
    }

    public int getContactDamage()
    {
        return contactDamage;
    }

    public void setMaxHealth(int health)
    {
        maxHealth = health;
    }

    public void setScorePoints(int points)
    {
        scorePoints = points;
    }

    public void setSpeed(int walk)
    {
        speed = walk;
    }

    public void changeState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }

        anim.Play(newState);

        currentState = newState;
    }

    public bool isVisible()
    {

        float camViewHalfHeight = Camera.main.orthographicSize;
        float camViewHalfWidth = camViewHalfHeight * Camera.main.aspect;
        float xMin = Camera.main.transform.position.x - camViewHalfWidth;
        float xMax = Camera.main.transform.position.x + camViewHalfWidth;
        float yMin = Camera.main.transform.position.y - camViewHalfHeight;
        float yMax = Camera.main.transform.position.y + camViewHalfHeight;

        if (body2D.position.x < xMin || body2D.position.x > xMax || body2D.position.y < yMin || body2D.position.y > yMax)
        {
            return false;
        }
        return true;
    }

    public void despawn()
    {
        if (!isVisible())
        {
            Destroy(gameObject);
        }
    }
}
