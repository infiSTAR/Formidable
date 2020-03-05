using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Formidable
{

    public static class FormidableProgram
    {

        static FormidableProgram()
        {

        }

        public static void Initialize()
        {
            GameObject gameObject = GameObject.Find(Settings.TargetGameObject);

            if (gameObject == null)
            {
                gameObject = new GameObject(Settings.ApplicationName);

                UnityEngine.Object.DontDestroyOnLoad(gameObject);
            }

            if (gameObject.GetComponent<FormidableBehaviour>() != null)
                UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<FormidableBehaviour>());

            gameObject.AddComponent<FormidableBehaviour>();
        }

    }

}
