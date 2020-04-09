using Assets.Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets
{


    public class GooglePhotos
    {

        // private static readonly HttpClient client = new HttpClient();


        string UserName = "eXpire163";
        string ClientID = "eXpire163";
        string ClientSecret = "4/ygEbv9-ts1GFSj3KoCT570hd1dHqQ_fYrrrWW9BVUftGpg2qdaX1dL-5W8nmEepfw-2CGif8lSKXptijULjExuU";
        UserCredential credential;

        public GooglePhotos()
        {
            string credPath = @"StorePath\";

            string[] scopes = {
            //"https://www.googleapis.com/auth/photoslibrary.sharing"
            "https://www.googleapis.com/auth/photoslibrary.readonly"
               // "https://www.googleapis.com/auth/photoslibrary"
         };

            using (var stream = new FileStream("Assets/credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    UserName,
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }


        }


        public IEnumerator GetAlbumAsync(System.Action<clsResponseRootObject> result)
        {
            var values = new Dictionary<string, string>{
                { "pageSize", "32" },
                { "albumId", LoadManager.ALBUM_ID }
            };


            UnityWebRequest request = new UnityWebRequest("https://photoslibrary.googleapis.com/v1/mediaItems:search", "POST");
            UploadHandler uploadHandler = (UploadHandler)new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(values)));
            request.uploadHandler = uploadHandler;
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + credential.Token.AccessToken);

            yield return request.SendWebRequest();
            // Debug.Log(request.downloadHandler.text);

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
            result(JsonConvert.DeserializeObject<clsResponseRootObject>(request.downloadHandler.text));


        }
        int reauthTry = 0;
        public clsResponseAlbum getAlbumList()
        {
            if (reauthTry > 3) {
                throw new Exception("could not reauth");
            }
            clsResponseRootObject responseObject = new clsResponseRootObject();

            String requestUrl = "https://photoslibrary.googleapis.com/v1/albums?pageSize=20";
            HttpWebRequest httpWebRequest = createRequest(requestUrl);
            try
            {
                HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    String readerText = reader.ReadToEnd();
                    Debug.Log(readerText);
                    reauthTry = 0;
                    return JsonConvert.DeserializeObject<clsResponseAlbum>(readerText);
                }
            }
            catch (WebException we) {
                Debug.Log(we);

                reauthTry++;
                GoogleWebAuthorizationBroker.ReauthorizeAsync(credential, CancellationToken.None).Wait();
                Debug.Log("Reauth OK");
               return  getAlbumList();


            }
            return null;
        }



        private HttpWebRequest createRequest(string requestUrl)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            //  HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://photoslibrary.googleapis.com/v1/mediaItems");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("client_id", ClientID);
            httpWebRequest.Headers.Add("client_secret", ClientSecret);
            httpWebRequest.Headers.Add("Authorization:" + credential.Token.TokenType + " " + credential.Token.AccessToken);

            return httpWebRequest;
        }


    }

}


