using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class melee : MonoBehaviour
{
    public Vector3 standbyPosition;
    public Vector3 endPosition;
    public float speedScale = 1.0f;
    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Melee_Attack());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = end;
    }

    public IEnumerator Melee_Attack()
    {
        Debug.Log("Melee");
        startPosition = transform.position;
        float attack_duration = 2f * Mathf.Clamp(speedScale, 0.1f, 10.0f);
        StartCoroutine(MoveOverSeconds(this.gameObject, standbyPosition, attack_duration/2));
        yield return new WaitForSeconds(attack_duration);
        StartCoroutine(MoveOverSeconds(this.gameObject, endPosition, attack_duration));
        yield return new WaitForSeconds(attack_duration*1.2f);
        StartCoroutine(MoveOverSeconds(this.gameObject, startPosition, attack_duration/2));
    }
}
