using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using OpenCvSharp;
public class OpenCv_ObjDetector1Umbral : MonoBehaviour
{
    WebCamTexture webCamTexture;


    private void Start()
    {
       

        WebCamDevice[] devices = WebCamTexture.devices;

        webCamTexture = new WebCamTexture(devices[0].name);

        webCamTexture.Play();

        
     

        StartCoroutine(WaitMen());
         

        //GetComponent<Renderer>().material.mainTexture = OpenCvSharp.Unity.MatToTexture(Binarize(OpenCvSharp.Unity.TextureToMat(webCamTexture)));


    }

    private void Update()
    {
   
        GetComponent<Renderer>().material.mainTexture = OpenCvSharp.Unity.MatToTexture(Binarize(OpenCvSharp.Unity.TextureToMat(webCamTexture)));
    }

    IEnumerator WaitMen()
    {
        yield return new WaitForSeconds(1);

        

      //  snapshot = Binarize(snapshot);

        //GetComponent<Renderer>().material.mainTexture = OpenCvSharp.Unity.MatToTexture(snapshot);

    }

    Mat Binarize(Mat mat)
    {
        Cv2.CvtColor(mat, mat, ColorConversionCodes.BGR2GRAY);
        Cv2.Threshold(mat, mat, 250, 255, ThresholdTypes.Binary);

        return mat;

    }
}
