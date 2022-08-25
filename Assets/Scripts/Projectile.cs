using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int projectileDamage = 1;

    //[SerializeField] private GameObject impactExplosion;
    [SerializeField] private ParticleSystem bulletImpactFx;

    //pool
    private IObjectPool<Projectile> projectilePool;

    //pool
    public void SetPool(IObjectPool<Projectile> pool)
    {
        projectilePool = pool;
    }

    //pool
    //private void OnBecameInvisible()
    //{
    //    projectilePool.Release(this);
    //}

    public int getProjectileDamage()
    {
        return projectileDamage;
    }

    public void playBulletImpactFx()
    {
        if (bulletImpactFx != null)
        {
            ParticleSystem instance = Instantiate(bulletImpactFx, transform.position, Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax); // destroy after the particles fx anim
        }
    }


    private void OnCollisionEnter2D(Collision2D collision) 
    {      
        if (collision.gameObject.tag == "Enemy") //if bullet hit enemy
        {
            try
            {
                collision.gameObject.GetComponent<EnemyMortality>().MinusHp(projectileDamage); // do damage to enemy
            }
            catch (MissingReferenceException error)
            {
                Debug.Log(error.Message);
            }
        }
        if (collision.gameObject.tag == "DestructableTerrain") // if bullet hit terrain
        {
            Vector3 tilePos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z);
            collision.gameObject.GetComponent<TerrainDestroyer>().DestroyTerrain(tilePos);
            Debug.Log("impact location: "+transform.position);
        }

        projectilePool.Release(this); // return bullet to pool
        playBulletImpactFx();

        //Destroy(gameObject);       
        //Instantiate(impactExplosion, transform.position, Quaternion.identity); //instantiate bullet impact
    }

}
