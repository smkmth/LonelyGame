using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum PhotoNavigation
{
    previousPhoto,
    nextPhoto
}

public class PhotoLibrary : MonoBehaviour {

    public List<Sprite> takenPhotoSprites;
    public int currentPhotoIndex;
    public Image photoDisplayWindow;
    public Button previousPhotoButton;
    public Button nextPhotoButton;
    public GameObject photoLibrary;
    public bool isLibraryActive;
    public Sprite noPhotoSprite;
    public GameObject polaroidFrame;
    public GameObject blackoutPanel;
    

    // Use this for initialization
    void Start () {


        previousPhotoButton.onClick.AddListener(delegate
        {
            OnClickChangePhoto(PhotoNavigation.previousPhoto);
        });

        nextPhotoButton.onClick.AddListener(delegate
        {
            OnClickChangePhoto(PhotoNavigation.nextPhoto);
        });


    }

    public void TogglePhotoLibrary()
    {
        if (!isLibraryActive)
        {
            photoLibrary.SetActive(true);
            isLibraryActive = true;
            Cursor.lockState = CursorLockMode.None;
            polaroidFrame.SetActive(true);
            previousPhotoButton.gameObject.SetActive(true);
            nextPhotoButton.gameObject.SetActive(true);

        }
        else
        {
            photoLibrary.SetActive(false);
            isLibraryActive = false;
            Cursor.lockState = CursorLockMode.Locked;
            polaroidFrame.SetActive(false);

            previousPhotoButton.gameObject.SetActive(false);
            nextPhotoButton.gameObject.SetActive(false);



        }
        UpdateDisplayedPhoto();

    }
	

    public void OnClickChangePhoto(PhotoNavigation navigationDirection)
    {

        switch (navigationDirection)
        {
            case PhotoNavigation.nextPhoto :
                currentPhotoIndex++;
                break;

            case PhotoNavigation.previousPhoto:
                currentPhotoIndex--;

                break;
        }

        UpdateDisplayedPhoto();

        

    }

    public void UpdateDisplayedPhoto()
    {
        if (takenPhotoSprites.Count != 0)
        {
            currentPhotoIndex = currentPhotoIndex % takenPhotoSprites.Count;

            photoDisplayWindow.sprite = takenPhotoSprites[currentPhotoIndex];


            if (takenPhotoSprites.Count-1 == currentPhotoIndex)
            {
                nextPhotoButton.interactable = false;

            }
            else
            {
                nextPhotoButton.interactable = true;

            }
            if (currentPhotoIndex == 0)
            {
                previousPhotoButton.interactable = false;
            }
            else
            {
                previousPhotoButton.interactable = true;

            }

        }
        else
        {
            photoDisplayWindow.sprite = noPhotoSprite;
            nextPhotoButton.interactable = false;
            previousPhotoButton.interactable = false;


        }







    }
}
