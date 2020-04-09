using Assets;
using Assets.Google;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{

    GooglePhotos google;

    public GameObject buttonGrid;
    public Button buttonTemplate;

    private void Start()
    {
        google = new GooglePhotos();
       clsResponseAlbum albs = google.getAlbumList();
        setupButtons(albs.albums);
    }

    private void setupButtons(List<Album> albums)
    {
        for (int i = 1; i < albums.Count; i++)
        {
            Button button = Instantiate(buttonTemplate,buttonGrid.transform);
            Album a = albums[i];
            button.name = albums[i].title+ a.mediaItemsCount;
            button.GetComponentInChildren<Text>().text = albums[i].title;
            button.onClick.AddListener(delegate { loadAlbum(a.id, a.mediaItemsCount); });
            

        }
    }
       

    public static string ALBUM_ID = "AC-ZXIR77Va9XRPRAFedco7fWwKhOEYfyvNua-nKkz0UIdZRi2P8MY-LI8dESKP-bvOitA_3GyNP";
    public static int MEDIA_COUNT = 8;
    public void loadAlbum(string albumId, int count) {
        Debug.Log(albumId);
        ALBUM_ID = albumId;
        MEDIA_COUNT = count;
        SceneManager.LoadScene("SampleScene");

    }
}
