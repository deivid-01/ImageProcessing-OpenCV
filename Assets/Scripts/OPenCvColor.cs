using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
public class OPenCvColor : MonoBehaviour
{
    public Image[] images;

    public Texture2D texture;
    void Start()
    {
        Mat matRGB = OpenCvSharp.Unity.TextureToMat(texture);

        //Sow all color types
        images[0].sprite = CreateNewSprite(GetAllColorTypes(matRGB));

        //Filtering license plata
        /*
        Mat matLab = new Mat();
        Cv2.CvtColor(matRGB, matLab, ColorConversionCodes.BGR2Lab);
        matLab = matLab.ExtractChannel(2);
        Cv2.Threshold(matLab, matLab, 180, 255, ThresholdTypes.Binary);

        
        //Set values of matRGb to zero where matLab is zero
        for (int i = 0; i < matLab.Rows; i++)
        {
            for (int j = 0; j < matLab.Cols; j++)
            {
                if (matLab.At<byte>(new int[] { i, j }) == 0)
                {

                    matRGB.Set<Vec3b>(new int[] { i, j }, new Vec3b(0,0,0));

                }
            }
        }
        
        images[0].sprite = CreateNewSprite(matRGB);

        */


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
        dst =  res.Rotate(RotateFlags.Rotate90Clockwise);

        


    }
    void   AddToRight(ref  Mat main, Mat newMat)
    {
        main = main.Rotate(RotateFlags.Rotate90Clockwise);
        main.Add(newMat.Rotate(RotateFlags.Rotate90Clockwise));

        main =  main.Rotate(RotateFlags.Rotate90CounterClockwise);
       
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
}
