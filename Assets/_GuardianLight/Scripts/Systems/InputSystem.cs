using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public static InputSystem Instance { get; private set; }
    public IObservable<Unit> KeyActionPressedDown { get; private set; }
    public IObservable<Unit> KeyInspectPressedDown { get; private set; }
    public IObservable<Unit> KeyInventoryPressedDown { get; private set; }

    public IObservable<Unit> KeyLeftArrowPressed { get; private set; }
    public IObservable<Unit> KeyRightArrowPressed { get; private set; }
    public IObservable<Unit> KeyLeftArrowPressedDown { get; private set; }
    public IObservable<Unit> KeyRightArrowPressedDown { get; private set; }


    private void Awake()
    {
        Instance = this;

        KeyActionPressedDown = this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.E));

        KeyInventoryPressedDown = this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.I));

        KeyInspectPressedDown = this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.O));


        KeyLeftArrowPressed = this.UpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.A));

        KeyRightArrowPressed = this.UpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.D));
        
        KeyLeftArrowPressedDown = this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.A));

        KeyRightArrowPressedDown = this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.D));
    }
}