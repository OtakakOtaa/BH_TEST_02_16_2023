using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.SpawnPoints
{
    public class SpawnPointsProvider : MonoBehaviour
    {
        [SerializeField] private RenderSettings _renderSettings;
        [SerializeField] private List<Vector3> _dotsList;
        [SerializeField] private bool _enableFlag = false;

        private readonly EnumerableRandomizer _randomizer = new ();

        public List<Vector3> RandomizeSpawnPoints()
            =>  _dotsList = _randomizer.Randomize(_dotsList).ToList();
        
        public void OnDrawGizmos()
        {
            if(_enableFlag is false) return; 
            DrawSpawnPoints();
        }

        private void DrawSpawnPoints()
        {
            if(_enableFlag is false) return;
            
            Color cashedGizmosColor = Gizmos.color;
            Gizmos.color = _renderSettings.DrawColor;

            foreach (var spawnDot in _dotsList)
            {
                switch (_renderSettings.RenderMarkType)
                {
                    case RenderMarkType.Sphere:
                        Gizmos.DrawSphere(spawnDot, _renderSettings.Scale);
                        break;
                    case RenderMarkType.Cube:
                        Gizmos.DrawCube(
                            spawnDot,
                            new Vector3(_renderSettings.Scale, _renderSettings.Scale, _renderSettings.Scale)
                        );
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            Gizmos.color = cashedGizmosColor;
        }

        [Serializable]
        public struct RenderSettings
        {
            public Color DrawColor;
            public RenderMarkType RenderMarkType;
            [Range(0.2f, 5f)] public float Scale;
        }
        
        public enum RenderMarkType
        {
            Sphere,
            Cube,
        }
    }
}