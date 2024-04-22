using System.Collections.Generic;
using UnityEngine;

namespace Assets.ArmoredSoldiers2.Scripts
{
    public class SkinEditor : MonoBehaviour
    {
        public Camouflage Camouflage;
        public Chevron Chevron;

        private readonly Dictionary<Camouflage, Color> _camouflageColors = new Dictionary<Camouflage, Color>
        {
            { Camouflage.Asphalt, new Color32(200, 200, 255, 255) },
            { Camouflage.Forest, new Color32(180, 220, 150, 255) },
            { Camouflage.Khaki, new Color32(255, 220, 180, 255) },
            { Camouflage.Sand, new Color32(255, 200, 150, 255) }
        };

        private readonly Dictionary<Chevron, Color> _chevronColors = new Dictionary<Chevron, Color>
        {
            { Chevron.Red, new Color32(255, 50, 0, 255) },
            { Chevron.Green, new Color32(140, 220, 0, 255) },
            { Chevron.Blue, new Color32(0, 200, 255, 255) },
            { Chevron.Yellow, new Color32(255, 200, 0, 255) },
            { Chevron.Orange, new Color32(255, 100, 0, 255) },
            { Chevron.Purple, new Color32(220, 0, 220, 255) }
        };

        public void OnValidate()
        {
            foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
            {
                if (spriteRenderer.name == "Chevron")
                {
                    spriteRenderer.color = _chevronColors[Chevron];
                }
                else
                {
                    spriteRenderer.color = _camouflageColors[Camouflage];
                }
            }
        }
    }

    public enum Camouflage
    {
        Asphalt,
        Forest,
        Khaki,
        Sand
    }

    public enum Chevron
    {
        Red,
        Green,
        Blue,
        Yellow,
        Orange,
        Purple
    }
}