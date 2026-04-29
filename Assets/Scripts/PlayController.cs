using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // เพิ่ม namespace สำ หรับ TextMeshPro

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 8f;

    [Header("Respawn Settings")]
    public Transform currentSpawnPoint; // ลาก SpawnPoint มาใส่ใน Inspector

    private Rigidbody2D rb;
    private int score = 0; // เพิ่มตัวแปรสำหรับเก็บคะแนน

    // เพิ่มตัวแปรสำ หรับเชื่อมต่อ TextMeshPro
    public TextMeshProUGUI scoreText; // ลาก ScoreText UI มาใส่ใน Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // ถ้าเริ่มเกมมาแล้วยังไม่ได้ตั้งจุดเกิด ให้ใช้ตำแหน่งปัจจุบันไปก่อน
        if (currentSpawnPoint == null)
        {
            Respawn();
        }
        else
        {
            transform.position = currentSpawnPoint.position;
        }
        //อัปเดต UI คะแนนเริ่มต้น
        UpdateScoreUI();
    }

    void Update()
    {
        // การเคลื่อนที่
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // การกระโดด
        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    // ส่วนของการชน (Trigger)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. ตรวจสอบว่าชน "อุปสรรคสีแดง" หรือไม่
        if (other.CompareTag("Obstacle"))
        {
            Respawn();
        }

        // 2. ตรวจสอบว่าชน "ประตูเหลือง" (Finish) หรือไม่
        if (other.CompareTag("Finish"))
        {
            NextLevel();
        }
        // ชนเหรียญ
        if (other.CompareTag("Coin"))
        {
            score += 10; // เพิ่มคะแนน
            UpdateScoreUI(); // อัปเดต UI คะแนน
            Destroy(other.gameObject); // ทำลายเหรียญที่ชน
        }
    }

    public void Respawn()
    {
        if (currentSpawnPoint != null)
        {
            transform.position = currentSpawnPoint.position;
            rb.linearVelocity = Vector2.zero; // รีเซ็ตความเร็วเป็นศูนย์
        }
        else
        {
            Debug.LogWarning("ยังไม่ได้ลาก SpawnPoint ใส่ใน Inspector นะจ๊ะ!");
            // ถ้าไม่มี SpawnPoint ให้เกิดที่ตำแหน่งปัจจุบัน
            transform.position = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
        }
    }

    public GameObject youWinText;
    void NextLevel()
    {   
        // หาเลขลำ ดับฉากปัจจุบัน แล ้วบวกไป 1 เพื่อไปฉากถัดไป
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        // เช็คก่อนว่ามีฉากถัดไปจริงไหม (ป้องกัน Error)
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("ไม่มีด่านต่อไปแล้ว! คุณชนะเกม!");
            youWinText.SetActive(true);
            Time.timeScale = 0f; // หยุดเกม
        }
    }
    // ฟังก์ชันสำหรับอัปเดต UI คะแนน
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}