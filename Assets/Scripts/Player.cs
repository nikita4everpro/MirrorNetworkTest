using Mirror;
using UnityEngine;

// Объект игрока
public class Player : NetworkBehaviour
{
    [SerializeField] private float _movementSpeed = 5;
    void Update()
    {
        if (isOwned == true)
        {
            // Перемещаем объект
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float speed = _movementSpeed * Time.deltaTime;
            transform.Translate(new Vector3(h * speed, 0, v * speed));
        }
    }
}