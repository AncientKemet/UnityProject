using System.Collections.Generic;
using UnityEngine;

namespace Code.Libaries.Generic.Trees
{
    /// <summary>
    /// Quad tree.
    /// </summary>
    public class QuadTree
    {
        public static bool AllowVisibilityFromTop = false;
        private int _divisions = 0;
        private bool _isDivided = false;
        private QuadTree[] children = null;
        private QuadTree parent = null;
        private LinkedList<IQuadTreeObject> objects;
        private Vector2 _position, _size;
        private Vector2 longBoundary;
        private Vector2 shortBoundary;
        private int _amountOfObjects = 0;
        private QuadTree _root = null;
        private QuadTree _north = null;
        private List<IQuadTreeObject> _objectsVisible = null;

        public List<IQuadTreeObject> ObjectsVisible
        {
            get
            {
                if (_objectsVisible == null)
                {
                    if (!_isDivided)
                    {
                        _objectsVisible = new List<IQuadTreeObject>(amountOfObjects);
                        if (_amountOfObjects > 0)
                            ObjectsVisible.AddRange(objects);
                        if (North != null)
                        {
                            if (North._amountOfObjects > 0)
                                ObjectsVisible.AddRange(North.objects);
                        }
                        if (NorthEast != null)
                        {
                            if (NorthEast._amountOfObjects > 0)
                                ObjectsVisible.AddRange(NorthEast.objects);
                        }
                        if (East != null)
                        {
                            if (East._amountOfObjects > 0)
                                ObjectsVisible.AddRange(East.objects);
                        }
                        if (SouthEast != null)
                        {
                            if (SouthEast._amountOfObjects > 0)
                                ObjectsVisible.AddRange(SouthEast.objects);
                        }
                        if (South != null)
                        {
                            if (South._amountOfObjects > 0)
                                ObjectsVisible.AddRange(South.objects);
                        }
                        if (SouthWest != null)
                        {
                            if (SouthWest._amountOfObjects > 0)
                                ObjectsVisible.AddRange(SouthWest.objects);
                        }
                        if (West != null)
                        {
                            if (West._amountOfObjects > 0)
                                ObjectsVisible.AddRange(West.objects);
                        }
                        if (NorthWest != null)
                        {
                            if (NorthWest._amountOfObjects > 0)
                                ObjectsVisible.AddRange(NorthWest.objects);
                        }
                    }
                    if (_isDivided && AllowVisibilityFromTop)
                    {
                        _objectsVisible = new List<IQuadTreeObject>(amountOfObjects);
                        foreach (var child in children)
                        {
                            _objectsVisible.AddRange(child.ObjectsVisible);
                        }
                    }
                }
                return _objectsVisible;
            }
        }

        public QuadTree North
        {
            get
            {
                if (_north == null)
                {
                    GetBranchNeightbour(ref _north, new Vector2(_size.x * 0.5f, _size.y * 1.5f));
                }
                return _north;
            }
        }

        private QuadTree _northEast = null;

        public QuadTree NorthEast
        {
            get
            {
                if (_northEast == null)
                {
                    GetBranchNeightbour(ref _northEast, new Vector2(_size.x * 1.5f, _size.y * 1.5f));
                }
                return _northEast;
            }
        }

        private QuadTree _east = null;

        public QuadTree East
        {
            get
            {
                if (_east == null)
                {
                    GetBranchNeightbour(ref _east, new Vector2(_size.x * 1.5f, _size.y * 0.5f));
                }
                return _east;
            }
        }

        private QuadTree _southEast = null;

        public QuadTree SouthEast
        {
            get
            {
                if (_southEast == null)
                {
                    GetBranchNeightbour(ref _southEast, new Vector2(_size.x * 1.5f, -_size.y * 0.5f));
                }
                return _southEast;
            }
        }

        private QuadTree _south = null;

        public QuadTree South
        {
            get
            {
                if (_south == null)
                {
                    GetBranchNeightbour(ref _south, new Vector2(_size.x * 0.5f, -_size.y * 0.5f));
                }
                return _south;
            }
        }

        private QuadTree _southWest = null;

        public QuadTree SouthWest
        {
            get
            {
                if (_southEast == null)
                {
                    GetBranchNeightbour(ref _southEast, new Vector2(-_size.x * 0.5f, -_size.y * 0.5f));
                }
                return _southWest;
            }
        }

        private QuadTree _west = null;

        public QuadTree West
        {
            get
            {
                if (_west == null)
                {
                    GetBranchNeightbour(ref _west, new Vector2(-_size.x * 0.5f, _size.y * 0.5f));
                }
                return _west;
            }
        }

        private QuadTree _northWest = null;

        public QuadTree NorthWest
        {
            get
            {
                if (_northWest == null)
                {
                    GetBranchNeightbour(ref _northWest, new Vector2(-_size.x * 0.5f, _size.y * 1.5f));
                }
                return _northWest;
            }
        }

        public QuadTree Root
        {
            get
            {
                if (_root == null)
                {
                    if (parent == null)
                    {
                        _root = this;
                        return  _root;
                    }

                    QuadTree par = parent;
                    for (int i = 0; i < 10; i++)
                    {
                        if (par.parent == null)
                        {
                            _root = par;
                            break;
                        }
                        else
                            par = par.parent;
                    }
                }
                return _root;
            }
        }

        void GetBranchNeightbour(ref QuadTree _ref, Vector2 offset)
        {
            //As root shouldn't have any neightbours
            if (Root != this)
            {
                _ref = Root.GetTreeFor(_position + offset, _divisions);
                if (_ref == null)
                {
                    Vector2 repeatOffset = new Vector2(Mathf.Repeat(Mathf.Abs(offset.x), Root._size.x), Mathf.Repeat(Mathf.Abs(offset.y), Root._size.y));
                    // probably on the edge

                }
            }
        }

        public int amountOfObjects
        {
            get
            {
                if (_isDivided)
                {
                    _amountOfObjects = 0;
                    foreach (var item in children)
                    {
                        _amountOfObjects += item.amountOfObjects;
                    }
                }
                return _amountOfObjects;
            }
        }

        public QuadTree(int divisions, Vector2 position, Vector2 size)
        {
            _position = position;
            _size = size;
            _divisions = divisions;

            shortBoundary = _position;
            longBoundary = _position + _size;
            if (divisions > 0)
            {
                _isDivided = true;
                int _divisionsLeft = divisions - 1;
                Vector2 _childSize = size / 2f;
                children = new QuadTree[]
                {
                    new QuadTree(_divisionsLeft, position + new Vector2(0, 0), _childSize),
                    new QuadTree(_divisionsLeft, position + new Vector2(_childSize.x, 0), _childSize),
                    new QuadTree(_divisionsLeft, position + new Vector2(0, _childSize.y), _childSize),
                    new QuadTree(_divisionsLeft, position + new Vector2(_childSize.x, _childSize.y), _childSize) 
                };
                foreach (var child in children)
                {
                    child.parent = this;
                }
            }

            if (!_isDivided)
            {
                objects = new LinkedList<IQuadTreeObject>();
            }
        }

        public void Update()
        {
            float time = Time.realtimeSinceStartup;
            if (!_isDivided)
            {
                if (objects.Count == 0)
                    return;
                List<KeyValuePair<QuadTree, IQuadTreeObject>> objectsToRemove = new List<KeyValuePair<QuadTree, IQuadTreeObject>>();
                foreach (var item in objects)
                {
                    if (item != null)
                    {
                        Vector2 change = item.PositionChange();
                        if (change != Vector2.zero)
                        {
                            Vector2 pos = item.GetPosition();
                            if (!ContainsPoint(pos))
                            {
                                bool succeed = false;
                                if (change.x >= 0)
                                {
                                    if (change.y >= 0)
                                    {
                                        if (East != null && East.ContainsPoint(pos))
                                        {
                                            objectsToRemove.Add(new KeyValuePair<QuadTree, IQuadTreeObject>(East, item));
                                            succeed = true;
                                        }
                                        else if (NorthEast != null && NorthEast.ContainsPoint(pos))
                                        {
                                            objectsToRemove.Add(new KeyValuePair<QuadTree, IQuadTreeObject>(NorthEast, item));
                                            succeed = true;
                                        }
                                        else if (North != null && North.ContainsPoint(pos))
                                        {
                                            objectsToRemove.Add(new KeyValuePair<QuadTree, IQuadTreeObject>(North, item));
                                            succeed = true;
                                        }
                                    }
                                    else
                                    {
                                        if (East != null && East.ContainsPoint(pos))
                                        {
                                            objectsToRemove.Add(new KeyValuePair<QuadTree, IQuadTreeObject>(East, item));
                                            succeed = true;
                                        }
                                        else if (SouthEast != null && SouthEast.ContainsPoint(pos))
                                        {
                                            objectsToRemove.Add(new KeyValuePair<QuadTree, IQuadTreeObject>(SouthEast, item));
                                            succeed = true;
                                        }
                                        else if (South != null && South.ContainsPoint(pos))
                                        {
                                            objectsToRemove.Add(new KeyValuePair<QuadTree, IQuadTreeObject>(South, item));
                                            succeed = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (change.y >= 0)
                                    {
                                        if (West != null && West.ContainsPoint(pos))
                                        {
                                            objectsToRemove.Add(new KeyValuePair<QuadTree, IQuadTreeObject>(West, item));
                                            succeed = true;
                                        }
                                        else if (NorthWest != null && NorthWest.ContainsPoint(pos))
                                        {
                                            objectsToRemove.Add(new KeyValuePair<QuadTree, IQuadTreeObject>(NorthWest, item));
                                            succeed = true;
                                        }
                                        else if (North != null && North.ContainsPoint(pos))
                                        {
                                            objectsToRemove.Add(new KeyValuePair<QuadTree, IQuadTreeObject>(North, item));
                                            succeed = true;
                                        }
                                    }
                                    else
                                    {
                                        if (West != null && West.ContainsPoint(pos))
                                        {
                                            objectsToRemove.Add(new KeyValuePair<QuadTree, IQuadTreeObject>(West, item));
                                            succeed = true;
                                        }
                                        else if (SouthWest != null && SouthWest.ContainsPoint(pos))
                                        {
                                            objectsToRemove.Add(new KeyValuePair<QuadTree, IQuadTreeObject>(SouthWest, item));
                                            succeed = true;
                                        }
                                        else if (South != null && South.ContainsPoint(pos))
                                        {
                                            objectsToRemove.Add(new KeyValuePair<QuadTree, IQuadTreeObject>(South, item));
                                            succeed = true;
                                        }
                                    }
                                }
                                if (!succeed)
                                {
                                    //Debug.Log("Object movement prediction failed.");
                                    objectsToRemove.Add(new KeyValuePair<QuadTree, IQuadTreeObject>(Root, item));
                                }
                            }
                        }
                    }
                }
                if (objectsToRemove.Count > 0)
                {
                    foreach (var item in objectsToRemove)
                    {
                        objects.Remove(item.Value);
                        item.Key.AddObject(item.Value);
                        _amountOfObjects--;
                    }
                }
            }
            else
            {
                foreach (var child in children)
                {
                    if (child.amountOfObjects > 0)
                        child.Update();
                }
            }

            //Reset visible objects as they might have changed
            
            _objectsVisible = null;
        }

        public void AddObject(IQuadTreeObject o)
        {
            if (!_isDivided)
            {
                objects.AddFirst(o);
                _amountOfObjects++;
                o.CurrentBranch = this;
            }
            else
            {
                QuadTree t = GetTreeFor(o.GetPosition());
                if (t != null)
                    t.AddObject(o);
                else
                {
                    Debug.LogError("Failed to find sub tree for DirecionVector: "+ o.GetPosition());
                }
            }
        }

        private QuadTree GetTreeFor(Vector2 pos)
        {
            return GetTreeFor(pos, 0);
        }

        private QuadTree GetTreeFor(Vector2 pos, int division)
        {
            if (ContainsPoint(pos))
            {
                if (_divisions == division)
                {
                    return this;
                }
                else
                {
                    QuadTree tree;
                    foreach (var child in children)
                    {
                        tree = child.GetTreeFor(pos, division);
                        if (tree != null)
                            return tree;
                    }
                    return null;
                }
            }
            else
                return null;
        }

        private bool ContainsPoint(Vector2 point)
        {
            if (shortBoundary.x <= point.x && shortBoundary.y <= point.y && longBoundary.x > point.x && longBoundary.y > point.y)
                return true;
            return false;
        }

        public void DrawGizmos()
        {
            if (amountOfObjects == 0)
                return;

            if (!_isDivided)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(_position + _size / 2, _size);
                foreach (var o in objects)
                {
                    Gizmos.DrawCube(o.GetPosition(), Vector3.one);
                }
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(_position + _size / 2, _size);
                foreach (var item in children)
                {
                    item.DrawGizmos();
                }
            }
        }

        public void RemoveObject(IQuadTreeObject o)
        {
            objects.Remove(o);
            _amountOfObjects--;
        }
    }
}

