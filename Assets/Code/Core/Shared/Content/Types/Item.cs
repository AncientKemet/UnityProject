using System;
using System.Collections;
using Code.Core.Client.Units;
using Code.Core.Shared.Content.Types.ItemExtensions;
using Code.Libaries.Generic.Managers;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;

namespace Code.Core.Shared.Content.Types
{
    [Serializable]
    [ExecuteInEditMode]
    public class Item : ContentItem
    {
        public Texture2D ItemIcon;

        [Range(0f, 20f)]
        public float Weight;

        public bool Tradable = true;
        
        public bool Stackable = false;

        public int MaxStacks = 1;

        private int _inContentManagerIndex = -1;

        public int InContentManagerIndex
        {
            get
            {
                if (_inContentManagerIndex == -1)
                {
                    _inContentManagerIndex = ContentManager.I.Items.IndexOf(this);
                }
                return _inContentManagerIndex;
            }
        }

        private void Start()
        {
            name = name.Replace("(Clone)", "");
            ItemRigid itemRigid = GetComponent<ItemRigid>();
            if (itemRigid != null)
            {
                itemRigid.PhysicsEnabled = true;
            }
        }

        public void EnterUnit(PlayerUnit unit)
        {
            StartCoroutine(_enterUnit(unit.transform,Vector3.up * 1));
        }

        private IEnumerator _enterUnit(Transform u, Vector3 offset)
        {
            float startTime = Time.realtimeSinceStartup;
            float time = 0.5f;

            Vector3 start = transform.position;
            Vector3 startScale = transform.localScale;

            float t = 0.001f;

            while (Time.realtimeSinceStartup - startTime < time)
            {
                yield return new WaitForEndOfFrame();

                t += Time.deltaTime;

                if (Time.realtimeSinceStartup - startTime < time)
                {
                    transform.position = start*(1f - t/time) + (u.transform.position+offset) * (t/time);
                    transform.localScale = startScale * (1f - t / time) + (Vector3.zero) * (t / time);
                }
            }

            ContentManager.I.CreateEffect(0, transform.position);

            Destroy(gameObject);
        }
    }

    [RequireComponent(typeof(Item))]
    public class ItemExtension : MonoBehaviour
    {
        public Item Item { get { return GetComponent<Item>(); } }
    }

}
