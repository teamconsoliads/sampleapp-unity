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
	public Toggle UserConsentToggle;

	[Header("DropDown:")]
	public Dropdown AdTypeDropDown;
	public Dropdown SceneNameDropDown;

	public List<string> AdTypeList = new List<string>();
	public List<string> SceneNameList = new List<string>();
	public GameObject iconAdGameObject;
	public GameObject nativeAdGameObject;
	public InputField deviceID;

	public void updateAdNetworkText(int sceneID){

		return;

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
		case 6:
		case 7:
		case 8:
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

	void Start () {

		adNetworkListText.text = "";

		AdTypeList.Add ("Interstitial/Video");
		AdTypeList.Add ("Rewarded");
		AdTypeList.Add ("SimpleBanner");
		AdTypeList.Add ("Native");
		AdTypeList.Add ("Icon");
		AdTypeList.Add ("Custom Position Banner");
		AdTypeList.Add ("Custom Size Banner");
		AdTypeList.Add ("Full Custom Banner");
		AdTypeList.Add ("Pending Banner");

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

		SceneNameDropDown.options.Add (new Dropdown.OptionData () { text = Enum.GetName (typeof(PlaceholderName), (int)PlaceholderName.Default) });

		//fill the dropdown menu OptionData with all COM's Name in ports[]
		foreach(PlaceholderName c in Enum.GetValues(typeof(PlaceholderName)))
		{
			if (c != PlaceholderName.Default) {
				SceneNameDropDown.options.Add (new Dropdown.OptionData () { text = Enum.GetName (typeof(PlaceholderName), (int)c) });
			}
		}
			
		//this swith from 1 to 0 is only to refresh the visual DdMenu
		SceneNameDropDown.value = 1;
		SceneNameDropDown.value = 0;

		//updateAdNetworkText (0);
	}

	void Awake()
	{
		SetupEvents();
	}

	void SetupEvents()
	{
		ConsoliAds.onConsoliAdsInitializationSuccess += onConsoliAdsInitialization;
		// Listen to all impression-related events

		ConsoliAds.onInterstitialAdLoadedEvent += onInterstitialAdLoaded;
		ConsoliAds.onInterstitialAdFailToLoadEvent += onInterstitialAdFailToLoad;
		ConsoliAds.onInterstitialAdShownEvent += onInterstitialAdShown;
		ConsoliAds.onInterstitialAdFailedToShowEvent += onInterstitialAdFailedToShow;
		ConsoliAds.onInterstitialAdClosedEvent += onInterstitialAdClosed;
		ConsoliAds.onInterstitialAdClickedEvent += onInterstitialAdClicked;

		ConsoliAds.onRewardedVideoAdShownEvent += onRewardedVideoAdShown;
		ConsoliAds.onRewardedVideoAdFailToShowEvent += onRewardedVideoAdFailToShow;
		ConsoliAds.onRewardedVideoAdLoadedEvent += onRewardedVideoAdLoaded;
		ConsoliAds.onRewardedVideoAdFailToLoadEvent += onRewardedVideoAdFailToLoad;
		ConsoliAds.onRewardedVideoAdClosedEvent += onRewardedVideoAdClosed;
		ConsoliAds.onRewardedVideoAdClickEvent += onRewardedVideoAdClick;
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
		ConsoliAds.onNativeAdClickEvent += onNativeAdClicked;
	}

	public void InitializeButtonPressed()
	{
		
	}

	void Update () {
		/*
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
		*/

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
			//updateAdNetworkText (SceneNameDropDown.value);
		}
	}

	public void isAdAvailable()
	{
		if (ConsoliAds.Instance.IsInitialized) {

			int sceneValue = SceneNameDropDown.value;
			Debug.Log ("showAd called " + sceneValue);

			if (sceneValue == 0) {
				sceneValue = 27;
			}

			Debug.Log ("After Changed showAd called " + sceneValue);

			PlaceholderName placeholderName = (PlaceholderName)sceneValue;

			switch (AdTypeDropDown.value) 
			{
			case 0:
				{
					Debug.Log ("IsInterstitialAvailable " + ConsoliAds.Instance.IsInterstitialAvailable (placeholderName));
				}
				break;
			case 1:
				{
					Debug.Log ("IsRewardedVideoAvailable " + ConsoliAds.Instance.IsRewardedVideoAvailable (placeholderName));
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
		int sceneValue = SceneNameDropDown.value;
		Debug.Log ("showAd called " + sceneValue);

		if (sceneValue == 0) {
			sceneValue = 27;
		}

		Debug.Log ("After Changed showAd called " + sceneValue);

		PlaceholderName placeholderName = (PlaceholderName)sceneValue;


		if (ConsoliAds.Instance.IsInitialized) {

			switch (AdTypeDropDown.value) 
			{
				case 0:
					{
					ConsoliAds.Instance.ShowInterstitial(placeholderName);
					}
				break;
				case 1:
					{
					ConsoliAds.Instance.ShowRewardedVideo(placeholderName);
					}
				break;
				case 2:
					{
					ConsoliAds.Instance.ShowBanner( BannerAdsManager.Instance.simpleBannerView , placeholderName );
					}
				break;
			case 3:
				{
					ConsoliAds.Instance.ShowNativeAd (nativeAdGameObject , placeholderName);
				}
				break;
			case 4:
				{
					ConsoliAds.Instance.ShowIconAd (iconAdGameObject , IconAnimationType.PULSE , true , placeholderName);
				}
				break;
			case 5:
				{
					ConsoliAds.Instance.ShowBanner( BannerAdsManager.Instance.customPositionBannerView ,placeholderName);
				}
				break;
			case 6:
				{
					ConsoliAds.Instance.ShowBanner(BannerAdsManager.Instance.customSizeBannerView , placeholderName );
				}
				break;
			case 7:
				{
					ConsoliAds.Instance.ShowBanner(BannerAdsManager.Instance.fullCustomBannerView , placeholderName);
				}
				break;
			case 8:
				{
					ConsoliAds.Instance.ShowBanner(BannerAdsManager.Instance.pendingBannerView , placeholderName);
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

		int sceneValue = SceneNameDropDown.value;
		Debug.Log ("RequestAd called " + sceneValue);

		if (sceneValue == 0) {
			sceneValue = 27;
		}

		Debug.Log ("After Changed RequestAd called " + sceneValue);

		PlaceholderName placeholderName = (PlaceholderName)sceneValue;

		if (ConsoliAds.Instance.IsInitialized) {
			
			switch (AdTypeDropDown.value) {
				case 0:
				{
					ConsoliAds.Instance.LoadInterstitial();
				}
				break;
				case 1:
					{
						ConsoliAds.Instance.LoadRewarded();
					}
				break;
				case 2:
				{
					ConsoliAds.Instance.HideBanner(BannerAdsManager.Instance.simpleBannerView);
				}
				break;
				case 3:
				{
						ConsoliAds.Instance.HideNative();
				}
				break;
				case 4:
				{
						ConsoliAds.Instance.DestoryIconAd(iconAdGameObject);
				}
				break;
				case 5:
				{
					ConsoliAds.Instance.HideBanner(BannerAdsManager.Instance.customPositionBannerView);
				}
				break;
			case 6:
				{
					ConsoliAds.Instance.HideBanner(BannerAdsManager.Instance.customSizeBannerView);
				}
				break;
			case 7:
				{
					ConsoliAds.Instance.HideBanner(BannerAdsManager.Instance.fullCustomBannerView);
				}
				break;
			case 8:
				{
					ConsoliAds.Instance.HideBanner(BannerAdsManager.Instance.pendingBannerView);
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
			Debug.Log (ex.ToString ());
		}
		ConsoliAds.Instance.addAdmobTestDevice (deviceId);
	}

	void onConsoliAdsInitialization()
	{
		Debug.Log("Sample: onConsoliAdsInitialization called ");
	}

	void onInterstitialAdLoaded()
	{
		Debug.Log("Sample: onInterstitialAdLoaded called for scene : ");
	}

	void onInterstitialAdFailToLoad()
	{
		Debug.Log("Sample: onInterstitialAdFailToLoad called for scene : ");
	}

	void onInterstitialAdShown(PlaceholderName placeholderName)
	{
		Debug.Log("Sample: onInterstitialAdShown called for scene : " + placeholderName);
	}

	void onInterstitialAdFailedToShow(PlaceholderName placeholderName)
	{
		Debug.Log("Sample: onInterstitialAdFailedToShow called for scene : " + placeholderName);
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

	void onRewardedVideoAdClick()
	{
		Debug.Log("Sample: onRewardedVideoAdClick called");
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

	void onNativeAdClicked()
	{
		Debug.Log("Sample: onNativeAdClicked called : ");
	}

}
