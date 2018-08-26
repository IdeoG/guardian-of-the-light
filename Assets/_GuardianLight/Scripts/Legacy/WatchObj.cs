using UnityEngine;

public class WatchObj : MonoBehaviour
{
    private Vector3 _origScale;
    private float _scaleFactor;

    public float MinScale = 1f, MaxScale = 2f;
    //public Transform DefoltScale;

    //Это для запоминания дефолтных настроек обьекта
    private Vector3 P;
    private Quaternion R;
    private Vector3 S;
    public float ScaleCoeficient = 1f;
    private float ScaleFactor;
    public float sensetivityX = 1f, sensetivityY = 1f;

    private float x, y, s;

    // Use this for initialization
    private void Awake()
    {
        P = transform.localPosition;
        S = transform.localScale;
        R = transform.localRotation;
    }

    private void Start()
    {
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
    private void Update()
    {
        y = -Input.GetAxis("Horizontal");
        x = Input.GetAxis("Vertical");
        s = Input.GetAxis("2JoyVertikal");

        ScaleFactor = ScaleCoeficient * s;

        transform.Rotate(new Vector3(x * sensetivityX, y * sensetivityY, 0f), Space.World);
        _scaleFactor += s * Time.deltaTime;
        _scaleFactor = Mathf.Clamp(_scaleFactor, MinScale, MaxScale);
        transform.localScale = _origScale * _scaleFactor;
    }

    public void ResetObj()
    {
        transform.localPosition = P;
        transform.localScale = S;
        transform.localRotation = R;
    }
}