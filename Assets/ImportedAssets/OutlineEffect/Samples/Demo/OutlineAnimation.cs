using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

namespace cakeslice
{
    public class OutlineAnimation : MonoBehaviour
    {
        bool pingPong = false;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Color c = GetComponent<OutlineEffect>().lineColor0;
            Color c3 = GetComponent<OutlineEffect>().lineColor2;

            if (pingPong)
            {
                c.a += Time.deltaTime;

                if(c.a >= 1)
                    pingPong = false;

                c3.a += Time.deltaTime;

                if (c3.a >= 1)
                    pingPong = false;
            }
            else
            {
                c.a -= Time.deltaTime;

                if(c.a <= 0.25f)
                    pingPong = true;

                c3.a -= Time.deltaTime;

                if (c3.a <= 0.25f)
                    pingPong = true;
            }

            c.a = Mathf.Clamp01(c.a);
            GetComponent<OutlineEffect>().lineColor0 = c;
            GetComponent<OutlineEffect>().UpdateMaterialsPublicProperties();

            c3.a = Mathf.Clamp01(c3.a);
            GetComponent<OutlineEffect>().lineColor2 = c3;
            GetComponent<OutlineEffect>().UpdateMaterialsPublicProperties();
        }
    }
}