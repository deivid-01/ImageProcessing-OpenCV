using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
public class Segmentation : MonoBehaviour
{
    public RawImage rawImage;
    public Texture2D texture;

    
    public int idxLabel;


    private void Start()
    {
        Mat mat = OpenCvSharp.Unity.TextureToMat(texture);
        Cv2.CvtColor(mat, mat, ColorConversionCodes.BGR2GRAY);

        AppliesSegmentation(ref mat);
    
        rawImage.texture = OpenCvSharp.Unity.MatToTexture(mat);
    }

    void AppliesSegmentation(ref Mat mat )
    {
        Mat dst = new Mat(new Size(mat.Width, mat.Height), MatType.CV_8U); // create destiny mat

        int totalLabels = Cv2.ConnectedComponents(mat, dst, PixelConnectivity.Connectivity4); //Applies segmentation

        if (!CheckLabels(totalLabels)) return;  // 

         FilterLabelInMat(ref mat,dst);

  
    }

    void FilterLabelInMat(ref Mat mat, Mat dst)
    {
        for (int i = 0; i < dst.Rows; i++)
        {
            for (int j = 0; j < dst.Cols; j++)
            {
                int[] indx = new int[2] { i, j };
                if (dst.At<byte>(indx[0], indx[1]) != idxLabel) //Filter label in mat
                {
                    mat.Set<byte>(indx, 0);
                }

            }
        }
    }


    bool CheckLabels(int num)
    {
        if (idxLabel >= num)
        {
            print("Label doesn't exits");
            return false ;
        }
        return true;
           
    }
}
