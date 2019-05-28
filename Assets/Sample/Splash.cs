using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Splash : MonoBehaviour
{
	bool isInitialized = false;
	public Toggle UserConsentToggle;
	float WAIT_TIME = 3.0f;

	[Header("UI Buttons")]
	public Button initButton;
	public InputField waitTimeField;

	void Start ()
	{
		waitTimeField.text = WAIT_TIME + "";
	}

	public void InitializeButtonPressed()
	{
		try {
			WAIT_TIME = float.Parse(waitTimeField.text);
		}
		catch (Exception ex) {
		}

		bool userConsent = UserConsentToggle.isOn;
		ConsoliAds.Instance.initialize(userConsent);
		initButton.enabled = false;// (false);
		UserConsentToggle.enabled = false;
		Invoke ("LevelLoad" , WAIT_TIME);
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
		isInitialized = true;
		Debug.Log("Splash: onConsoliAdsInitialization called ");
	}
}
