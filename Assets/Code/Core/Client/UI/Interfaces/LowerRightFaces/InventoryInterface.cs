using Code.Libaries.UnityExtensions.Independent;
using UnityEngine;

namespace Code.Core.Client.UI.Interfaces.LowerRightFaces
{
    public class InventoryInterface : UIInterface<InventoryInterface>
    {
        public override void Hide()
        {
            CorotineManager.Instance.StartCoroutine(
                Ease.Vector(
                    transform.localScale,
                    Vector3.zero,
                    delegate(Vector3 vector3)
                    {
                        transform.localScale = vector3;
                    },
                    delegate
                    {
                        gameObject.SetActive(false);
                    },
                    0.33f
                    )
                );

        }

        public override void Show()
        {
            gameObject.SetActive(true);
            CorotineManager.Instance.StartCoroutine(
                Ease.Vector(
                    transform.localScale,
                    Vector3.one,
                    delegate(Vector3 vector3)
                    {
                        transform.localScale = vector3;
                    },
                    delegate
                    {
                    },
                    0.33f
                    )
                );
        }
    }
}
