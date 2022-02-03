using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WhiteboardMarkerScript : MonoBehaviour
{
    [SerializeField] private Transform _tip;
    [SerializeField] private int _penSize = 5;


    private Renderer _renderer;

    //List to save the pixels that we are drawing
    private Color[] _colors;

    //Length tip
    private float _tipHeight;

    private RaycastHit _touch;
    private WhiteboardScript _whiteboard;
    private Vector2 _touchPos, _lastTouchPos;
    private bool _touchedLastFrame;
    private Quaternion _lastTouchRot;
    void Start()
    {
        //Access to the color
        _renderer = _tip.GetComponent<Renderer>();

        //Drawing squares by 5x5 pixels (2 pixels with the renderer material color). Setting colors array
        _colors = Enumerable.Repeat(_renderer.material.color, _penSize * _penSize).ToArray();
        _tipHeight = _tip.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if we are checking the whiteboard. If this is true, check the resolution, change the texture of the texture in the prticular we're touching
        //Add a little bit of interpolation so just in case we're moving the pin really fast it doesn't give us some dots, it actually streack correctly and gives us an actual line
        Draw();
    }

    private void Draw()
    {
        if(Physics.Raycast(_tip.position, transform.up, out _touch, _tipHeight))
        {
            //We only want to draw in the whiteboard, not in the other places
            if (_touch.transform.CompareTag("Whiteboard"))
            {
                if(_whiteboard == null)
                {

                    _whiteboard = _touch.transform.GetComponent<WhiteboardScript>();
                }

                //Setting touchPos with our texture coordinates
                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                //Calculte where are we touching in the whiteboard in relation to the resolution that we set up earlier. Convert the touch position to the whiteboard's texture sie
                var x = (int)(_touchPos.x * _whiteboard.textureSize.x - (_penSize / 2));
                var y = (int)(_touchPos.y * _whiteboard.textureSize.y - (_penSize / 2));

                //Exit the loop if the marker stops being in contct with the whiteboard
                if (y < 0 || y > _whiteboard.textureSize.y || x < 0 || x > _whiteboard.textureSize.x) return;

                //Draw
                if (_touchedLastFrame)
                {
                    //Setting the initial pixel that we touch
                    _whiteboard.texture.SetPixels(x, y, _penSize, _penSize, _colors);

                    //Interpolating from the last point that we touch to the current
                    for(float f=0.01f; f<1.00f; f += 0.001f)
                    {
                        var lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                        _whiteboard.texture.SetPixels(lerpX, lerpY, _penSize, _penSize, _colors);
                    }
                    //Locking the rotation so the marker doesn't snp up in line with the whiteboard
                    transform.rotation = _lastTouchRot;

                    //Applying the color to the texture
                    _whiteboard.texture.Apply();
                }

                //Setting the variables for the next frme so we have acces to it
                _lastTouchPos = new Vector2(x, y);
                _lastTouchRot = transform.rotation;
                _touchedLastFrame = true;
                return;

            }
        }

        //Re-starting variable
        _whiteboard = null;
        _touchedLastFrame = false;
    }
}
