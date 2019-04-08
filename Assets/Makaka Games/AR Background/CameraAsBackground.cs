// =========================
// MAKAKA GAMES - MAKAKA.ORG
// =========================

using UnityEngine;
using UnityEngine.UI;

[HelpURL("https://makaka.org/category/docs")]
[AddComponentMenu ("AR/CameraAsBackground")]
public class CameraAsBackground : MonoBehaviour 
{
	private RawImage rawImage;
	private WebCamTexture webCamTexture;
	private AspectRatioFitter aspectRatioFitter;

	private float minimumWidthForOrientation = 100;
	private float EulerAnglesOfPI = 180f;
	
	private Rect uvRectForVideoVerticallyMirrored = new Rect(1f, 0f, -1f, 1f);	
	private Rect uvRectForVideoNotVerticallyMirrored = new Rect(0f, 0f, 1f, 1f);

	private float currentCWNeeded;
	private float currentAspectRatio;

	private Vector3 currentLocalEulerAngles = Vector3.zero;


	void Awake()
	{
		aspectRatioFitter = GetComponent<AspectRatioFitter>();
		rawImage = GetComponent<RawImage>();

		try
		{
			Application.RequestUserAuthorization(UserAuthorization.WebCam);

			if (WebCamTexture.devices.Length == 0)
			{
				Debug.LogWarning("No 🎥 cameras found");
			}
			else
			{
				// Get Main Camera == Back Camera
				webCamTexture = new WebCamTexture (Screen.width, Screen.height);
				//webCamTexture.filterMode = FilterMode.Trilinear;
				Play();

				rawImage.texture = webCamTexture;
				rawImage.material.SetTextureScale("_MainTex", new Vector2(1f, 1f));
			}
		}
		catch (System.Exception e)
		{
			Debug.LogWarning("Camera 🎥 is not available: " + e);
		}
	}
	
	void Update ()
	{
		SetOrientationUpdate();
	}

	public void SetOrientationUpdate()
	{
		if (webCamTexture)
		{
			if (webCamTexture.width < minimumWidthForOrientation) 
			{
				return;
			}

			currentCWNeeded = -webCamTexture.videoRotationAngle;

			if (webCamTexture.videoVerticallyMirrored) 
			{
				currentCWNeeded += EulerAnglesOfPI;
			}

			currentLocalEulerAngles.z = currentCWNeeded;
			rawImage.rectTransform.localEulerAngles =  currentLocalEulerAngles;

			currentAspectRatio = (float) webCamTexture.width / (float) webCamTexture.height;
			aspectRatioFitter.aspectRatio = currentAspectRatio;

			if (webCamTexture.videoVerticallyMirrored) 
			{
				rawImage.uvRect =  uvRectForVideoVerticallyMirrored;
			}
			else
			{
				rawImage.uvRect =  uvRectForVideoNotVerticallyMirrored;
			}
		}
	}

	public WebCamTexture GetWebCamTexture()
	{
		return webCamTexture;
	}

	public void Play()
	{
		if (webCamTexture)
		{
			webCamTexture.Play();
		}
	}

	public void Stop()
	{
		if (webCamTexture)
		{
			webCamTexture.Stop();
		}
	}

	public void ChangeResolutionAndPlay(float factor)
	{
		Stop();

		webCamTexture.requestedWidth = Mathf.RoundToInt(webCamTexture.requestedWidth * factor);
		webCamTexture.requestedHeight = Mathf.RoundToInt(webCamTexture.requestedHeight * factor);
		
		Play();
	}

	void OnDestroy()
	{
		Stop();
	}
}