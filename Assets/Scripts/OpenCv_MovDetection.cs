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
     /*   
        Mat result = OpenCvSharp.Unity.TextureToMat(webCamTexture);
     

        Mat mat = new Mat(result.Height, result.Width, MatType.CV_8UC3, new Scalar(0, 0, 0));
    

         mat[new OpenCvSharp.Rect(100, 100, 50, 50)].SetTo(Scalar.All(255));

 


        for (int i = 0; i < mat.Height; i++)
        {
            for (int j = 0; j < mat.Width; j++)
            {
                if (mat.At<Vec3b>(new int[2] { i, j }).Item0 == 255)
                    
                    continue;
                Vec3b vec = new Vec3b(100, 100, 100);
                result.Set<Vec3b>(new int[2] { i, j }, vec);

            }
        }

       

        GetComponent<Renderer>().material.mainTexture = OpenCvSharp.Unity.MatToTexture(result);
        
        */
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

        Mat original = OpenCvSharp.Unity.TextureToMat(texture);
        Mat result = OpenCvSharp.Unity.TextureToMat(texture); //new Mat(720, 1200, MatType.CV_8UC3, new Scalar(0, 0, 0));
      
        Mat mat = new Mat(result.Height, result.Width, MatType.CV_8UC3, new Scalar(0, 0, 0));
      
        mat[new OpenCvSharp.Rect(result.Width/2, result.Height/2, 100, 100)].SetTo(Scalar.All(255));

        for (int i = 0; i < mat.Height; i++)
        {
            for (int j = 0; j < mat.Width; j++)
            {
                if (mat.At<Vec3b>(new int[2] { i, j })[0] != 255)
                {

                    //mat.Set<int>(new int[2] { i, j }, 255);
                    Vec3b vec = new Vec3b(0, 0, 0);
                    result.Set<Vec3b>(new int[2] { i, j }, vec);


                }
                else
                {
                    result[new OpenCvSharp.Rect(i,j, 1, 1)].SetTo(Scalar.All(0));

                    //result.Set<Vec3b>(new int[2] { i, j }, original.At<Vec3b>(new int[2] { i, j }));
                }
              

            }
        }


        


         GetComponent<Renderer>().material.mainTexture = OpenCvSharp.Unity.MatToTexture(mat);
        Debug.Log("oliwis");
    }
}
