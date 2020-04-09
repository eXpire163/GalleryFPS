using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WWWImage : MonoBehaviour
{

    Renderer renderer;
    public Texture texture;
    public Material material;

    public GameObject panel;

    private IEnumerator job;

    public void setUrl(string url)
    {

        job = updateImage(url);
        StartCoroutine(job);

      
    }
    IEnumerator updateImage(string url)
    {
      
        renderer = panel.GetComponent<Renderer>();
        WWW www = new WWW(url);

        while (!www.isDone)
            yield return null;
        Debug.Log(url);

        //renderer.material.SetTexture("www", www.texture);
        texture = www.texture;

       // Debug.Log(texture.width);
       // Debug.Log(texture.height);

        float ration = texture.width / texture.height;

        material = new Material(Shader.Find("Diffuse"));
        material.color = Color.white;
        material.mainTexture = texture;

        renderer.material = material;
        transform.localScale = new Vector3(ration * transform.localScale.x, transform.localScale.y, transform.localScale.z);

    }
}
