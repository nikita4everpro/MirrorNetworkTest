using UnityEngine;

// Базовый класс окна UI
public abstract class BaseUIWindow : MonoBehaviour
{
    private void Awake()
    {
        Init();
        Hide();
    }

    private void OnDestroy()
    {
        Deinit();
    }

    protected abstract void Init();
    protected abstract void Deinit();

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

}
