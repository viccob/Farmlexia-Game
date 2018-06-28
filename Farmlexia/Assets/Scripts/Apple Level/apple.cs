using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class apple : MonoBehaviour {

    TextMesh textM;
    string text;
    Animator appleAnim;

    appleLevelManager manager;

    public float timer, velocity;

    private string appleState;

    string[] textList;

    public GameObject failSymbol, failParticles;
    ParticleSystem particles;

	void Start () {

        setAppleState("Falling");

        manager = FindObjectOfType<appleLevelManager>().GetComponent<appleLevelManager>();

        textList = manager.getSyllablesList();

        appleAnim = GetComponent<Animator>();
        textM = GetComponentInChildren<TextMesh>();

        particles = GetComponent<ParticleSystem>();
        particles.Stop();

        //text = textList[Random.Range(0,textList.Length)];
        text = manager.nextSyllaba;
        textM.text = text;

        timer = 0;
        velocity = manager.getAppleVelocity();
	}
	

	void Update () {


        switch (appleState) {

            case "Falling":

                transform.position = transform.position - new Vector3(0, velocity, 0) * Time.deltaTime;                
                break;


            case "Eliminated":

                timer += Time.deltaTime;
                if (timer > 2) Destroy(this.gameObject);
                break;


            case "Hit":

                var heading = new Vector3(-8.77f, -5.68f, 0) - transform.position;

                var distance = heading.magnitude;
                var direction = heading / distance;

                if (distance < 1.5f) { Destroy(gameObject); }
                else {
                    transform.Translate(direction * 2.5f);
                    transform.localScale -= new Vector3(0.03f, 0.03f, 0);
                }

                break;

            case "Fail":

                transform.localScale = transform.localScale - new Vector3(0.1f, 0.1f, 0);

                if (transform.localScale.x <= 0.1 || transform.localScale.y <= 0.1) {

                    if (!isGood()) { Destroy(failParticles); }
                    Destroy(gameObject);                  
                }
                
                

                break;

        }
	}
    

    void OnTriggerEnter2D(Collider2D otro) {

        if (otro.tag=="suelo" && appleState!="Hit") {

            textM.text = "";
            appleAnim.SetBool("isPlof", true);

            setAppleState("Eliminated");

            if (isGood() && manager.getGameState() == "GameON")
            {
                Instantiate(failSymbol, transform.position, Quaternion.identity);
                setAppleState("Fail");
                manager.badSelection();// manager.badSelection();
            }

        } 
    }
    

    void OnMouseDown()
    {
        if (appleState != "Eliminated")
        {
            transform.localScale += new Vector3(0.3f, 0.3f, 0f);
            if (manager.getGoodSyllable() == text)
            {
                manager.goodSelection();

                particles.Play();
                setAppleState("Hit");

            }
            else
            {

                manager.badSelection();
                Instantiate(failSymbol, transform.position, Quaternion.identity);
                failParticles = Instantiate(failParticles, transform.position, Quaternion.identity);

                setAppleState("Fail");
                manager.changeProbability(text);
            }
        }
    }


    //GETTERS
    public bool isGood() { return text == manager.getGoodSyllable(); }

    public float getAppleVelocity() { return velocity; }

    //SETTERS
    public void setAppleState(string state) { appleState = state; }

    public void setAppleVelocity(float vel) { velocity = vel; }


}
