using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveIndicator : MonoBehaviour
{
    private Vector3 initialPosition;
    private float bobbingSpeed;
    private string moveDir;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        bobbingSpeed = 0.05f;
        moveDir = "up";
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaPos = transform.position - initialPosition;

        if(deltaPos.y > 0.08f && moveDir == "up") {
            moveDir = "down";
        } else if(deltaPos.y < -0.01f && moveDir == "down") {
            moveDir = "up";
        }

        if(moveDir == "up") {
            MoveAndScaleUp();
        } else if(moveDir == "down") {
            MoveAndScaleDown();
        }
    }

    private void MoveAndScaleUp() {
        transform.position += new Vector3(0f, Time.deltaTime * bobbingSpeed, 0f);
        transform.localScale += new Vector3(Time.deltaTime * bobbingSpeed, Time.deltaTime * bobbingSpeed, 0f);
    }

    private void MoveAndScaleDown() {
        transform.position -= new Vector3(0f, Time.deltaTime * bobbingSpeed, 0f); 
        transform.localScale -= new Vector3(Time.deltaTime * bobbingSpeed, Time.deltaTime * bobbingSpeed, 0f);
    }

}
