using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Formidable.Drawing
{

    public static class DrawManager
    {

        private static bool _isInitialized;
        private static bool _areResourcesInitialized;

        private static Texture2D _lineTexture;
        private static Dictionary<TextStyle, GUIStyle> _guiStyles;

        static DrawManager()
        {
            _isInitialized = false;
            _areResourcesInitialized = false;
            _lineTexture = null;
            _guiStyles = null;
        }

        public static void Initialize()
        {
            if (_isInitialized)
                return;

            _lineTexture = new Texture2D(1, 1);
            _guiStyles = new Dictionary<TextStyle, GUIStyle>();

            _isInitialized = true;
        }

        public static void InitializeResources()
        {
            if (!_isInitialized || _areResourcesInitialized)
                return;

            _guiStyles.Add(TextStyle.Normal, new GUIStyle(GUI.skin.label) { fontSize = 14 });
            _guiStyles.Add(TextStyle.Small, new GUIStyle(GUI.skin.label) { fontSize = 12 });

            _areResourcesInitialized = true;
        }

        public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
        {
            if (!_isInitialized)
                return;

            Matrix4x4 oldMatrix = GUI.matrix;
            Color oldColor = GUI.color;

            float angle = Vector3.Angle((pointB - pointA), Vector2.right);

            if (pointA.y > pointB.y)
                angle = -angle;

            GUI.color = color;

            GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, (pointA.y + 0.5f)));
            GUIUtility.RotateAroundPivot(angle, pointA);

            GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1f, 1f), _lineTexture);

            GUI.matrix = oldMatrix;
            GUI.color = oldColor;
        }

        public static void DrawRectangle(float x, float y, float width, float height, Color color)
        {
            if (!_isInitialized)
                return;

            Vector2 point = new Vector2(x, y);
            Vector2 xWidthExtendedPoint = new Vector2((x + width), y);
            Vector2 yHeightExtendedPoint = new Vector2(x, (y + height));
            Vector2 endPoint = new Vector2((x + width), (y + height));

            DrawLine(point, xWidthExtendedPoint, color, 1f);
            DrawLine(point, yHeightExtendedPoint, color, 1f);
            DrawLine(xWidthExtendedPoint, endPoint, color, 1f);
            DrawLine(yHeightExtendedPoint, endPoint, color, 1f);
        }

        public static void DrawShadowedText(float x, float y, string text, Color color, TextStyle textStyle)
        {
            if (!_isInitialized || !_areResourcesInitialized)
                return;

            if (!_guiStyles.ContainsKey(textStyle))
                return;

            GUIStyle guiStyle = _guiStyles[textStyle];
            GUIContent guiContent = new GUIContent(text);
            Vector2 textSize = guiStyle.CalcSize(guiContent);

            GUIStyle oldGUIStyle = guiStyle;

            guiStyle.normal.textColor = Color.black;

            GUI.Label(new Rect((x + 1f), (y + 1f), textSize.x, textSize.y), guiContent, guiStyle);

            guiStyle.normal.textColor = color;

            GUI.Label(new Rect(x, y, textSize.x, textSize.y), guiContent, guiStyle);

            guiStyle = oldGUIStyle;
        }

        public static Vector2 CalculateTextSize(string text, TextStyle textStyle)
        {
            if (!_isInitialized || !_areResourcesInitialized)
                return default;

            if (!_guiStyles.ContainsKey(textStyle))
                return default;

            if (String.IsNullOrEmpty(text))
                return Vector2.zero;

            return _guiStyles[textStyle].CalcSize(new GUIContent(text));
        }

    }

}
