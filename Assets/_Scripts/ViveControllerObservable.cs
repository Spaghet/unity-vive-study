using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ViveControllerObservable : MonoBehaviour {
	private static ObservableController _leftController;
	private static ObservableController _rightController;

	public static ObservableController LeftController {
		get {
			if(_leftController == null) {
				_leftController = new ObservableController(ObservableController.Chirarity.Left);
			}
			return _leftController;
		}
	}

	public static ObservableController RightController {
		get {
			if (_rightController == null) {
				_rightController = new ObservableController(ObservableController.Chirarity.Right);
			}
			return _rightController;
		}
	}

	#region Controller
	public class ObservableController{
		#region Private
		Chirarity _chirarity;
		SteamVR_Controller.Device _controller;
		#endregion
		#region Public
		public ObservableController(Chirarity chirarity) {
			var deviceRelation = _chirarity == Chirarity.Left ? SteamVR_Controller.DeviceRelation.Leftmost : SteamVR_Controller.DeviceRelation.Rightmost;
			
			_chirarity = chirarity;
			_controller = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(deviceRelation));
		}
		public enum Chirarity {
			Left,
			Right
		}

		//Axis stuff

		//TriggerState as float value every frame
		public IObservable<float> TriggerStream {
			get { return Observable.EveryUpdate().Select(_ => _controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x); }
		}
		//TouchPad State as Vector2 every frame
		public IObservable<Vector2> TouchPadStream {
			get { return Observable.EveryUpdate().Select(_ => _controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)); }
		}

		//ButtonTouch (Light/Shallow Press)

		public IObservable<bool> TriggerTouchStream {
			get { return Observable.EveryUpdate().Select(_ => _controller.GetTouch(SteamVR_Controller.ButtonMask.Trigger)); }
		}

		public IObservable<bool> GripTouchStream {
			get { return Observable.EveryUpdate().Select(_ => _controller.GetTouch(SteamVR_Controller.ButtonMask.Grip)); }
		}

		public IObservable<bool> ApplicationMenuTouchStream {
			get { return Observable.EveryUpdate().Select(_ => _controller.GetTouch(SteamVR_Controller.ButtonMask.ApplicationMenu)); }
		}

		public IObservable<bool> TouchPadTouchStream {
			get { return Observable.EveryUpdate().Select(_ => _controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)); }
		}

		//ButtonPress (Deep/Hard Press)
		public IObservable<bool> TriggerPressStream {
			get { return Observable.EveryUpdate().Select(_ => _controller.GetPress(SteamVR_Controller.ButtonMask.Trigger)); }
		}

		public IObservable<bool> GripPressStream {
			get { return Observable.EveryUpdate().Select(_ => _controller.GetPress(SteamVR_Controller.ButtonMask.Grip)); }
		}

		public IObservable<bool> ApplicationMenuPressStream {
			get { return Observable.EveryUpdate().Select(_ => _controller.GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu)); }
		}

		public IObservable<bool> TouchPadPressStream {
			get { return Observable.EveryUpdate().Select(_ => _controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad)); }
		}

		//Public Methods
		public void HapticPulse(int strength) {
			strength = Mathf.Clamp(strength, 100, 2000);
			_controller.TriggerHapticPulse((ushort)strength);
		}
		#endregion

	}
	#endregion
}
