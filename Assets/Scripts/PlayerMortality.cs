using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMortality : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int strartingHp = 10;

    [Header("Injured settings")]
    [SerializeField] private float invulnerablePeriod = 3f;
    [SerializeField] private float controlDisablePeriod = 1f;
    [SerializeField] private float SpriteFlashPeriod = 0.2f;
    //[SerializeField] private Vector2 knockBackForce = new Vector2(5f, 5f);

    //[Header("Death and dismemberment")]
    //[SerializeField] List<GameObject> bodyPartsList;
    //[SerializeField] float yeetForce = 20f;
    //[SerializeField] float rotatingSpeed = 2f;

    private int hp=0;
    public event Action onHpChange;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 11, false);
        hp = strartingHp;
        if (onHpChange != null)
        {
            onHpChange();
        }
    }
    public int GetStartingHealth()
    {
        return strartingHp;
    }

    public int getHp()
    {
        return hp;
    }

    // when player get hurt by enemies/hazards
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Hazard")
        {
            MinusHp(1);

            //if die, dont run injured anims
            //if (hp <= 0) 
            //{               
            //    return; 
            //}

            ////player get hit at left or right
            //else if (collision.gameObject.transform.position.x < transform.position.x)
            //{
            //    GetComponent<Rigidbody2D>().velocity = knockBackForce;
            //    Debug.Log("fly right");
            //}
            //else if (collision.gameObject.transform.position.x > transform.position.x)
            //{
            //    Debug.Log("fly left");
            //    GetComponent<Rigidbody2D>().velocity = new Vector2(-knockBackForce.x, knockBackForce.y);
            //}                   
        }
    }

    public void MinusHp(int HpToMinus)
    {
        //minus hp
        hp -= HpToMinus;
        
        //update UI with latest hp
        if (onHpChange != null)
        {
            onHpChange();
        }

        if (hp <= 0) //die anims
        {
            Die();
            
            FindObjectOfType<GameSession>().ResetGame();
        }
        else // injured anims
        {
            InjuredAnims();
        }

    }


    private IEnumerator TemporaryDisablePlayerMovement()
    {
        //GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerInput>().DeactivateInput();

        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(controlDisablePeriod);

        //GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerInput>().ActivateInput();

        GetComponent<SpriteRenderer>().color = Color.white;

        StartCoroutine(FlashingSprite());
    }
   
    private IEnumerator TemporaryInvulnerable()
    {
        Physics2D.IgnoreLayerCollision(8, 11, true);
        yield return new WaitForSeconds(invulnerablePeriod);
        Physics2D.IgnoreLayerCollision(8, 11, false);       
    }

    public void InjuredAnims()
    {
        StartCoroutine(TemporaryDisablePlayerMovement());
        StartCoroutine(TemporaryInvulnerable());
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
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;//player freeze position
        Physics2D.IgnoreLayerCollision(8, 11, true);//disable collision
        GetComponent<PlayerInput>().DeactivateInput();//disable player input
        GetComponent<Animator>().SetTrigger("Dying");//set die anims
        Debug.Log("die");
    }

    //private void Dismemberment()
    //{
    //    Destroy(gameObject);

    //    Transform firePoint = GetComponent<Transform>().transform;


    //    for (int bodyPartsIndex = 0; bodyPartsIndex < bodyPartsList.Count; bodyPartsIndex++)
    //    {
    //        GameObject flyingBodyParts = Instantiate(bodyPartsList[bodyPartsIndex], firePoint.position, firePoint.rotation);
    //        Rigidbody2D rb = flyingBodyParts.GetComponent<Rigidbody2D>();

    //        Vector2 yeetDirection = new Vector2(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(5, 10));
    //        rb.AddForce(yeetDirection * yeetForce, ForceMode2D.Impulse);
    //        rb.AddTorque(rotatingSpeed, ForceMode2D.Impulse);
    //    }
    //}
}
