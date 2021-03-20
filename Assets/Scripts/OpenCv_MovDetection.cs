using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
public class OpenCv_MovDetection : MonoBehaviour
{
    public AnimationCurve plot = new AnimationCurve();

    WebCamTexture webCamTexture;
    public Texture2D texture;

    public List<double> snapsValues = new List<double>(); 
    List<Mat> mats = new List<Mat>();
    Mat mask = new Mat();
    OpenCvSharp.Rect sub = new OpenCvSharp.Rect();
    OpenCvSharp.Rect sub2 = new OpenCvSharp.Rect();

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        webCamTexture = new WebCamTexture(devices[0].name);

        webCamTexture.Play();

        CreateMask();
        
        //StartCoroutine(BasicDetectorMovement());
    }


    private void Update()
    {

        //Apply target zone to webcamTexture
        mask[sub] = OpenCvSharp.Unity.TextureToMat(webCamTexture)[sub];
        mask[sub2] = OpenCvSharp.Unity.TextureToMat(webCamTexture)[sub2];
        
        //Update texture

        GetComponent<Renderer>().material.mainTexture = OpenCvSharp.Unity.MatToTexture(mask);
        
      
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
                    GetComponent<Renderer>().material.mainTexture = OpenCvSharp.Unity.MatToTexture(result);
                }
                
            
               
                    
                    //snapsValues.Add((sum.Val0 + sum.Val1 + sum.Val2));
                   // plot.AddKey(snapsValues.Count,(float) snapsValues[snapsValues.Count-1]);

               
                mats = new List<Mat>();
                
            }

        }

        StartCoroutine(BasicDetectorMovement());


    }

    void CreateMask()
    {
       //Set all mask to black
        mask = new Mat(webCamTexture.height, webCamTexture.width, MatType.CV_8UC3, new Scalar(0, 0, 0));
            
        // Get rectangle of target zone
        sub = new OpenCvSharp.Rect(webCamTexture.width / 2 + 100, webCamTexture.height / 2 - 100, 250, 250);
        sub2 = new OpenCvSharp.Rect(webCamTexture.width / 2 - 400, webCamTexture.height / 2 - 100, 250, 250);

    }
}
