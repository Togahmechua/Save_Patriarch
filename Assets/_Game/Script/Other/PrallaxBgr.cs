using System.Collections.Generic;
using UnityEngine;

public class ParallaxBgr : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float imageWidth = 18.98f;

    private List<Transform> bgs = new List<Transform>();

    private void Start()
    {
        // Lấy tất cả background con
        foreach (Transform child in transform)
        {
            bgs.Add(child);
        }

        // Sắp xếp theo x tăng dần để dễ xử lý
        bgs.Sort((a, b) => a.position.x.CompareTo(b.position.x));
    }

    private void Update()
    {
        foreach (Transform bg in bgs)
        {
            bg.Translate(Vector3.left * speed * Time.deltaTime);
        }

        // Kiểm tra background đầu tiên có ra khỏi màn chưa
        Transform first = bgs[0];
        if (first.position.x <= -imageWidth)
        {
            // Lấy background cuối cùng
            Transform last = bgs[bgs.Count - 1];

            // Di chuyển first ra phía sau last
            first.position = new Vector3(last.position.x + imageWidth, first.position.y, first.position.z);

            // Đưa first xuống cuối list
            bgs.RemoveAt(0);
            bgs.Add(first);
        }
    }
}
