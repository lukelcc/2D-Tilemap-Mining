using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Drill : MonoBehaviour
{

    [SerializeField] float drillRange = .5f;
    [SerializeField] float drillDuration = 1f;
    [SerializeField] float drillUpOffsetRange = .2f;//need extra range to reach upper tiles
    //[SerializeField] Vector2 drillCentre = new Vector2(.06f, 0);

    private Vector2 moveInput;
    private Vector3 drillPos;

    private void Update()
    {
        MoveDrillToBackCentre();
        MoveDrillPostion();
    }

    private void Start()
    {
        drillPos = transform.position;
    }

    //new input system
    //void OnMove(InputValue value) // getting WSAD key input from user
    //{
    //    moveInput = value.Get<Vector2>();
    //    if (moveInput.x < 0 && moveInput.y == 0)
    //    {
    //        GetComponent<CircleCollider2D>().offset = new Vector2(-0.5f, 0f);
    //        Debug.Log("collider left");
    //    }
    //    else if (moveInput.x > 0 && moveInput.y == 0)
    //    {
    //        GetComponent<CircleCollider2D>().offset = new Vector2(0.5f, 0f);
    //        Debug.Log("collider right");
    //    }
    //    //DrillTile();
    //}

    private void MoveDrillToBackCentre()
    {
        if (GetComponentInParent<Rigidbody2D>().velocity == Vector2.zero && GetComponentInParent<PlayerMovement>().getMoveInput()==Vector2.zero)
        {
            GetComponent<CircleCollider2D>().offset = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DestructableTerrain")
        {
            Debug.Log("Drilling");
            StartCoroutine(DrillingTakesTime());
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyMortality>().Die();
        }
    }

    //DrillingProcess
    private IEnumerator DrillingTakesTime()
    {
        //record drill position when hit tile
        //Vector3 drillPointAtStart = drillPos;
        //Debug.Log(drillPointAtStart);

        //drill anims
        GetComponentInParent<Animator>().SetBool("IsDrilling", true);

        //drilling time
        yield return new WaitForSeconds(drillDuration);
        
        //Destroy the tile after the drill duration
        DrillTile(drillPos);

        GetComponentInParent<Animator>().SetBool("IsDrilling", false);

    }

    private void DrillAnims()
    {
        if (GetComponent<CircleCollider2D>().OverlapPoint(drillPos))
            GetComponentInParent<Animator>().SetBool("IsDrilling", true);
    }

    public void MoveDrillPostion()
    {
        moveInput = GetComponentInParent<PlayerMovement>().getMoveInput();
        //drill left
        if (moveInput.x < 0 && moveInput.y == 0)
        {
            GetComponent<CircleCollider2D>().offset = new Vector2(-drillRange, 0);
            drillPos = new Vector3(transform.position.x - drillRange, transform.position.y, transform.position.z);
            Debug.Log("Drill left");
            //Debug.Log("sprite pos:" + transform.position);
            //Debug.Log("drill pos:" + drillPos);
        }
        //drill right
        else if (moveInput.x > 0 && moveInput.y == 0)
        {
            GetComponent<CircleCollider2D>().offset = new Vector2(drillRange, 0);
            drillPos = new Vector3(transform.position.x + drillRange, transform.position.y, transform.position.z);
            Debug.Log("Drill right");
            //Debug.Log("sprite pos:" + transform.position);
            //Debug.Log("drill pos:" + drillPos);
        }
        //drill up
        else if (moveInput.x == 0 && moveInput.y > 0)
        {
            GetComponent<CircleCollider2D>().offset = new Vector2(0, drillRange);
            drillPos = new Vector3(transform.position.x, transform.position.y + drillRange + drillUpOffsetRange, transform.position.z);
            Debug.Log("Drill up");
            //Debug.Log("sprite pos:" + transform.position);
            //Debug.Log("drill pos:" + drillPos);
        }
        //drill down
        else if (moveInput.x == 0 && moveInput.y < 0)
        {
            GetComponent<CircleCollider2D>().offset = new Vector2(0, -drillRange);
            drillPos = new Vector3(transform.position.x, transform.position.y - drillRange, transform.position.z);
            Debug.Log("Drill down");
            //Debug.Log("sprite pos:" + transform.position);
            //Debug.Log("drill pos:" + drillPos);
        }



        //DrillTile();
        //Vector2 point = new Vector2(drillPos.x, drillPos.y);
        //Debug.Log(GetComponent<CircleCollider2D>().OverlapPoint(point));
    }
    public void DrillTile(Vector3 tileToDrillPos)
    {
        FindObjectOfType<TerrainDestroyer>().DestroyTerrain(tileToDrillPos);
    }

    //private void DrillTile()
    //{
    //    if (moveInput.x < 0 && moveInput.y == 0) //drill left
    //    {          
    //        Vector3 drillPos = new Vector3(transform.position.x - drillRadius, transform.position.y, transform.position.z);
    //        //Debug.Log("sprite pos:" + transform.position);
    //        //Debug.Log("drill pos:" + drillPos);
    //        FindObjectOfType<TerrainDestroyer>().DestroyTerrain(drillPos);
    //    }
    //    if (moveInput.x > 0 && moveInput.y == 0) //drill right
    //    {
    //        Vector3 drillPos = new Vector3(transform.position.x + drillRadius, transform.position.y, transform.position.z);
    //        //Debug.Log("sprite pos:" + transform.position);
    //        //Debug.Log("drill pos:" + drillPos);
    //        FindObjectOfType<TerrainDestroyer>().DestroyTerrain(drillPos);
    //    }
    //}

    //drill cooldown?

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "DestructableTerrain")
    //    {
    //        collision.gameObject.GetComponent<TerrainDestroyer>().DestroyTerrain(transform.position);
            
    //        Debug.Log("drill tile : " + transform.position);
    //    }

    //    //if (collision.gameObject.tag == "Enemy")
    //    //{
    //    //    try
    //    //    {
    //    //        collision.gameObject.GetComponent<EnemyMortality>().Instakill();
    //    //    }
    //    //    catch (MissingReferenceException error)
    //    //    {
    //    //        Debug.Log(error.Message);
    //    //    }
    //    //}
    //}


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
