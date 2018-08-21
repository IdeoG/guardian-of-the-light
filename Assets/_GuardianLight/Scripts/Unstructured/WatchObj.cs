using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchObj : MonoBehaviour {

    float x, y, s;
    public float sensetivityX =1f, sensetivityY =1f;
    public float MinScale = 1f, MaxScale = 2f;
    float ScaleFactor;
    public float ScaleCoeficient = 1f;
    //public Transform DefoltScale;

    //Это для запоминания дефолтных настроек обьекта
    private Vector3 P;
    private Vector3 S;
    private Quaternion R;

    private Vector3 _origScale;
    private float _scaleFactor;

    // Use this for initialization
    private void Awake()
    {
        P = transform.localPosition;
        S = transform.localScale;
        R = transform.localRotation;
    }

    void Start () {
      // DefoltScale.localScale = new Vector3  (transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}

    private void OnEnable()
    {
        _origScale = transform.localScale;
        _scaleFactor = 1f;
    }

    private void OnDisable()
    {
        transform.localScale = _origScale;
    }

    // Update is called once per frame
    void Update () {
        y = -Input.GetAxis("Horizontal");
        x = Input.GetAxis("Vertical");
        s = Input.GetAxis("2JoyVertikal");

        ScaleFactor = ScaleCoeficient * s;

       transform.Rotate(new Vector3(x*sensetivityX, y* sensetivityY, 0f), Space.World);
       _scaleFactor += s * Time.deltaTime;
        _scaleFactor = Mathf.Clamp(_scaleFactor, MinScale, MaxScale);
       transform.localScale = _origScale * (_scaleFactor);
      
    }
    public void ResetObj()
    {
        transform.localPosition = P;
        transform.localScale = S;
        transform.localRotation = R;


    }
}
