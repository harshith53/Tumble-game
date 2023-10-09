using System.Collections;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private bool isDragging = false;
    public float waitTime = 0.15f;
    public float maxRadius = 2f;

    public GameObject nextPlayer;

    private Rigidbody2D rb;
    public Rigidbody2D pivot;
    private SpringJoint2D sj;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sj = GetComponent<SpringJoint2D>();
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector3.Distance(mousePos, pivot.position) > maxRadius)
            {
                rb.position = pivot.position + (mousePos - pivot.position).normalized * maxRadius;
            }
            else
                rb.position = mousePos;
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        rb.isKinematic = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        rb.isKinematic = false;

        StartCoroutine(Release());
    }

    IEnumerator Release()
    {
        yield return new WaitForSeconds(waitTime);

        GetComponent<SpringJoint2D>().enabled = false;
        this.enabled = false;

        yield return new WaitForSeconds(5f);

        if (nextPlayer != null)
        {
            nextPlayer.SetActive(true);
        }
        else
        {
            // Game Over
            EnemyBehaviour.EnemiesAlive = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
