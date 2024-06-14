using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gamekit3D.GameCommands
{
    public class SimpleTranslator : SimpleTransformer
    {
        public new Rigidbody rigidbody;
        public Vector3 start = -Vector3.forward;
        public Vector3 end = Vector3.forward;

        private bool sceneLoaded = false; // Pour s'assurer que la scène ne se charge qu'une seule fois

        public override void PerformTransform(float position)
        {
            var curvePosition = accelCurve.Evaluate(position);
            var pos = transform.TransformPoint(Vector3.Lerp(start, end, curvePosition));
            Vector3 deltaPosition = pos - rigidbody.position;

            if (Application.isEditor && !Application.isPlaying)
                rigidbody.transform.position = pos;
            else
                rigidbody.MovePosition(pos);

            if (m_Platform != null)
                m_Platform.MoveCharacterController(deltaPosition);

            // Vérifier si la plateforme est à la position finale
            if (!sceneLoaded && Vector3.Distance(pos, transform.TransformPoint(end)) < 0.01f)
            {
                sceneLoaded = true; // Empêche de charger la scène plusieurs fois
                LoadNextScene();
            }
        }

        private void LoadNextScene()
        {
            // Charger la nouvelle scène
            SceneManager.LoadScene(2);
        }
    }
}