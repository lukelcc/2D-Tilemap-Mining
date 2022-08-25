using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMortality : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int strartingHp = 10;

    [Header("Injured settings")]
    [SerializeField] private float invulnerablePeriod = 3f;
    [SerializeField] private float controlDisablePeriod = 1f;
    [SerializeField] private float SpriteFlashPeriod = 0.2f;
    [SerializeField] private Vector2 knockBackForce = new Vector2(5f, 5f);

    //to do - death
    [Header("Death and dismemberment")]
    [SerializeField] List<GameObject> bodyPartsList;
    [SerializeField] float yeetForce = 20f;
    [SerializeField] float rotatingSpeed = 2f;

    private int hp;
    public event Action onHpChange;

    private void Start()
    {
        hp = strartingHp;
    }
    public int GetStartingHealth()
    {
        return strartingHp;
    }

    public int getHp()
    {
        return hp;
    }

    private void OnCollisionEnter2D(Collision2D collision) // when player get hurt by enemies/hazards
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Hazard")
        {
            //player get hit at left or right
            if (collision.gameObject.transform.position.x < transform.position.x)
            {
                Debug.Log("fly right");
                InjuredAnim(knockBackForce);
            }
            if (collision.gameObject.transform.position.x > transform.position.x)
            {
                Debug.Log("fly left");
                InjuredAnim(new Vector2(-knockBackForce.x,knockBackForce.y));
            }

            //player turns red
            StartCoroutine(TemporaryDisablePlayerMovement());
            StartCoroutine(TemporaryInvulnerable());
            
            MinusHp(1);
        }
    }

    public void MinusHp(int HpToMinus)
    {    
        hp -= HpToMinus;
        Debug.Log("hp:" + hp);
        //update UI with latest hp
        if (onHpChange != null)
        {
            onHpChange();
        }

        //die
        if (hp <= 0)
        {
            Die();
            FindObjectOfType<GameSession>().ResetGame();
        }
        
    }

    public void InjuredAnim(Vector2 knockBackDirection) 
    {
        GetComponent<Rigidbody2D>().velocity = knockBackDirection;
        StartCoroutine(TemporaryDisablePlayerMovement());
    }


    private IEnumerator TemporaryDisablePlayerMovement()
    {
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(controlDisablePeriod);
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<SpriteRenderer>().color = Color.white;

        StartCoroutine(FlashingSprite());
    }

    private IEnumerator TemporaryInvulnerable()
    {
        Physics2D.IgnoreLayerCollision(8, 11, true);
        yield return new WaitForSeconds(invulnerablePeriod);
        Physics2D.IgnoreLayerCollision(8, 11, false);       
    }

    private IEnumerator FlashingSprite()
    {
        for (float i = 0f; i < (invulnerablePeriod - controlDisablePeriod); i+=(SpriteFlashPeriod*2))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(SpriteFlashPeriod);
            GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(SpriteFlashPeriod);
        }
    }

    public void Die()
    {
        Dismemberment();
    }

    private void Dismemberment()
    {
        Destroy(gameObject);

        Transform firePoint = GetComponent<Transform>().transform;


        for (int bodyPartsIndex = 0; bodyPartsIndex < bodyPartsList.Count; bodyPartsIndex++)
        {
            GameObject flyingBodyParts = Instantiate(bodyPartsList[bodyPartsIndex], firePoint.position, firePoint.rotation);
            Rigidbody2D rb = flyingBodyParts.GetComponent<Rigidbody2D>();

            Vector2 yeetDirection = new Vector2(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(5, 10));
            rb.AddForce(yeetDirection * yeetForce, ForceMode2D.Impulse);
            rb.AddTorque(rotatingSpeed, ForceMode2D.Impulse);
        }
    }
}
