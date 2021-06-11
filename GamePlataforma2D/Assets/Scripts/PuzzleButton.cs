using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    private Animator anim;

    public Animator barrierAnim;
    public LayerMask layer;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    void OnPressed()
    {
        anim.SetBool("isPressed" , true);
        barrierAnim.SetBool("isPressed", true);
    }

    void OnExit()
    {
        anim.SetBool("isPressed", false);
        barrierAnim.SetBool("isPressed", false);
    }

    ////Retorna enquanto o objeto esta em colisão com outro
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Stone"))
    //    {
    //        OnPressed();
    //    }
    //}

    private void FixedUpdate()
    {
        OnCollision();
    }

    ////Retorna quando um objeto para de colidir com outro
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Stone"))
    //    {
    //        OnExit();
    //    }
    //}

    void OnCollision()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.5f, layer);

        if(hit != null)
        {
            OnPressed();
            hit = null;
        }
        else
        {
            OnExit();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
