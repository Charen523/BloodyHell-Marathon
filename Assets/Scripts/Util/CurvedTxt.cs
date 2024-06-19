using UnityEngine;
using TMPro;

public class CurvedTxt : MonoBehaviour
{
    public TMP_Text titleText;
    public AnimationCurve curve;

    private void Start()
    {
        if (titleText == null)
        {
            titleText = GetComponent<TMP_Text>();
        }

        makeCurve();
        WarpText();
    }

    private void makeCurve()
    {
        curve = new AnimationCurve();
        
        Keyframe key1 = new Keyframe(0, 0, 0, 18);
        Keyframe key2 = new Keyframe(0.5f, 5, 0, 0);
        Keyframe key3 = new Keyframe(1, 0, -18, 0);

        
        curve.AddKey(key1);
        curve.AddKey(key2);
        curve.AddKey(key3);
    }

    private void WarpText()
    {
        titleText.ForceMeshUpdate();
        TMP_TextInfo textInfo = titleText.textInfo;
        Vector3[] vertices;
        
        float boundsMinX = titleText.bounds.min.x;
        float boundsMaxX = titleText.bounds.max.x;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
                continue;

            vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                Vector3 orig = vertices[charInfo.vertexIndex + j];
                float x0 = (orig.x - boundsMinX) / (boundsMaxX - boundsMinX);
                float y0 = curve.Evaluate(x0) * 10;

                vertices[charInfo.vertexIndex + j] = new Vector3(orig.x, orig.y + y0, orig.z);
            }
        }

        titleText.UpdateVertexData();
    }
}
