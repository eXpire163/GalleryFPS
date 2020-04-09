using Assets;
using Assets.Google;
using System.Collections;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

   
    void Start()
    {
        Debug.Log("starting rommManger");
        StartCoroutine(Mmyinit());
        
    }

    IEnumerator Mmyinit() {
        Debug.Log("init rommManger");
        GooglePhotos googlePhotos = new GooglePhotos();

        clsResponseRootObject urls = null;
        yield return googlePhotos.GetAlbumAsync(value => urls = value);

        Debug.Log(urls);
        updateFrames(urls);
    }

    void updateFrames(clsResponseRootObject urls) {
        GameObject[] frames = GameObject.FindGameObjectsWithTag("GalleryTag");
        for (int i = 0; i < frames.Length; i++)
        {
            // frames[i].GetComponent<WWWImage>().setUrl(bucket+urls[i%urls.Length]);
            if(i<urls.mediaItems.Count)
                frames[i].GetComponent<WWWImage>().setUrl(urls.mediaItems[i].baseUrl);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    
}
