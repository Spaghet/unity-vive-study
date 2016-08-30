/*
 Wrapper class for default SteamVR_Controller as well as export of each button/
 */

using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class ViveWrapper : MonoBehaviour {
	
	void Start() {
		//Get SteamVR controllers
		SteamVR_TrackedObject trackedObject = GetComponent<SteamVR_TrackedObject>();
		var device = SteamVR_Controller.Input((int)trackedObject.index);

		//Manual way of doing things
		this.UpdateAsObservable()
			.Where(_ => device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
			.Select(_ => device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0))
			.Subscribe(x => {
				Debug.Log(x);
		    }).AddTo(this.gameObject);	
	}
}
