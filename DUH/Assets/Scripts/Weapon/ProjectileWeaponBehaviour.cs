using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    protected Vector3 direction;
    public float destroyAfterSeconds;

    //Current weapon data
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPirece;

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CoolDownDuration;
        currentPirece = weaponData.Pierce;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;

        float dirx = direction.x;
        float diry = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;
        //left
        if (dirx < 0 && diry == 0)
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        }
        //down
        else if (dirx == 0 && diry < 0)
        {
            scale.y = scale.x * -1;
        }
        //up
        else if (dirx == 0 && diry > 0)
        {
            scale.x = scale.x * -1;
        }
        // right up 
        else if (dirx > 0 && diry > 0)
        {
            rotation.z = 0f;
        }
        //right up
        else if (dir.x < 0 && dir.y > 0)
        {
            rotation.z = 180f;
        }
        else if (dir.x < 0 && dir.y > 0)
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = -90f;
        }
        else if (dir.x < 0 && dir.y < 0)
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = 0f;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);
            ReducePierce();
        }
        else if(collision.CompareTag("Prop"))
        {
            if(collision.TryGetComponent<BreakableProps>(out BreakableProps breakableProps))
            {
                breakableProps.TakeDamage(currentDamage);
                ReducePierce();
            }
        }
    }
    void ReducePierce()
    {
        currentPirece--;
        if (currentPirece <= 0)
        {
            Destroy(gameObject);
        }
    }
}
