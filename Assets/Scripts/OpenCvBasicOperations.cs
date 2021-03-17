using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using UnityEngine.UI;

public class OpenCvBasicOperations : MonoBehaviour
{
    public SpriteRenderer imgSR;
    public Texture2D img2Texture;
    void Start()
    {
        //Get Image Data
        Mat mat = OpenCvSharp.Unity.TextureToMat(imgSR.sprite.texture);
        Mat mat2 = OpenCvSharp.Unity.TextureToMat(img2Texture);

        //---------------------------------------------
        //Sum
        // Mat matR = mat + mat2;  //or Cv2.Sum(mat, mat2, dst, null,-1);
        //--------------------------------------------
        //Crop Image
        // Mat matR =mat[new OpenCvSharp.Rect(0, 0, mat.Width/2, mat.Height)]; 
        //-----------------------------------------
        //Get type of Mat
        //print(mat.Type().ToString());


        Mat[] channels = new Mat[3];

        //Separate each channel
        channels = Cv2.Split(mat);

        //channel [0] -> Blue
        //channel [1] -> Green
        //channel [2] -> Red
        
        //Set channel to Zero
        channels[2] = Mat.Zeros(mat.Rows, mat.Cols,MatType.CV_8UC1);

        //Merging Channels
        Cv2.Merge(channels,mat);



        //Update Sprite
        imgSR.GetComponent<SpriteRenderer>().sprite = CreateNewSprite(mat);


    }

    public Sprite CreateNewSprite(Mat dst) => Sprite.Create(OpenCvSharp.Unity.MatToTexture(dst), //Texture 
                                                new UnityEngine.Rect(0, 0, dst.Width, dst.Height),//Rect Propierties
                                                new Vector2(0.5f, 0.5f),                          //Offset
                                                100);                                             //Size


}
