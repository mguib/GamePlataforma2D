using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerAudio playerAudio;
    private Rigidbody2D rig;

    private Health health;
    private bool isJumping;
    private bool doubleJump;
    private bool isAttacking;
    private bool recovery;

    public float speed;
    public LayerMask enemyLayer;
    public float jumpForce;
    public Animator anim;
    public Transform point;
    public float radius;
    

    private static Player instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<PlayerAudio>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Attack();
    }

    private void FixedUpdate()
    {
        Move();
    }

    /*Função que controla os movimentos do player
     * Seta a velocidade
     * Seta um padrão de moviemnto do GetAxis
     * Verifica para onde o player esta se movendo ou pulando e seta suas animações
     */
    void Move()
    {
        //Se não pressionar nada, retorna 0. Se precionar direita retorna 1. Se precionar esquerda -1
        float movement = Input.GetAxis("Horizontal");

        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if(movement > 0)
        {
            if (!isJumping && !isAttacking)
            {
                anim.SetInteger("transition", 1);
            }
            
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if(movement < 0)
        {
            if (!isJumping && !isAttacking)
            {
                anim.SetInteger("transition", 1);
            }

            //anim.SetInteger("transition", 1);
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if(movement == 0 && !isJumping && !isAttacking)
        {
            anim.SetInteger("transition", 0);
        }
    }

    /*Função de pulo e pulo duplo
     * Função que realiza que pulo e pulo duplo setando as duas animações
     */
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                playerAudio.PlaySFX(playerAudio.jumpSoud);
                anim.SetInteger("transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;
                doubleJump = true;
                //Debug.Log("Pulando");

            }else if (doubleJump)
            {
                playerAudio.PlaySFX(playerAudio.jumpSoud);
                anim.SetInteger("transition", 3);
                rig.AddForce(Vector2.up * (jumpForce -1f), ForceMode2D.Impulse);
                doubleJump = false;
            }
            
        }
    }

    /*Função de atack no player
     * Função que usa a função padrão de atack do GetAxis criando uma área de alcance para o atack
     * e verifica em qual inimigo esta atingindo com a função Collider2D hit.
     * A função utliza dos métodos das classes dos inimigos para dar dano em cada um separadamente 
     */
    void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isAttacking = true; //Deixa o atack ativo
            anim.SetInteger("transition", 4);
            
            Collider2D hit = Physics2D.OverlapCircle(point.position, radius, enemyLayer);   //Armazena informação de quem está sendo tocado

            playerAudio.PlaySFX(playerAudio.hitSound);

            if (hit != null)
            {
                if (hit.GetComponent<Slime>())
                {
                    hit.GetComponent<Slime>().OnHit();  //Utiliza o método da classe Slime para dar dano ao personagem
                }

                if (hit.GetComponent<Goblim>())
                {
                    hit.GetComponent<Goblim>().OnHit(); //Utiliza o método da classe Goblim para dar dano ao personagem
                }
            }

            StartCoroutine(OnAttack());
        }
        
    }

    /*Essa função conta um tempo para realizar a animação de atack e finalizar ela com um tempo
     * determinado.
     */
    IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(0.33f);
        isAttacking = false;
    }

    //Cria o gizmos de atack do personagem em forma de esfera
    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(point.position, radius);   
    }

    //Detecta colisão entere os objetos, neste caso com o chão permitindo o player pular
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            isJumping = false;
            //Debug.Log("Ta no chão");
        }

        if(collision.gameObject.layer == 9)
        {
            PlayerPos.instace.CheckPoint();
        }
    }

    //Função utilizada para dectar colisão entre o player e inimigos que usam trigger e chama a função
    // de dano OnHit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            OnHit();
        }

        if (collision.CompareTag("Coin"))
        {
            playerAudio.PlaySFX(playerAudio.coinSound);
            collision.GetComponent<Animator>().SetTrigger("hit");
            GameController.instance.GetCoin();
            Destroy(collision.gameObject, 0.4f);
        }

    }

    /*Função de dano no player
     * Ao ssofrer um dano o player fica invulnerável por um certo tempo e não soferá danos
     * O tempo é determinado pela variavel recoveryCount
     * A função tbm verifica se o player morreu ou não
     */
    float recoveryCount; //Tempo de imortalidade e não pegar dano
    public void OnHit()
    {
        recoveryCount += Time.deltaTime;

        if(recoveryCount >= 1f)
        {
            anim.SetTrigger("hit");
            health.health--;

            recoveryCount = 0f;
        }
        

        if(health.health <= 0 && !recovery)
        {
            recovery = true;
            anim.SetTrigger("death");

            //Aqui será feito o game over
            GameController.instance.ShowGameOver();
        }
    }
}
