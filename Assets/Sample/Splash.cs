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

	void Start ()
	{
	}

	public void InitializeButtonPressed()
	{
		bool userConsent = UserConsentToggle.isOn;
		ConsoliAds.Instance.initialize(userConsent);
		initButton.enabled = false;// (false);
		UserConsentToggle.enabled = false;
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
