using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DestructableTerrain")
        {
            collision.gameObject.GetComponent<TerrainDestroyer>().DestroyTerrain(transform.position);
            
            Debug.Log("drill tile : " + transform.position);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            try
            {
                collision.gameObject.GetComponent<EnemyMortality>().Instakill();
            }
            catch (MissingReferenceException error)
            {
                Debug.Log(error.Message);
            }
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "DestructableTerrain")
    //    {
    //        Vector3 newpos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
    //        collision.gameObject.GetComponent<TerrainDestroyer>().DestroyTerrain(newpos);
    //        Debug.Log("drill tile : " + transform.position);
    //    }
    //}

}
