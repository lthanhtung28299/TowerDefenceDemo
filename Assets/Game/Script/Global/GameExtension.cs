using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = System.Random;

public static class GameExtension
{
    public static MonoBehaviour Show(this MonoBehaviour obj)
    {
        obj.gameObject.SetActive(true);
        return obj;
    }

    public static MonoBehaviour Hide(this MonoBehaviour obj)
    {
        obj.gameObject.SetActive(false);
        return obj;
    }

    public static GameObject Show(this GameObject obj)
    {
        obj.SetActive(true);
        return obj;
    }

    public static GameObject Hide(this GameObject obj)
    {
        obj.SetActive(false);
        return obj;
    }

    public static Component Show(this Component obj)
    {
        obj.gameObject.SetActive(true);
        return obj;
    }
    
    public static Component Hide(this Component obj)
    {
        obj.gameObject.SetActive(false);
        return obj;
    }
    
    public static Object Show(this Object obj)
    {
        var gameObject = obj as GameObject;
        gameObject.Show();
        return obj;
    }
    
    public static Object Hide(this Object obj)
    {
        var gameObject = obj as GameObject;
        gameObject.Hide();
        return obj;
    }

    public static Button SetUpSpriteButton(this Button button, List<Sprite> sprites)
    {
        var state = button.spriteState;
        state.highlightedSprite = sprites[1];
        state.pressedSprite = sprites[1];
        state.selectedSprite = sprites[0];
        state.disabledSprite = sprites[0];
        button.image.sprite = sprites[0];
        button.spriteState = state;
        return button;
    }

    public static Image SetImageSprite(this Image img, Sprite sp)
    {
        img.sprite = sp;
        img.SetNativeSize();
        return img;
    }

    public static Image SetScaleImage(this Image img, Sprite sp, float scaleSize)
    {
        img.sprite = sp;
        img.SetNativeSize();
        img.rectTransform.sizeDelta = new Vector2(img.sprite.rect.width * (scaleSize / 100),img.sprite.rect.height * (scaleSize / 100));
        return img;
    }
    
    public static void ShuffleList<T>(this List<T> list)  
    {  
        int n = list.Count;
        Random rng = new Random();
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            (list[k], list[n]) = (list[n], list[k]);
        }  
    }
    
    public static void LookAt2DCoordinate(this Transform transform, Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}