using System.Collections;
using System.Collections.Generic;
using CandyCrush.Enums;
using UnityEngine;

namespace CandyCrush.Services
{
    public class CameraService
    {
        public void CenterCamera(GameObject camera, GameObject targetObject, int zPosition = -10)
        {
            // Obtém todos os renderers dos filhos do objeto alvo
            Renderer[] childRenderers = targetObject.GetComponentsInChildren<Renderer>();

            if (childRenderers.Length == 0)
            {
                Debug.LogError("Nenhum renderer encontrado nos filhos do objeto alvo.");
                return;
            }

            // Calcula a posição média dos renderers dos filhos
            Vector3 centerPosition = Vector3.zero;
            foreach (Renderer childRenderer in childRenderers)
                centerPosition += childRenderer.bounds.center;


            centerPosition /= childRenderers.Length;

            // Define a posição da câmera para a posição média calculada
            camera.transform.position = new Vector3(centerPosition.x, centerPosition.y, zPosition);
        }
    }

}