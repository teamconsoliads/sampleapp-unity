using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Text;
using System;

public class ConsoliAdsSample : MonoBehaviour {

	private bool isInitialized = true;
	private bool isNeedToUpdate = true;

	[Header("Texts:")]
	public Text adNetworkListText;

	[Header("Buttons:")]
	public Button showAdButton;

	[Header("Toggles:")]
	public Toggle RoundRobinToggle;
	public Toggle PriorityToggle;
	public Toggle UserConsentToggle;

	[Header("DropDown:")]
	public Dropdown AdTypeDropDown;
	public Dropdown SceneNameDropDown;

	public List<string> AdTypeList = new List<string>();
	public List<string> SceneNameList = new List<string>();
	public GameObject iconAdGameObject;
	public GameObject nativeAdGameObject;

	public InputField deviceID;

	public 

	void Start () {

		adNetworkListText.text = "";

		AdTypeList.Add ("Interstitial/Video");
		AdTypeList.Add ("Rewarded");
		AdTypeList.Add ("Banner");
		AdTypeList.Add ("Native");
		AdTypeList.Add ("Icon");

		AdTypeDropDown.options.Clear ();
		SceneNameDropDown.options.Clear ();

		//fill the dropdown menu OptionData with all COM's Name in ports[]
		foreach (string c in AdTypeList) 
		{
			AdTypeDropDown.options.Add (new Dropdown.OptionData() {text=c});
		}
		//this swith from 1 to 0 is only to refresh the visual DdMenu
		AdTypeDropDown.value = 1;
		AdTypeDropDown.value = 0;
	}

	void Awake()
	{
		SetupEvents();
	}

	void SetupEvents()
	{
		// Listen to all impression-related events
		ConsoliAds.onInterstitialAdShownEvent += onInterstitialAdShown;
		ConsoliAds.onVideoAdShownEvent += onVideoAdShown;
		ConsoliAds.onRewardedVideoAdShownEvent += onRewardedVideoAdShown;
		ConsoliAds.onPopupAdShownEvent += onPopupAdShown;
		ConsoliAds.onRewardedVideoAdCompletedEvent += onRewardedVideoCompleted;
		ConsoliAds.onConsoliAdsInitializationSuccess += onConsoliAdsInitialization;
	}

	public void InitializeButtonPressed()
	{
		
	}

	void Update () {

		if (ConsoliAds.Instance == null) {
			return;
		}
		if (isInitialized && isNeedToUpdate) {

			isNeedToUpdate = false;
		
			SceneNameDropDown.options.Clear ();

			foreach (CAScene scene in ConsoliAds.Instance.scenesArray) 
			{
				SceneNameList.Add ("" + scene.placeholderName);
				SceneNameDropDown.options.Add (new Dropdown.OptionData() {text= "" + scene.placeholderName});
			}
			SceneNameDropDown.value = 1;
			SceneNameDropDown.value = 0;
		}

	}

	public void updateAdNetworkText(int sceneID){
	
		if (ConsoliAds.Instance == null) {
			return;
		}
			
		if (sceneID >= ConsoliAds.Instance.scenesArray.Length) {
			return;
		}
		CAScene scene = ConsoliAds.Instance.scenesArray [sceneID];
		var bld = new StringBuilder();

		string str = "AdNetworks: \n";

		switch (AdTypeDropDown.value)
		{
			case 0:
				{
					foreach (AdNetworkName adNetwork in scene.interstitialAndVideoDetails.networkList ) 
					{
							bld.Append( "" + adNetwork + "\n");
					}
				}
				break;
			case 1:
				{
					foreach (AdNetworkName adNetwork in scene.rewardedVideoDetails.networkList ) 
					{
							bld.Append("" + adNetwork + "\n");
					}
				}
				break;
			case 2:
				{
					if (scene.bannerDetails.enabled) {
							bld.Append("" + scene.bannerDetails.adType + "\n");
					}
				}
				break;
			case 3:
				{
					if (scene.nativeDetails.enabled) {
							bld.Append("" + scene.nativeDetails.adType + "\n");
					}
				}
				break;
			case 4:
				{
						if (scene.iconDetails.enabled) {
							bld.Append("" + scene.iconDetails.adType + "\n");
						}
				}
				break;
		}
		adNetworkListText.text = bld.ToString();
	}

	public void roundRobinToggleValueChange() {

		PriorityToggle.isOn = !RoundRobinToggle.isOn;
		if(RoundRobinToggle.isOn){
			ConsoliAds.Instance.setShowAdMechanism (ConsoliAdsShowAdMechanism.RoundRobin);
		}
	}

	public void priorityToggleValueChange() {

		RoundRobinToggle.isOn = !PriorityToggle.isOn;
		if(PriorityToggle.isOn){
			ConsoliAds.Instance.setShowAdMechanism (ConsoliAdsShowAdMechanism.RoundRobin);
		}
	}
		
	public void adTypeDropDownValueChanged(int value) {

		if (isInitialized){
			updateAdNetworkText (SceneNameDropDown.value);
		}
	}

	public void sceneNameDropDownValueChanged(int value) {
		if(isInitialized){
			updateAdNetworkText (SceneNameDropDown.value);
		}
	}

	public void isAdAvailable()
	{
		if (isInitialized) {

			switch (AdTypeDropDown.value) 
			{
			case 0:
				{
					Debug.Log ("IsInterstitialAvailable " + ConsoliAds.Instance.IsInterstitialAvailable (SceneNameDropDown.value));
				}
				break;
			case 1:
				{
					Debug.Log ("IsRewardedVideoAvailable " + ConsoliAds.Instance.IsRewardedVideoAvailable (SceneNameDropDown.value));
				}
				break;
			}
		}
		else {
			Debug.Log ("Consoliads is not Initialized");
		}

	}

	public void showAd()
	{
		Debug.Log ("showAd called " + SceneNameDropDown.value);
		if (isInitialized) {

			switch (AdTypeDropDown.value) 
			{
				case 0:
					{
						ConsoliAds.Instance.ShowInterstitial(SceneNameDropDown.value);
					}
				break;
				case 1:
					{
						ConsoliAds.Instance.ShowRewardedVideo(SceneNameDropDown.value);
					}
				break;
				case 2:
					{
						ConsoliAds.Instance.ShowBanner(SceneNameDropDown.value);
					}
				break;
			case 3:
				{
					ConsoliAds.Instance.ShowNativeAd (nativeAdGameObject , SceneNameDropDown.value);
				}
				break;
			case 4:
				{
					ConsoliAds.Instance.ShowIconAd (iconAdGameObject , SceneNameDropDown.value);
				}
				break;
			}
		}
		else {
			Debug.Log ("Consoliads is not Initialized");
		}

	}

	public void RequestAd()
	{

		Debug.Log ("request Ad called " + SceneNameDropDown.value);

		if (isInitialized) {
			
			switch (AdTypeDropDown.value) {
				case 0:
				break;
				case 1:
					{
						ConsoliAds.Instance.LoadRewarded(SceneNameDropDown.value);
					}
				break;
				case 2:
				{
						ConsoliAds.Instance.HideBanner();
				}
				break;
				case 3:
				{
						ConsoliAds.Instance.HideNative(SceneNameDropDown.value);
				}
				break;
				case 4:
				{
						ConsoliAds.Instance.DestoryIconAd(iconAdGameObject , SceneNameDropDown.value);
				}
				break;
			}
		}			
	}

	public void addTestDevice()
	{
		string deviceId = "";
		try {
			deviceId = deviceID.text.ToString();
		}
		catch (Exception ex) {
		}
		ConsoliAds.Instance.addAdmobTestDevice (deviceId);
	}

	/* ------------------------ */

	void onConsoliAdsInitialization()
	{
		isInitialized = true;
		Debug.Log("Sample: onConsoliAdsInitialization called ");
	}

	void onInterstitialAdShown()
	{
		Debug.Log("Sample: onInterstitialAdShown called");
	}
	void onVideoAdShown()
	{
		Debug.Log("Sample: onVideoAdShown called");
	}
	void onRewardedVideoAdShown()
	{
		Debug.Log("Sample: onRewardedVideoAdShown called");
	}
	void onPopupAdShown()
	{
		Debug.Log("Sample: onPopupAdShown called");
	}
	public void onRewardedVideoCompleted()
	{
		Debug.Log("Sample: Event received : Rewarded Video Complete");
	}
}
