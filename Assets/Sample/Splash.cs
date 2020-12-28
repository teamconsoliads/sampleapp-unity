using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Splash : MonoBehaviour
{
	public Toggle UserConsentToggle;

	[Header("UI Buttons")]
	public Button initButton;

	[Header("Input Fields:")]
	public InputField sceneIndextxtField;

	int sceneIndex;

	void Start ()
	{
		sceneIndex = 0;
	}

	public void InitializeButtonPressed()
	{
		bool userConsent = UserConsentToggle.isOn;
		ConsoliAds.Instance.initialize(userConsent);
		initButton.enabled = false;// (false);
		UserConsentToggle.enabled = false;
	}

	public void PendingButtonPressed()
	{
		if (sceneIndextxtField.text != "") {
			try {
				sceneIndex = int.Parse (sceneIndextxtField.text);
			}
			catch(Exception ex) {
				Debug.Log ("Unable to parse sceneIndex");
			}
		}
		InitializeButtonPressed ();
		ConsoliAds.Instance.ShowBanner ( BannerAdsManager.Instance.pendingBannerView , PlaceholderName.Default);
	}

	void OnDisable()
	{
		ConsoliAds.onConsoliAdsInitializationSuccess -= onConsoliAdsInitialization;	
	}

	void OnEnable()
	{
		SetupEvents();
	}

	void SetupEvents()
	{
		ConsoliAds.onConsoliAdsInitializationSuccess += onConsoliAdsInitialization;	
	}
	
	public void LevelLoad()
	{
		SceneManager.LoadScene("ConsoliAdsSample");
	}

	void onConsoliAdsInitialization()
	{
		Debug.Log("Splash: onConsoliAdsInitialization called ");
		LevelLoad();
	}
}
