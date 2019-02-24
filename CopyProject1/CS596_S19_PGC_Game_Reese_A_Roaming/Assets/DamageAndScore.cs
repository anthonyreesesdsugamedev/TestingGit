using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DamageAndScore : MonoBehaviour
{

    private Scene scene;
    private int score;
    private void Start()
    {
        score = 0;
        scene = SceneManager.GetActiveScene();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "spike")
        {
            score = 0;
            SceneManager.LoadScene(scene.name);
        }
        if (collision.gameObject.tag == "coin")
        {
            score++;
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(250, 37, 200, 20), "Score: " + score);
    }
}
