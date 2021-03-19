using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
public class OpenCv_MovDetection : MonoBehaviour
{
    public AnimationCurve plot = new AnimationCurve();

    WebCamTexture webCamTexture;

    public List<double> snapsValues = new List<double>(); 
    List<Mat> mats = new List<Mat>();

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        webCamTexture = new WebCamTexture(devices[0].name);

        webCamTexture.Play();
       // GetComponent<Renderer>().material.mainTexture = webCamTexture;
        StartCoroutine(BasicDetectorMovement());
    }

   
    // Update is called once per frame

    IEnumerator BasicDetectorMovement()
    {
        yield return new WaitForSeconds(0.1f);


        if (mats.Count < 2)
        {

            mats.Add(OpenCvSharp.Unity.TextureToMat(webCamTexture));
           
            if (mats.Count == 2)
            {
                Mat result = mats[0] - mats[1];
                Scalar sum = result.Sum();
                if ((sum.Val0 + sum.Val1 + sum.Val2) > 10000000)
                {
                    print("TARGET IS MOVING");
                    GetComponent<Renderer>().material.color = Color.red;// OpenCvSharp.Unity.MatToTexture(result);
                }
                else {
                    GetComponent<Renderer>().material.color = Color.black;
                }
                    
                    //snapsValues.Add((sum.Val0 + sum.Val1 + sum.Val2));
                   // plot.AddKey(snapsValues.Count,(float) snapsValues[snapsValues.Count-1]);

               
                mats = new List<Mat>();
                
            }

        }

        StartCoroutine(BasicDetectorMovement());



    }
}
