using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
	public static InputSystem Instance { get; private set; }
	public IObservable<Unit> KeyActionPressed { get; private set; }
	public IObservable<Unit> KeyInspectPressed { get; private set; }
	public IObservable<Unit> KeyInventoryPressed { get; private set; }
	
	private void Awake()
	{
		Instance = this;
		
		KeyActionPressed = this.UpdateAsObservable()
			.Where(_ => Input.GetKeyDown(KeyCode.E));

		KeyInventoryPressed = this.UpdateAsObservable()
			.Where(_ => Input.GetKeyDown(KeyCode.I));
		
		KeyInspectPressed = this.UpdateAsObservable()
			.Where(_ => Input.GetKeyDown(KeyCode.O));
	}
}
