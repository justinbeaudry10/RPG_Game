using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginningNote : MonoBehaviour
{
    public GameObject note;                         //Declaration of the note gO

    // Start is called before the first frame update
    void Start()
    {
        //Making sure the UI object is set as false at beginning
        note.SetActive(false);
    }

    // Activates Note UI object on screen upon player collision with object
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Note collided with player");
            note.SetActive(true);
        }
    } 
    private void Update()
    {
       //Closes UI Object upon key press of "C"
       if (Input.GetKeyDown(KeyCode.X))
        {
            Destroy(note);
            Destroy(gameObject);
        }
    }
}
