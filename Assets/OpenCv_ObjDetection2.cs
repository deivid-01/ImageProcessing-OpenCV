using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
public class OpenCv_ObjDetection2 : MonoBehaviour
{

    public GameObject imageGO;
    public Image image;
    WebCamTexture webCamTexture;
    public Texture2D texture;

    Mat mat = new Mat();
    Mat result = new Mat();

    void Start()
    {
        WebCamDevice[] webCamDevices = WebCamTexture.devices;
         webCamTexture = new WebCamTexture(webCamDevices[0].name);
         webCamTexture.Play();

        // 
       // Mat matRGB = OpenCvSharp.Unity.TextureToMat(texture);

        //Mat matLab = new Mat();
        //Cv2.CvtColor(matRGB, matLab, ColorConversionCodes.BGR2Lab);
        //matLab = matLab.ExtractChannel(2);
        //Cv2.Threshold(matLab, matLab, 167, 255, ThresholdTypes.Binary);

        //image.sprite = CreateNewSprite(matLab);
    }

    private void Update()
    {

        result = OpenCvSharp.Unity.TextureToMat(webCamTexture);
        Cv2.CvtColor(result, result, ColorConversionCodes.BGR2Lab);
        result = result.ExtractChannel(2);
        Cv2.Threshold(result, result, 167, 255, ThresholdTypes.Binary);

        imageGO.GetComponent<Renderer>().material.mainTexture = OpenCvSharp.Unity.MatToTexture(result);

    }

    public Sprite CreateNewSprite(Mat dst) => Sprite.Create(OpenCvSharp.Unity.MatToTexture(dst), //Texture 
                                            new UnityEngine.Rect(0, 0, dst.Width, dst.Height),//Rect Propierties
                                            new Vector2(0.5f, 0.5f),                          //Offset
                                            100);
    void SplitEachChannel(ref Mat dst)
    {
        dst = dst.Rotate(RotateFlags.Rotate90CounterClockwise);

        Mat res = dst.ExtractChannel(0);
        res.Add(dst.ExtractChannel(1));
        res.Add(dst.ExtractChannel(2));
        dst = res.Rotate(RotateFlags.Rotate90Clockwise);


    }

    Mat GetAllColorTypes(Mat matRGB)
    {
        Mat matHSV = new Mat();
        Mat matLab = new Mat();

        Cv2.CvtColor(matRGB, matHSV, ColorConversionCodes.BGR2HSV);
        Cv2.CvtColor(matRGB, matLab, ColorConversionCodes.BGR2Lab);

        SplitEachChannel(ref matRGB);
        SplitEachChannel(ref matHSV);
        SplitEachChannel(ref matLab);

        matRGB.Add(matHSV);
        matRGB.Add(matLab);

        return matRGB;
    }
    /**
     abaabaesundinosaurioqueviveenmicorazonycuandosehacegrandeeslindoyesponjosomasqueunpeluchitochititicoysabososiseñorsiseñor
     **/
}
