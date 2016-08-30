using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class ViveControllerReactiveProperties : MonoBehaviour {
	[SerializeField]
	private GameObject _controller;
	private SteamVR_Controller.Device _device;

	//Reactive Properties
	private ReactiveProperty<Vector2> _trigger = new ReactiveProperty<Vector2>();
	public IObservable<Vector2> TriggerStream {
		get { return _trigger.AsObservable(); }
	}
	
	public IObservable<Vector2> TriggerStream2 {
		get { return this.UpdateAsObservable().Select(_ => _device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)); }
	}

	void Start() {
		//Get SteamVR controllers
		SteamVR_TrackedObject trackedObject = GetComponent<SteamVR_TrackedObject>();
		_device = SteamVR_Controller.Input((int)trackedObject.index);

		TriggerStream2
			.Subscribe(x =>
				Debug.LogFormat("Trigger(0.0~1.0): {0}", x.x)
			).AddTo(this.gameObject);
	}

	void Update() {
		_trigger.Value = _device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
	}
}
