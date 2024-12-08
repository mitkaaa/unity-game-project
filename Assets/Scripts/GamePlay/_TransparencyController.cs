using System.Collections.Generic;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    public GameObject player;
    public LayerMask obstructionLayerMask; // Слой объектов, которые могут перекрывать вид

    private List<Renderer> obstructedObjects = new List<Renderer>();  // Список объектов, сделанных прозрачными
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();  // Оригинальные материалы

    public Material semiTransparentMaterial;  // Полупрозрачный материал (50% прозрачности)

    void Update()
    {
        // Направление от камеры к главной сфере
        Vector3 directionToSphere = player.transform.position - transform.position;

        // Найти объекты, перекрывающие обзор сферы
        RaycastHit[] hits = Physics.RaycastAll(transform.position, directionToSphere, directionToSphere.magnitude, obstructionLayerMask);

        // Восстановить оригинальные материалы для ранее сделанных прозрачными объектов
        foreach (Renderer renderer in obstructedObjects)
        {
            if (originalMaterials.ContainsKey(renderer))
            {
                renderer.materials = originalMaterials[renderer];
            }
        }

        obstructedObjects.Clear();

        // Прозрачность для новых объектов, перекрывающих обзор
        foreach (RaycastHit hit in hits)
        {
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();

            if (hitRenderer != null)
            {
                // Сохранить оригинальные материалы, если они еще не сохранены
                if (!originalMaterials.ContainsKey(hitRenderer))
                {
                    originalMaterials[hitRenderer] = hitRenderer.materials;
                }

                // Применить полупрозрачный материал
                Material[] transparentMaterials = new Material[hitRenderer.materials.Length];
                for (int i = 0; i < transparentMaterials.Length; i++)
                {
                    transparentMaterials[i] = semiTransparentMaterial;
                }
                hitRenderer.materials = transparentMaterials;

                obstructedObjects.Add(hitRenderer);
            }
        }
    }
}