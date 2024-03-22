using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player1 : MonoBehaviour
{
    public float speed = 5;
    private Rigidbody2D rb;
    public float jumph = 5;
    private bool isgrounded = false;

    private Animator anim;
    private Vector3 rotation;

    private Coins m;

    public GameObject panel;
    public GameObject deatheffect;

    public GameObject kamera; // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rotation = transform.eulerAngles;
        m = GameObject.FindGameObjectWithTag("Text").GetComponent<Coins>();

    }

    // Update is called once per frame
    void Update()
    {
        float richtung = Input.GetAxis("Horizontal");

        if(richtung !=0)
        {
            anim.SetBool("Running", true);
        }
        else
        {
            anim.SetBool("Running", false);
        }
        if (richtung < 0)
        {
            transform.eulerAngles = rotation - new Vector3(0, 180, 0);
            transform.Translate(Vector2.right * speed * -richtung* Time.deltaTime);
        }
        if (richtung > 0)
        {
            transform.eulerAngles = rotation;
            transform.Translate(Vector2.right * speed * richtung* Time.deltaTime);
        }
        if(isgrounded == false)
        {
            anim.SetBool("Jumping", true);
        }
        else
        {
            anim.SetBool("Jumping", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isgrounded)
        {
            rb.AddForce(Vector2.up * jumph, ForceMode2D.Impulse);
            isgrounded = false;
        }
        kamera.transform.position = new Vector3(transform.position.x, 0, -10);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isgrounded = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Instantiate(deatheffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            panel.SetActive(true);
            FindObjectOfType<AudioManager>().Play("Die");
            
        }    
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            m.Addmoney();
            Destroy(other.gameObject);
            FindObjectOfType<AudioManager>().Play("Coin");
        }    
        if (other.gameObject.tag == "Spike")
        {
            Instantiate(deatheffect, transform.position, Quaternion.identity);
            panel.SetActive(true);
            Destroy(gameObject);
            FindObjectOfType<AudioManager>().Play("Die");

        }
        if (other.gameObject.tag == "Finish")
        {
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }    
    }
}
