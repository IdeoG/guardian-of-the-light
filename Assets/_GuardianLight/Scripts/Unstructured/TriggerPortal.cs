using UnityEngine;

public class TriggerPortal : MonoBehaviour
{
    [HideInInspector] public GameObject gameObjDoit, gameObjLocationChanger;

    [HideInInspector] public bool inThinking;

    private void Start()
    {
        gameObjDoit = FindObjectOfType<GameMannager>().Gameobject_DooIt;
        gameObjLocationChanger = FindObjectOfType<GameMannager>().CanvasLocationChanger;
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player") gameObjLocationChanger.SetActive(true);
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player") gameObjLocationChanger.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        // voidChangerWorld();

        if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Joystick1Button2))
        {
            gameObjLocationChanger.SetActive(false);
            FindObjectOfType<GameMannager>().WorldChanger();
            // gameObjDoit.SetActive(true);

            // FindObjectOfType<GameMannager>().voidOnPoused();
            //inThinking = true;
        }
    }

    // пореключет тебя между игровыми мирами
    /* public void voidChangerWorld()
    {
        if (inThinking && (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Joystick1Button2)))
        {
            FindObjectOfType<GameMannager>().WorldChanger();
            FindObjectOfType<GameMannager>().voidOutPoused();

            gameObjDoit.SetActive(false);


            inThinking = false;
            }
            }
        */
}