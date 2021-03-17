using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
public class FaceDetector : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject bird;

    public int offsetY;
    WebCamTexture _webCamTexture;
    CascadeClassifier cascade;
    OpenCvSharp.Rect Myface;

    float lastY = 0;


    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;


        _webCamTexture = new WebCamTexture(devices[0].name);
        _webCamTexture.Play();

        cascade = new CascadeClassifier(Application.dataPath + "/OpenCV+Unity/Demo/Face_Detector/haarcascade_frontalface_default.xml");
    }

    // Update is called once per frame
    void Update()
    {
     

        Mat frame = OpenCvSharp.Unity.TextureToMat(_webCamTexture);
        if (frame is null)
        {
            Debug.Log("Is null");        
        }
        FindNewFace(frame);
        display(frame);
    }

    void FindNewFace(Mat frame)
    {
     
        var faces = cascade.DetectMultiScale(frame, 1.1, 2, HaarDetectionType.ScaleImage);

        if (faces.Length >= 1)
        {
           
            //Debug.Log(Camera.main.ScreenToViewportPoint(new Vector3(faces[0].Location.X, faces[0].Location.Y,0)));
            Myface = faces[0];
             Vector3  desirePosition=  Camera.main.ScreenToWorldPoint(new Vector3(faces[0].Location.X,faces[0].Location.Y,0));
            //  desirePosition.z = -10;
            desirePosition.y = -1*desirePosition.y - offsetY;
           
            bird.transform.position = new Vector3(bird.transform.position.x,Mathf.Clamp(desirePosition.y,-3,3), -10);
  
            //Debug.Log($" {bird.transform.position.y}");
            lastY = faces[0].Y;
        }
    }

    void display(Mat frame)
    {
        if (Myface != null)
        {
            frame.Rectangle(Myface, new Scalar(250, 0, 0), 2);
        }
        Texture newTexture = OpenCvSharp.Unity.MatToTexture(frame);
        GetComponent<Renderer>().material.mainTexture = newTexture;
    }
    
}
