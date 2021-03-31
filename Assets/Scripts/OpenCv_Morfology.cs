using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
using System;


public class OpenCv_Morfology : MonoBehaviour
{

    WebCamTexture webCamtexture;
    
    public RawImage rawImage;

    public Texture2D texture;

    void Start()
    {
        #region Camera enabled
        /**
        WebCamDevice[] devices = WebCamTexture.devices;
        webCamtexture = new WebCamTexture(devices[0].name);
        webCamtexture.Play();
        
        rawImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(webCamtexture.width, webCamtexture.height);
        rawImage.texture = webCamtexture;
        **/
        #endregion
        //Change  size
        rawImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width, texture.height);

        Mat mat = OpenCvSharp.Unity.TextureToMat(texture);

        //Structural Elements
        Mat strElem = GetStructuringElement("rect");
        Mat strElmen2 = GetStructuringElement("line");

        rawImage.texture = texture;

        //Dilate
        // StartCoroutine(Dilate(mat, strElem, 20));

        //Erode
        // StartCoroutine(Erode(mat, strElem, 20));


        Cv2.MorphologyEx(mat, mat, MorphTypes.Close,strElem,iterations:16); // erode(dilate()) | Delate circle
        Cv2.MorphologyEx(mat, mat, MorphTypes.Open,strElem,iterations:16); // dilate(erode())  | Delate protuberancia

        rawImage.texture = OpenCvSharp.Unity.MatToTexture(mat);

    }

    IEnumerator Dilate(Mat mat,Mat strElem, int numDilations, int actual = 0)
    {
        
       
        yield return new WaitForSeconds(0.1f);
        Cv2.Dilate(mat, mat, strElem, iterations: 1);

        rawImage.texture = OpenCvSharp.Unity.MatToTexture(mat);

        if (actual < numDilations)
        {
            StartCoroutine(Dilate(mat, strElem, numDilations, actual + 1));
        }



    }

    IEnumerator Erode(Mat mat, Mat strElem, int numDilations, int actual = 0)
    {


        yield return new WaitForSeconds(0.1f);

        Cv2.Erode(mat, mat, strElem, iterations: 1);

        rawImage.texture = OpenCvSharp.Unity.MatToTexture(mat);

        if (actual < numDilations)
        {
            StartCoroutine(Erode(mat, strElem, numDilations, actual + 1));
        }


    }



    Mat GetStructuringElement(string typeStr)
    {
        if (typeStr.Equals("line"))
        {
            Mat strTest = Mat.Eye(new Size(5, 5), MatType.CV_8UC1);

            return strTest.Rotate(RotateFlags.Rotate90Clockwise);
        }
        else if (typeStr.Equals("rect"))
        {
            return Cv2.GetStructuringElement(MorphShapes.Rect, new Size(5, 5));
        }

        return null;
    }
        

}
