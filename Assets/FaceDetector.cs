using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
public class FaceDetector : MonoBehaviour
{
    // Start is called before the first frame update

    WebCamTexture _webCamTexture;
    CascadeClassifier cascade;
    OpenCvSharp.Rect Myface;

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
        //GetComponent<Renderer>().material.mainTexture = _webCamTexture;
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
            Debug.Log(faces[0].Location);
            Myface = faces[0];
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
