using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControllerClient : MonoBehaviour
{
    public int movSpeed = 100;
    public GameObject consoleUDP;
    public GameObject consoleTCP;

    Animator animator;

    private bool nearUDP = false;
    private bool nearTCP = false;

    private bool insideUI = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the user is focused on a UI input field
        bool isTypingInInputField = EventSystem.current.currentSelectedGameObject != null &&
                                    EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null;

        Vector3 speed = new Vector3(0, 0, 0);

        if (!insideUI)
        {
            if (Input.GetKey("d"))
            {
                // speed += Vector3.Cross(new Vector3(1, 0, 0), this.transform.forward) * movSpeed;
                speed += new Vector3(1, 0, 0) * movSpeed;
            }
            if (Input.GetKey("a"))
            {
                //speed += Vector3.Cross(new Vector3(-1, 0, 0), this.transform.forward) * movSpeed;
                speed += new Vector3(-1, 0, 0) * movSpeed;

            }
            if (Input.GetKey("w"))
            {
                // speed += Vector3.Cross(new Vector3(0, 0, 1), this.transform.forward) * movSpeed;
                speed += new Vector3(0, 0, 1) * movSpeed;

            }
            if (Input.GetKey("s"))
            {
                //speed += Vector3.Cross(new Vector3(0, 0, -1), this.transform.forward) * movSpeed;
                speed += new Vector3(0, 0, -1) * movSpeed;

            }
        }  

        this.gameObject.GetComponent<Rigidbody>().velocity = speed;

        if (speed != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(this.gameObject.GetComponent<Rigidbody>().velocity);
            animator.SetBool("isWalking", true);
        }
        else { animator.SetBool("isWalking", false); }

        if (nearUDP && Input.GetKeyDown("e") && !isTypingInInputField)
        {
            consoleUDP.SetActive(!consoleUDP.activeSelf);
            insideUI = !insideUI;

        }
        if (nearTCP && Input.GetKeyDown("e") && !isTypingInInputField)
        {
            consoleTCP.SetActive(!consoleTCP.activeSelf);
            insideUI = !insideUI;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("ComputerTCP"))
        {
            nearTCP = true;
        }
        if (other.gameObject.CompareTag("ComputerUDP"))
        {
            nearUDP = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("ComputerTCP"))
        {
            nearTCP = false;
            consoleTCP.SetActive(false);
        }
        if (other.gameObject.CompareTag("ComputerUDP"))
        {
            nearUDP = false;
            consoleUDP.SetActive(false);
        }
    }
}


