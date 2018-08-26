using UnityEngine;
using UnityEngine.PostProcessing;
using UnityStandardAssets.Characters.ThirdPerson;

public class GameMannager : MonoBehaviour
{
    public GameObject CanvaseInventory;
    public GameObject CanvaseWatchObj;
    public GameObject CanvasLocationChanger;

    private DepthOfFieldModel.Settings DeffSettings;
    public float FocusDistansOnUI = 0.1f;
    public float FocusDistansStandart = 4.5f;
    public float FocusSpeed = 0.1f;
    public GameObject Gameobject_DooIt;
    public bool InFocus = true;

    public bool InInventory;
    public bool inWatchObj;
    public bool inWorld_0;
    public bool inWorld_1;

    public float LovFarCameraWorld = 25f;

    // все для UI
    public GameObject Plaer;
    public Transform Portal_World_0, Portal_World_1;

    // все для пост эффектов
    public PostProcessingProfile postEnviranment;
    [HideInInspector] public float StandartFarCameraWorld = 70f;

    // все для изменения мира
    public Transform StartPlaerPosition;
    public GameObject WordCamera;
    public GameObject World_0Prefab, World_1Prefab;


    // Use this for initialization
    private void Start()
    {
        DefaltPostprofSettings();
        voidOutPoused();

        inWorld_1 = true;
        World_0Prefab.SetActive(false);
        World_1Prefab.SetActive(true);
        Plaer.transform.position = StartPlaerPosition.position;
        StandartFarCameraWorld = WordCamera.GetComponent<Camera>().farClipPlane;
    }

    // Update is called once per frame
    private void Update()
    {
        // DeffSettings.focusDistance = FocusDistansOnUI;

        UI_Controls();
        //voidTimerBlur();
    }

    //пауза для перемещения персонада и вращения камеры
    public void voidOnPoused()
    {
        //фризим переещения персонажа
        Plaer.GetComponent<ThirdPersonUserControl>().LockInput = true;
        //фризим камеру
    }

    public void voidOutPoused()
    {
        // даем персу возможность ходить
        Plaer.GetComponent<ThirdPersonUserControl>().LockInput = false;
    }


    // он осуществляет переключение UI по кнопкам геймпада( в основном для нивенторя)
    //  написано коряво!!! ЛУчше наверно через кейсы или еще что по умнее!)
    public void UI_Controls()
    {
        //открытие инвентаря
        if ((Input.GetKeyUp(KeyCode.Joystick1Button3) || Input.GetKeyUp(KeyCode.I)) && !CanvaseInventory.activeSelf)
        {
            voidOnPoused();
            CanvaseInventory.SetActive(true);
            InInventory = true;
        }
        // выход из инвентаря
        else if (Input.GetKeyUp(KeyCode.Joystick1Button1) || Input.GetKeyUp(KeyCode.I))
        {
            voidOutPoused();
            CanvaseInventory.SetActive(false);
            inWatchObj = false;
            InInventory = false;
            WordCamera.GetComponent<Camera>().farClipPlane = StandartFarCameraWorld;
        }

        // переход к просмотру обьекта
        if ((Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyUp(KeyCode.O)) && InInventory)
        {
            inWatchObj = true;
            InInventory = true;
            CanvaseInventory.SetActive(false);
            CanvaseWatchObj.SetActive(true);

            WordCamera.GetComponent<Camera>().farClipPlane = LovFarCameraWorld;

            voidPostProcessUI();
            InFocus = false;
        }

        // переход от просмотра к инвенторю
        if (CanvaseWatchObj.activeSelf && (Input.GetKeyUp(KeyCode.Joystick1Button1) || Input.GetKeyUp(KeyCode.I)))
        {
            FindObjectOfType<WatchObj>().ResetObj();

            Plaer.GetComponent<ThirdPersonUserControl>().LockInput = true;

            CanvaseWatchObj.SetActive(false);
            CanvaseInventory.SetActive(true);

            DefaltPostprofSettings();
            InFocus = true;

            inWatchObj = false;
            InInventory = true;
        }
    }


    // пост процессы
    public void DefaltPostprofSettings()
    {
        // меняет блюр обратно как было при игре
        DeffSettings.focusDistance = FocusDistansStandart;

        DeffSettings.aperture = 0.7f;
        DeffSettings.useCameraFov = true;
        DeffSettings.kernelSize = DepthOfFieldModel.KernelSize.Medium;
        postEnviranment.depthOfField.settings = DeffSettings;
    }

    // меняет дефолтные пост эффекты на мои при UI
    public void voidPostProcessUI()
    {
        DeffSettings.focusDistance = FocusDistansOnUI;

        DeffSettings.aperture = 0.7f;
        DeffSettings.useCameraFov = true;
        DeffSettings.kernelSize = DepthOfFieldModel.KernelSize.Medium;

        postEnviranment.depthOfField.settings = DeffSettings;
    }

    public void voidTimerBlur()
    {
        if (!InFocus && DeffSettings.focusDistance > FocusDistansOnUI)
        {
            DeffSettings.focusDistance -= FocusSpeed;
            Debug.Log("OutFocus");
        }

        if (InFocus && DeffSettings.focusDistance < FocusDistansStandart)
        {
            DeffSettings.focusDistance = FocusDistansOnUI + FocusSpeed * Time.deltaTime;
            Debug.Log("InFocus");
        }
    }

    // этот метод меняет мир 1 на мир 0 и можно дописать еще)
    public void WorldChanger()
    {
        if (inWorld_1)
        {
            World_0Prefab.SetActive(true);
            World_1Prefab.SetActive(false);

            Plaer.transform.position = Portal_World_0.position;

            inWorld_1 = false;
            inWorld_0 = true;
        }
        else if (inWorld_0)
        {
            World_0Prefab.SetActive(false);
            World_1Prefab.SetActive(true);

            Plaer.transform.position = Portal_World_1.position;

            inWorld_1 = true;
            inWorld_0 = false;
        }

        CanvasLocationChanger.SetActive(false);
    }
}