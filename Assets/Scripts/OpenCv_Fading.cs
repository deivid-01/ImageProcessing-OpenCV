using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class OpenCv_Fading : MonoBehaviour
{
    public SpriteRenderer imagesBase;
    public List<Texture2D> textures;

    public int max = 100;
    float actual = 0;

    void Start()
    {
        Mat mat = OpenCvSharp.Unity.TextureToMat(imagesBase.sprite.texture);
        Mat mat2 = OpenCvSharp.Unity.TextureToMat(textures[0]);
        
        StartCoroutine(HideAndShow(mat,mat2));

        //Show2Images();

        //StartCoroutine(Fading());

      
    }

    public Sprite CreateNewSprite(Mat dst) => Sprite.Create(OpenCvSharp.Unity.MatToTexture(dst), //Texture 
                                            new UnityEngine.Rect(0, 0, dst.Width, dst.Height),//Rect Propierties
                                            new Vector2(0.5f, 0.5f),                          //Offset
                                            100);                                             //Size


    public void Show2Images()
    {
        Mat mat = OpenCvSharp.Unity.TextureToMat(imagesBase.sprite.texture);
        Mat mat2 = OpenCvSharp.Unity.TextureToMat(textures[0]);

        Mat res = (mat * 0.5f)+ (mat2 * 0.5f);

        imagesBase.sprite = CreateNewSprite(res);

    }
    IEnumerator Fading()
    {
        yield return new WaitForSeconds(0.1f);

        Mat mat = OpenCvSharp.Unity.TextureToMat(imagesBase.sprite.texture);
        mat -= Scalar.All(10);
        if (actual >= max)
        {
            print("fin");
            yield return 0;
        }
           
       
        actual += 10;
        imagesBase.sprite = CreateNewSprite(mat);
        print("Increase");
        StartCoroutine (Fading());

    }

    IEnumerator HideAndShow(Mat mat,Mat mat2)
    {
        yield return new WaitForSeconds(0.1f);

       

        if (actual >= 0.9)
            yield return 0;
        Mat res = (1 - actual) * mat + actual * mat2;
        imagesBase.sprite = CreateNewSprite(res);

        actual += 0.1f;
        StartCoroutine(HideAndShow( mat, mat2));
    }
    


}
