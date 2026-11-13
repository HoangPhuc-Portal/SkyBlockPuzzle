using UnityEngine;
using UnityEngine.UI;

namespace BlockBlast.Shape
{
    /// <summary>
    /// Visual của một ô block trong UI Tray.
    /// Class này chỉ có nhiệm vụ hiển thị Sprite.
    /// </summary>
    public class BlockTileUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Image image;

        /// <summary>
        /// Sprite hiện tại đang được hiển thị.
        /// </summary>
        public Sprite Sprite
        {
            get
            {
                return image != null
                    ? image.sprite
                    : null;
            }
        }

        private void Awake()
        {
            // Tự tìm Image nếu chưa kéo vào Inspector.
            if (image == null)
            {
                image =
                    GetComponent<Image>();
            }
        }

        /// <summary>
        /// Gán sprite cho tile.
        /// </summary>
        public void SetSprite(Sprite sprite)
        {
            if (image == null)
            {
                image =
                    GetComponent<Image>();
            }

            if (image == null)
            {
                Debug.LogError(
                    $"BlockTileUI trên {name} không có Image."
                );

                return;
            }

            image.sprite = sprite;

            // Sprite quyết định visual,
            // không tint thêm màu khác.
            image.color = Color.white;
        }

        /// <summary>
        /// Xóa sprite hiện tại.
        /// </summary>
        public void Clear()
        {
            if (image == null)
                return;

            image.sprite = null;
        }
    }
}