using UnityEngine;
using System.Collections;

public class BannerAdsManager : MonoBehaviour {

    private static BannerAdsManager _instance;

    public static BannerAdsManager Instance { get { return _instance; } }

	public ConsoliAdsBannerView pendingBannerView;

	public ConsoliAdsBannerView simpleBannerView;

	public ConsoliAdsBannerView customPositionBannerView;

	public ConsoliAdsBannerView customSizeBannerView;

	public ConsoliAdsBannerView fullCustomBannerView;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

	void Start ()
	{
		pendingBannerView = new ConsoliAdsBannerView ();

		simpleBannerView = new ConsoliAdsBannerView ();

		AdPosition position = new AdPosition (50 , 50 );
		customPositionBannerView = new ConsoliAdsBannerView (position);

		AdSize size = new AdSize (300 , 300);
		customSizeBannerView = new ConsoliAdsBannerView (size);

		position.x = 100;
		position.y = 100;
		fullCustomBannerView = new ConsoliAdsBannerView (size , position);
	}

}
