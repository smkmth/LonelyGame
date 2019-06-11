using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameCamera : MonoBehaviour {

    public float CameraRadius;
    public float CameraRange;
    public LayerMask clueLayerMask;
    public Texture2D photoTex;
    public GameObject photoTargetObject;
    public float photoDisplayTime;
    public Sprite emptySprite;
    public PhotoLibrary photoLibrary;

    public void Start()
    {
        photoLibrary = GetComponent<PhotoLibrary>();
    }

    void Update () {

        if (Input.GetButtonDown("TakePhoto"))
        {
            RaycastHit[] targets = Physics.SphereCastAll(transform.position, CameraRadius, transform.forward, CameraRange,clueLayerMask );
            foreach(RaycastHit hit in targets)
            {
                Clue clue = hit.collider.gameObject.GetComponent<Clue>();
                if (!clue.foundClue)
                {
                    Debug.Log("target");
                    StartCoroutine(TakePhoto());
                    clue.foundClue = true;
                }

            }
        }
        
        if (Input.GetButtonDown("ViewPhotos"))
        {
            photoLibrary.TogglePhotoLibrary();
        }
        
		
	}

    IEnumerator TakePhoto()
    {
        yield return new WaitForEndOfFrame();
        Time.timeScale = 0.0f;
        photoTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        photoTex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        photoTex.Apply();
        Sprite photoSprite = Sprite.Create(photoTex, new Rect(0, 0, photoTex.width, photoTex.height), new Vector2(0.5f, 0.5f));
        photoLibrary.takenPhotoSprites.Add(photoSprite);
        photoTargetObject.GetComponent<Image>().sprite = photoSprite;
        yield return new WaitForSecondsRealtime(photoDisplayTime);
        photoTargetObject.GetComponent<Image>().sprite = emptySprite;
        Time.timeScale = 1.0f;
    }
}
