using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Image;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class SlideshowFrame : UdonSharpBehaviour
{
    [SerializeField, Tooltip("URLs of images to load")]
    private VRCUrl[] imageUrls;
    
    [SerializeField, Tooltip("URL of text file containing captions for images, one caption per line.")]
    private VRCUrl stringUrl;
    
    [SerializeField, Tooltip("Renderer to show downloaded images on.")]
    private new Renderer renderer;
    
    [SerializeField, Tooltip("Text field for captions.")]
    private Text field;
    
    [SerializeField, Tooltip("Duration in seconds until the next image is shown.")]
    private float slideDurationSeconds = 10f;
    
    private int _loadedIndex = -1;
    private VRCImageDownloader _imageDownloader;
    private IUdonEventReceiver _udonEventReceiver;
    private string[] _captions = new string[0];
    private Texture2D[] _downloadedTextures;

    [SerializeField] private Transform quadTransform;
    [SerializeField] private Transform frameTransform;
    [SerializeField] private float frameWidth;

    private void Start()
    {
        // Downloaded textures will be cached in a texture array.
        _downloadedTextures = new Texture2D[imageUrls.Length];
        
        // It's important to store the VRCImageDownloader as a variable, to stop it from being garbage collected!
        _imageDownloader = new VRCImageDownloader();
        
        // To receive Image and String loading events, 'this' is casted to the type needed
        _udonEventReceiver = (IUdonEventReceiver)this;
        
        // Captions are downloaded once. On success, OnImageLoadSuccess() will be called.
        VRCStringDownloader.LoadUrl(stringUrl, _udonEventReceiver);
        
        // Load the next image. Then do it again, and again, and...
        LoadNextRecursive();
    }

    public void LoadNextRecursive()
    {
        LoadNext();
        SendCustomEventDelayedSeconds(nameof(LoadNextRecursive), slideDurationSeconds);
    }
    
    private void LoadNext()
    {
        // All clients share the same server time. That's used to sync the currently displayed image.
        _loadedIndex = (int)(Networking.GetServerTimeInMilliseconds() / 1000f / slideDurationSeconds) % imageUrls.Length;

        var nextTexture = _downloadedTextures[_loadedIndex];
        
        if (nextTexture != null)
        {
            // Image already downloaded! No need to download it again.
            renderer.sharedMaterial.mainTexture = nextTexture;

            ResizeQuadToTex(nextTexture);
            
        } 
        else
        {
            var rgbInfo = new TextureInfo();
            rgbInfo.GenerateMipMaps = true;
            _imageDownloader.DownloadImage(imageUrls[_loadedIndex], renderer.material, _udonEventReceiver, rgbInfo);
        }
        
        UpdateCaptionText();
    }

    private void ResizeQuadToTex(Texture2D texture) 
    {
        float ratio = (float)texture.width / texture.height;
        float y = 1.21818f;
        float x = y * ratio;
        Vector3 size = new Vector3(x, y, 1);
        quadTransform.localScale = size;
        Vector3 original = new Vector3(2.0303f, 2.0303f, 0.3f);


        frameTransform.localScale = new Vector3((size.x * 1.7f), (size.y * 1.7f), 0.3f);
    }

    private void UpdateCaptionText()
    {
        if (_loadedIndex < _captions.Length)
        {
            field.text = _captions[_loadedIndex];
        }
        else
        {
            field.text = "";
        }
    }

    public override void OnStringLoadSuccess(IVRCStringDownload result)
    {
        _captions = result.Result.Split('\n');
        UpdateCaptionText();
    }

    public override void OnStringLoadError(IVRCStringDownload result)
    {
        Debug.LogError($"Could not load string {result.Error}");
    }

    public override void OnImageLoadSuccess(IVRCImageDownload result)
    {
        Debug.Log($"Image loaded: {result.SizeInMemoryBytes} bytes.");
        
        _downloadedTextures[_loadedIndex] = result.Result;

        ResizeQuadToTex(_downloadedTextures[_loadedIndex]);
    }

    public override void OnImageLoadError(IVRCImageDownload result)
    {
        Debug.Log($"Image not loaded: {result.Error.ToString()}: {result.ErrorMessage}.");
    }

    private void OnDestroy()
    {
        _imageDownloader.Dispose();
    }
}