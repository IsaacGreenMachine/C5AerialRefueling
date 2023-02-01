using UnityEngine;

public class EndScreen : MonoBehaviour
{
    /// <summary>
    /// The instance of the end screen, used to set it active when the game ends.
    /// </summary>
    public GameObject Instance;

    /// <summary>
    /// Sets the end screen active and disables the game object.
    /// </summary>
    public void SetUp()
    {
        Instance.SetActive(false);
        gameObject.SetActive(true);
    }
}
