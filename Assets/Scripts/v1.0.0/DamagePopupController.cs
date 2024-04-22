using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopupController : MonoBehaviour
{

    private float disappearTimer;
    private Color textColor;
    private TextMeshPro textMesh;
    private Vector3 moveVector = new Vector3(0.5f,1f) * 3f;
    private const float DISAPPEAR_TIMER_MAX = 1f;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        textColor = textMesh.color;
        transform.position += new Vector3(1,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 6f * Time.deltaTime;


        if(disappearTimer > DISAPPEAR_TIMER_MAX * 0.5f) {
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        } else {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }
        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0) {
            float disappearSpeed = 1.5f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if(textColor.a < 0) {
                Destroy(gameObject);
            }
            
        }
        
    }
}
