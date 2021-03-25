using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Linq;
using OpenCvSharp;
public class OpenCv_TextDetection : MonoBehaviour
{
    public AnimationCurve plot = new AnimationCurve();

    public SpriteRenderer sr;
    public Texture2D texture;

    [Range(1,5)]
    public int numLine;


    private void Start()
    {
        Mat mat = OpenCvSharp.Unity.TextureToMat(texture);
        print(mat.Type());

        mat = Binarize(mat);

        Proyection_Vertical(mat);

        mat = ExtractLine(mat,numLine);

        sr.sprite = CreateNewSprite(mat);

        // GenerateGraph();
    }

    Mat Binarize(Mat mat)
    {
        Cv2.CvtColor(mat, mat, ColorConversionCodes.BGR2GRAY);

        for (int i = 0; i < mat.Rows; i++)
        {
            for (int j = 0; j < mat.Cols; j++)
            {
                if (mat.At<byte>(new int[2] { i, j }) < 100)
                {

                    mat.Set<byte>(new int[2] { i, j }, 0);
                }
                else
                {
                    mat.Set<byte>(new int[2] { i, j }, 255);
                }

            }
        }

        return mat;

    }

    void Proyection_Vertical(Mat mat)
    {
        //Rotate
        Cv2.Rotate(mat, mat, RotateFlags.Rotate90CounterClockwise);
       
        // sum [:,i]

        List<int> proyectionV = new List<int>();

        for (int i = 0; i < mat.Cols; i++)
        {
            int sum = 0;

            for (int j = 0; j < mat.Rows; j++)
            {
                sum += mat.At<byte>(new int[2] { j, i });
            }
            plot.AddKey(i, sum);
            proyectionV.Add(sum);
        }


    }


    Mat ExtractLine(Mat mat, int numline)
    {
        Cv2.Rotate(mat, mat, RotateFlags.Rotate90Clockwise);
        OpenCvSharp.Rect line = new OpenCvSharp.Rect(0, 70+(100)*(numline-1), mat.Cols-2, 100);
        return mat[line];
    }

    public Sprite CreateNewSprite(Mat dst) => Sprite.Create(OpenCvSharp.Unity.MatToTexture(dst), //Texture 
                                            new UnityEngine.Rect(0, 0, dst.Width, dst.Height),//Rect Propierties
                                            new Vector2(0.5f, 0.5f),                          //Offset
                                            100);


    void GenerateGraph()
    {




        //Generate Graph

        float[] xValues = GenerateNumbers(0, 5); // Generate array of numbers between (0,5)
        float[] yValues = xValues.Select(x => x * x).ToArray(); //y = x^2

        foreach (var point in xValues.Zip(yValues,(x,y)=> new { x = x,y = y}))
        {
       // plot.AddKey(point.x, point.y); // Show in graph
        }
    }

    float[] GenerateNumbers(int min, int max)
    {
        return  Enumerable.Range(1*min, max*10).Select(i => i / 10F).ToArray();

    }
}
