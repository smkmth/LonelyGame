using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameCamera : MonoBehaviour
{
    public AudioSource spookyStingSource;

    public float CameraRadius;
    public float CameraRange;
    public LayerMask clueLayerMask;
    public Texture2D photoTex;
    public GameObject photoTargetObject;
    public float photoDisplayTime;
    public Sprite emptySprite;
    public PhotoLibrary photoLibrary;
    public Slider energyBar;
    public bool isCameraActive;
    private SpriteRenderer currentGhost;
    public int cameraShots;

    public void Start()
    {
        spookyStingSource = GetComponent<AudioSource>();
        photoLibrary = GetComponent<PhotoLibrary>();
        isCameraActive = true;

    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {

        }

        if (Input.GetButtonDown("TakePhoto"))
        {
            if (isCameraActive && cameraShots > 0)
            {
                RaycastHit[] targets = Physics.SphereCastAll(transform.position, CameraRadius, transform.forward, CameraRange, clueLayerMask);
                foreach (RaycastHit hit in targets)
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Clue"))
                    {
                        Clue clue = hit.collider.gameObject.GetComponent<Clue>();
                        if (clue.clueType != ClueType.PhotoTarget)
                        {
                            return;
                        }
                        if (!clue.foundClue)
                        {
                            clue.foundClue = true;
                            energyBar.value += clue.energyValue;
                        }
                    }
                    else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ghost"))
                    {
                        if (energyBar.value == 100)
                        {

                            currentGhost = hit.collider.gameObject.GetComponent<SpriteRenderer>();
                            currentGhost.enabled = true;
                            spookyStingSource.Play();
                        }
                    }
                }
                cameraShots--;
                StartCoroutine(TakePhoto());
            }

        }

        if (Input.GetButtonDown("ViewPhotos"))
        {
            photoLibrary.TogglePhotoLibrary();
            if (photoLibrary.isLibraryActive)
            {
                isCameraActive = false;
            }
            else
            {
                isCameraActive = true;

            }
        }


    }

    IEnumerator TakePhoto()
    {
        energyBar.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        Time.timeScale = 0.0f;
        Texture2D photoTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBAHalf , false);
        photoTex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        photoTex.Apply();
        Sprite photoSprite = Sprite.Create(photoTex, new Rect(0, 0, photoTex.width, photoTex.height), new Vector2(0.5f, 0.5f));
        photoLibrary.takenPhotoSprites.Add(photoSprite);
        photoTargetObject.GetComponent<Image>().sprite = photoSprite;
        yield return new WaitForSecondsRealtime(photoDisplayTime);
        photoTargetObject.GetComponent<Image>().sprite = emptySprite;
        Time.timeScale = 1.0f;
        energyBar.gameObject.SetActive(true);
        if (currentGhost != null)
        {
            currentGhost.enabled = false;
            currentGhost = null;
        }


    }
}
