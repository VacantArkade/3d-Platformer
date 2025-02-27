using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    [SerializeField] GameObject hurtBox;
    [SerializeField] PlayerStats myStats;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        myStats.health -= (int)(1.0f * Time.deltaTime);
    }
}
