using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Text;
using System;

public class ConsoliAdsSample : MonoBehaviour {
    
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

	AdSize size = new AdSize (300,250);
	AdPosition position = new AdPosition (100 , 200);
	ConsoliAdsBannerView consoliAdsBannerView;
	ConsoliAdsBannerView consoliAdsBannerViewSecond;

	public InputField deviceID;

	public 

	void Start () {

		consoliAdsBannerView = new ConsoliAdsBannerView ();
		consoliAdsBannerViewSecond = new ConsoliAdsBannerView ();

		adNetworkListText.text = "";

		AdTypeList.Add ("Interstitial/Video");
		AdTypeList.Add ("Rewarded");
		AdTypeList.Add ("Banner");
		AdTypeList.Add ("Native");
		AdTypeList.Add ("Icon");
		AdTypeList.Add ("Second Banner");

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
		ConsoliAds.onConsoliAdsInitializationSuccess += onConsoliAdsInitialization;
		// Listen to all impression-related events
		ConsoliAds.onInterstitialAdShownEvent += onInterstitialAdShown;
		ConsoliAds.onInterstitialAdFailedToShowEvent += onInterstitialAdFailedToShow;
		ConsoliAds.onInterstitialAdClosedEvent += onInterstitialAdClosed;
		ConsoliAds.onInterstitialAdClickedEvent += onInterstitialAdClicked;

		ConsoliAds.onRewardedVideoAdShownEvent += onRewardedVideoAdShown;
		ConsoliAds.onRewardedVideoAdFailToShowEvent += onRewardedVideoAdFailToShow;
		ConsoliAds.onRewardedVideoAdLoadedEvent += onRewardedVideoAdLoaded;
		ConsoliAds.onRewardedVideoAdFailToLoadEvent += onRewardedVideoAdFailToLoad;
		ConsoliAds.onRewardedVideoAdClosedEvent += onRewardedVideoAdClosed;
		ConsoliAds.onRewardedVideoAdClickEvent += onRewardedVideoAdShown;
		ConsoliAds.onRewardedVideoAdCompletedEvent += onRewardedVideoCompleted;

		ConsoliAds.onBannerAdShownEvent += onBannerAdShown;
		ConsoliAds.onBannerAdRefreshEvent += onBannerAdRefresh;
		ConsoliAds.onBannerAdFailToShowEvent += onBannerAdFailToShow;
		ConsoliAds.onBannerAdClickEvent += onBannerAdClick;

		ConsoliAds.onIconAdCloseEvent += didCloseIconAd;
		ConsoliAds.onIconAdClickEvent += didClickIconAd;
		ConsoliAds.onIconAdShownEvent+= didDisplayIconAd;
		ConsoliAds.onIconAdRefreshEvent += didRefreshIconAd;
		ConsoliAds.onIconAdFailedToShowEvent += didFailedToLoadIconAd;

		ConsoliAds.onNativeAdShownEvent += onNativeAdLoaded;
		ConsoliAds.onNativeAdFailedToShownEvent += onNativeAdFailedToLoad;
	}

	public void InitializeButtonPressed()
	{
		
	}

	void Update () {

		if (ConsoliAds.Instance == null) {
			return;
		}
		if (ConsoliAds.Instance.IsInitialized && isNeedToUpdate) {

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
						foreach (AdNetworkName adNetwork in scene.bannerDetails.networkList ) 
						{
							bld.Append("" + adNetwork + "\n");
						}
					}
				}
				break;
			case 3:
				{
					if (scene.nativeDetails.enabled) {
						foreach (AdNetworkName adNetwork in scene.nativeDetails.networkList ) 
							{
								bld.Append("" + adNetwork + "\n");
							}
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
		case 5:
			{
				if (scene.bannerDetails.enabled) {
					foreach (AdNetworkName adNetwork in scene.bannerDetails.networkList ) 
					{
						bld.Append("" + adNetwork + "\n");
					}
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

		if (ConsoliAds.Instance.IsInitialized)
        {
			updateAdNetworkText (SceneNameDropDown.value);
		}
	}

	public void sceneNameDropDownValueChanged(int value) {
		if(ConsoliAds.Instance.IsInitialized)
        {
			updateAdNetworkText (SceneNameDropDown.value);
		}
	}

	public void isAdAvailable()
	{
		if (ConsoliAds.Instance.IsInitialized) {

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
		if (ConsoliAds.Instance.IsInitialized) {

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
						ConsoliAds.Instance.ShowBanner(SceneNameDropDown.value , consoliAdsBannerView);
					}
				break;
			case 3:
				{
					ConsoliAds.Instance.ShowNativeAd (nativeAdGameObject , SceneNameDropDown.value);
				}
				break;
			case 4:
				{
					ConsoliAds.Instance.ShowIconAd (iconAdGameObject , SceneNameDropDown.value , IconAnimationType.PULSE);
				}
				break;
			case 5:
				{
					ConsoliAds.Instance.ShowBanner(SceneNameDropDown.value , consoliAdsBannerViewSecond);
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

		//ConsoliAds.Instance.getDataFromPlatform ("41");

		Debug.Log ("request Ad called " + SceneNameDropDown.value);

		if (ConsoliAds.Instance.IsInitialized) {
			
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
					ConsoliAds.Instance.HideBanner(consoliAdsBannerView);
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
				case 5:
				{
					ConsoliAds.Instance.HideBanner(consoliAdsBannerViewSecond);
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
		Debug.Log("Sample: onConsoliAdsInitialization called ");
	}

	void onInterstitialAdShown()
	{
		Debug.Log("Sample: onInterstitialAdShown called");
	}

	void onInterstitialAdFailedToShow()
	{
		Debug.Log("Sample: onInterstitialAdFailedToShow called");
	}

	void onInterstitialAdClosed()
	{
		Debug.Log("Sample: onInterstitialAdClosed called");
	}

	void onInterstitialAdClicked()
	{
		Debug.Log("Sample: onInterstitialAdClicked called");
	}

	void onRewardedVideoAdShown()
	{
		Debug.Log("Sample: onRewardedVideoAdShown called");
	}

	void onRewardedVideoAdFailToShow()
	{
		Debug.Log("Sample: onRewardedVideoAdFailToShow called");
	}

	void onRewardedVideoAdLoaded()
	{
		Debug.Log("Sample: onRewardedVideoAdLoaded called");
	}

	void onRewardedVideoAdFailToLoad()
	{
		Debug.Log("Sample: onRewardedVideoAdFailToLoad called");
	}

	void onRewardedVideoAdClosed()
	{
		Debug.Log("Sample: onRewardedVideoAdClosed called");
	}

	void onRewardedVideoCompleted()
	{
		Debug.Log("Sample: onRewardedVideoCompleted called");
	}

	void onBannerAdShown()
	{
		Debug.Log("Sample: onBannerAdShown called");
	}

	void onBannerAdFailToShow()
	{
		Debug.Log("Sample: onBannerAdFailToShow called");
	}

	void onBannerAdClick()
	{
		Debug.Log("Sample: onBannerAdClick called");
	}

	void onBannerAdRefresh()
	{
		Debug.Log("Sample: onBannerAdRefresh called");
	}

	void didFailedToLoadIconAd()
	{
		Debug.Log("Sample: didFailedToLoadIconAd called");
	}
	void didLoadIconAd()
	{
		Debug.Log("Sample: didLoadIconAd called");
	}
	void didRefreshIconAd()
	{
		Debug.Log("Sample: didRefreshIconAd called");
	}
	void didDisplayIconAd()
	{
		Debug.Log("Sample: didDisplayIconAd called");
	}
	void didClickIconAd()
	{
		Debug.Log("Sample: didClickIconAd called");
	}
	void didCloseIconAd()
	{
		Debug.Log("Sample: didCloseIconAd called");
	}

	void onNativeAdLoaded()
	{
		Debug.Log("Sample: onNativeAdLoaded called : ");
	}

	void onNativeAdFailedToLoad()
	{
		Debug.Log("Sample: onNativeAdFailedToLoad called : ");
	}

}
