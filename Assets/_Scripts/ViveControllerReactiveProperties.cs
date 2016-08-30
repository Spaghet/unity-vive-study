using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class ViveControllerReactiveProperties : MonoBehaviour {
	private SteamVR_Controller.Device _leftController;
	private SteamVR_Controller.Device _rightController;

	//Reactive Properties
	private ReactiveProperty<Vector2> _trigger = new ReactiveProperty<Vector2>();
	public IObservable<Vector2> TriggerStream {
		get { return _trigger.AsObservable(); }
	}
	
	public IObservable<Vector2> TriggerStream2 {
		get { return this.UpdateAsObservable().Select(_ => _leftController.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)); }
	}

	void Start() {
		//Get SteamVR controllers
		_leftController = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost));
		_rightController = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost));

		TriggerStream2
			.Subscribe(x =>
				Debug.LogFormat("Trigger(0.0~1.0): {0}", x.x)
			).AddTo(this.gameObject);
	}

	void Update() {
		_trigger.Value = _leftController.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
	}
}
