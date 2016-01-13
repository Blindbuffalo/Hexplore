using UnityEngine;
using System.Collections;

public class RayCastTest : MonoBehaviour {

    private Layout L = new Layout(Layout.pointy, new Vector3(1f, 1f), new Vector3(0f, 0f));
    // Update is called once per frame
    void Update () {

        if (Input.GetMouseButtonDown(1))
        {
            int layer = LayerMask.NameToLayer("PlayArea");
            
            LayerMask lm =  1 << layer ;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit HitInfo;
            if (Physics.Raycast(ray, out HitInfo, 1000f, lm))
            {
                //Debug.Log("hit!");new Vector3( HitInfo.point.x, HitInfo.point.z, 5)
                
                FractionalHex h = Layout.PixelToHex(L, HitInfo.point);
                Hex MouseOverHex = FractionalHex.HexRound(h);

                DrawGraphics.Instance.DrawHex(MouseOverHex, "test", Color.red);

            }
        }







    }
}
