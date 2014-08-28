using System.Collections.Generic;
using Code.Libaries.Building;
using UnityEngine;

namespace Code.Libaries.Building
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    public class BuildingGenerator : MonoBehaviour
    {
        private Mesh _mesh;

        public Mesh mesh
        {
            get { return _mesh; }
            set { _mesh = value; meshFilter.mesh = value; }
        }
        private MeshFilter _meshFilter;

        public MeshFilter meshFilter
        {
            get 
            {
                if(_meshFilter == null)
                {
                    _meshFilter = GetComponent<MeshFilter>();
                }
                return _meshFilter;
            }
        }

        [SerializeField]
        private bool _forceBuild = false;

        [SerializeField]
        private float WallHeight = 1f;

        private List<BuildingWall> _walls = null;

        public List<BuildingNode> nodes = new List<BuildingNode>();

        public List<BuildingWall> walls
        {
            get
            {
                if (_walls == null)
                {
                    _walls = new List<BuildingWall>();
                    BuildingWall w = new BuildingWall();
                    foreach (var buildingNode in nodes)
                    {
                        if (w.node0 == null)
                            w.node0 = buildingNode;
                        else if (w.node1 == null)
                        {
                            w.node1 = buildingNode;
                            _walls.Add(w);
                           
                            w = new BuildingWall();
                            w.node0 = buildingNode;
                        }
                    }
                }
                return _walls;
            }
        }

#if UNITY_EDITOR
        public void Update()
#else
        public void FixedUpdate()
#endif
        {
            if (_forceBuild)
            {
                BuildBuilding();
            }
        }

        public void ForceBuild()
        {
            _forceBuild = true;
        }

        private void BuildBuilding()
        {
            _forceBuild = false;
            _walls = null;
        }

        private void OnDrawGizmos()
        {
            foreach (var item in walls)
            {
                if(item != null)
                    if(item.node1 != null && item.node0 != null)
                    Gizmos.DrawLine(item.node0.transform.position, item.node1.transform.position);
            }
        }

    }

}
