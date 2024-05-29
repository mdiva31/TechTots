using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Slide : MonoBehaviour
{
    public bool ready;
    public SlideLayout layout;
    public TMP_Text titleTmp;
    [Header("Single Layout")]
    public TMP_Text content;
    public Image image;
    [Header("Multiple Layout")]
    public TMP_Text content1;
    public TMP_Text content2;
    public TMP_Text content3;
    public Image image1;
    public Image image2;
    public Image image3;
    
    public IEnumerator SetupSlide(AppData.ContentItem content )
    {
        layout = content.slideLayout;
        switch (content.slideLayout)
        {
            case SlideLayout.quiz:
                break;
            case SlideLayout.title_only:
                titleTmp.text = content.title;
                break;
            case SlideLayout.title_content:
                titleTmp.text = content.title;
                this.content.text = content.content;
                break;
            case SlideLayout.title_image:
                titleTmp.text = content.title;
                yield return IE_DowmloadImage(image, content.image);
                break;
            case SlideLayout.text_only:
                this.content.text = content.content;
                break;
            case SlideLayout.image_kiri_text_kanan:
                this.content.text = content.content;
                yield return IE_DowmloadImage(image, content.image);
                break;
            case SlideLayout.image_kanan_text_kiri:
                this.content.text = content.content;
                yield return IE_DowmloadImage(image, content.image);
                break;
            case SlideLayout.image_only:
                yield return IE_DowmloadImage(image, content.image);
                break;
            case SlideLayout.image_subtitle:
                this.content.text = content.content;
                yield return IE_DowmloadImage(image, content.image);
                break;
            case SlideLayout.column_2_text_only:
                this.content1.text = content.content1;
                this.content2.text = content.content2;
                break;
        }
        ready = true;
    }

    public IEnumerator IE_DowmloadImage(Image targetImage , string url)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            // Check for errors
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error downloading image: " + uwr.error);
            }
            else
            {
                // Get the downloaded texture
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);

                // Convert the texture to a sprite
                Sprite sprite = ConvertTextureToSprite(texture);

                // Apply the sprite to the target Image component
                targetImage.sprite = sprite;
            }
        }
    }
    Sprite ConvertTextureToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

}
