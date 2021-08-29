using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D body2D;
    string bullet;
    public int damage;

    bool freezeBullet;

    RigidbodyConstraints2D rb2dConstraints;

    [SerializeField] Vector2 bulletDirection;
    [SerializeField] float bulletSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        body2D = GetComponent<Rigidbody2D>();

    }

    void Start()
    {
        damage = 1;
        Invoke("DestroySelf", .5f);
    }
    // Update is called once per frame
    void Update()
    {
        if (freezeBullet) return;
    }

    public void setBulletDirection(Vector2 direction)
    {
        this.bulletDirection = direction;
    }

    public void setBulletSpeed(float speed)
    {
        this.bulletSpeed = speed;
    }

    public void shoot()
    {
        body2D.velocity = bulletDirection * bulletSpeed;
    }

    public void FreezeBullet(bool freeze)
    {
        if (freeze)
        {
            freezeBullet = true;
            rb2dConstraints = body2D.constraints;
            body2D.constraints = RigidbodyConstraints2D.FreezeAll;
            body2D.velocity = Vector2.zero;
        }
        else
        {
            freezeBullet = false;
            body2D.constraints = rb2dConstraints;
            body2D.velocity = bulletDirection * bulletSpeed;
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(this.damage);
            }
            DestroySelf();
        }
    }

}
