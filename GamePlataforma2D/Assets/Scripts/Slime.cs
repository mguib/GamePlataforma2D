using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private Rigidbody2D rig;
    private Animator anim;

    public float speed;
    public Transform point;
    public float radius;
    public LayerMask layer;
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rig.velocity = new Vector2(speed, rig.velocity.y);  //Seta uma velocidade constante de movimento ao personagem
        OnCollision();
    }

    /*Fun��o que detecta a colis�o do personagem em um gizmo pela posi��o, tamnho do raio e em qual layer est� tocando
     * Ao colidir em um objeto qocm a layer selecionada (chao) ele muda sua rota��o no eixo Y
     */
    void OnCollision()
    {
        Collider2D hit = Physics2D.OverlapCircle(point.position, radius, layer);

        if(hit != null)
        {
            //S� � chamado quando o inimigo bate em um objeto que tenha layer selecionada
            speed = -speed;
            
            if(transform.eulerAngles.y == 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            
        }
    }

    /*M�todo que causa dano no personagem
     * Esse m�todo retira 1 da vida do personagem (inimigo em quest�o) e verifica se ele esta morto.
     * Se sim, seta anima��o de morte. Se n�o, diminui 1 de vida e seta anima��o de dano
     */
    public void OnHit()
    {
        anim.SetTrigger("hit");
        health--;

        if(health <= 0)
        {
            speed = 0;
            anim.SetTrigger("dead");
            Destroy(gameObject, 1f);
        }
    }

    //Cria uma esfera/gizmo na frete do personagem para dectar colis�o, passando uma posi��o e um tamanho do raio de alcance
    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(point.position, radius);
    }
}
