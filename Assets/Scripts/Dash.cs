using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    [Header("Dash settings")]
    [SerializeField] private float dashingPower = 20f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    private bool canDash = true;
   // private bool isDashing = false;

    //Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Dash
    void OnDash(InputValue value)
    {
        if (!canDash) return;
        StartCoroutine(DashMove());
    }

    private IEnumerator DashMove()
    {
        GetComponent<PlayerMovement>().enabled = false;
        //GetComponent<PlayerInput>().DeactivateInput();
        canDash = false;
        float originalGravity = GetComponent<Rigidbody2D>().gravityScale;
        GetComponent<Rigidbody2D>().gravityScale = 0f;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        //dash according to sprite directions
        if (GetComponent<SpriteRenderer>().flipX)
            GetComponent<Rigidbody2D>().velocity = new Vector2(dashingPower, 0f);
        else
            GetComponent<Rigidbody2D>().velocity = new Vector2(-dashingPower, 0f);

        yield return new WaitForSeconds(dashingTime);

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().gravityScale = originalGravity;
        GetComponent<PlayerMovement>().enabled = true;
        //GetComponent<PlayerInput>().ActivateInput();

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

}
