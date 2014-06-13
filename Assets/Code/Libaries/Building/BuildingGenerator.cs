using System.Collections.Generic;
using Code.Libaries.Building;
using UnityEngine;

namespace Code.Libaries.Building
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    public class BuildingGenerator : MonoBehaviour
    {
        private Mesh mesh;
        private bool _forceBuild = false;
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


        public void FixedUpdate()
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

    }

}
