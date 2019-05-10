using UnityEngine;
using MLAgents;

public class DragonAgent : Agent {

    [SerializeField] GameController controller;
    [SerializeField] Dragon dragon;
    //[SerializeField] int numberOfRaycasts = 9;
    //[SerializeField] float raycastSpacing = .5f;
    [SerializeField] float raycastDistance = 40f;
    [SerializeField] float raycastXOffset = -3f;
    [SerializeField] float[] obstacleRaycasts = new float[] { -2.5f, -1.5f, -.5f, 1f, 2.3f, 3.6f };
    [SerializeField] float[] coinRaycasts = new float[] { -.5f, .5f, 1.5f };

    Rigidbody2D rb;
    Flapper flapper;
    int score;

    private void Awake()
    {
        rb = dragon.GetComponent<Rigidbody2D> ();
        flapper = dragon.GetComponent<Flapper> ();
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        base.AgentAction (vectorAction, textAction);
        if ((int)vectorAction[0] > 0) {
            flapper.Flap ();
        }
        if (dragon.score > score) {
            AddReward ((dragon.score - score) * 1f);
            score = dragon.score;
            //Done ();
        }
        //if (dragon.transform.position.y > 3.3f || dragon.transform.position.y < -2f) SetReward (-Time.deltaTime);
        //else SetReward (Time.deltaTime);
        //SetReward (Time.deltaTime);
        AddReward (.001f);
    }

    public override void AgentReset()
    {
        Debug.Log ($"Score: {score}\t Reward: {GetCumulativeReward ()}");
        base.AgentReset ();
        score = 0;
        if (controller.state == GameController.State.GameOver)
            controller.RestartGame ();
        if (controller.state == GameController.State.Waiting) {
            controller.StartGame ();
            flapper.Flap ();
        }
    }

    public override void CollectObservations()
    {
        base.CollectObservations ();
        AddVectorObs (Mathf.Clamp01 ((flapper.transform.position.y + 4f) / 8f));
        AddVectorObs (Mathf.Clamp01 ((rb.velocity.y + 20f) / 28f));
        for (int i = 0; i < obstacleRaycasts.Length; i++) {
            //float offset = (i % 2 == 0) ? -i / 2 * raycastSpacing : (i + 1) / 2 * raycastSpacing;
            float height = obstacleRaycasts[i];
            Vector2 pos = new Vector2 (flapper.transform.position.x + raycastXOffset, height);
            RaycastHit2D hit = Physics2D.Raycast (pos, Vector2.right, raycastDistance, LayerMask.GetMask ("Obstacle"));
            if (hit.collider) AddVectorObs (hit.distance / (2 * raycastDistance));
            else AddVectorObs (1f);
            //hit = Physics2D.Raycast (pos, Vector2.right, raycastDistance, LayerMask.GetMask ("Obstacle"));
            //if (hit.collider) AddVectorObs (hit.distance);
            //else AddVectorObs (2 * raycastDistance);
        }
        float closestCoinDistance = 2 * raycastDistance;
        float closestCoinHeight = 0f;
        for (int i = 0; i < coinRaycasts.Length; i++) {
            float height = coinRaycasts[i];
            Vector2 pos = new Vector2 (flapper.transform.position.x + raycastXOffset, height);
            RaycastHit2D hit = Physics2D.Raycast (pos, Vector2.right, raycastDistance, LayerMask.GetMask ("Coin"));
            if (hit.collider && hit.distance<closestCoinDistance) {
                closestCoinDistance = hit.distance;
                closestCoinHeight = height;
            }
        }
        AddVectorObs (closestCoinDistance / (2 * raycastDistance));
        AddVectorObs (Mathf.Clamp01 ((closestCoinHeight + 4f) / 8f));
    }

    private void OnDrawGizmosSelected()
    {
        if (dragon == null) return;
        Gizmos.color = Color.blue;
        for (int i = 0; i < obstacleRaycasts.Length; i++) {
            //float offset = (i % 2 == 0) ? -i / 2 * raycastSpacing : (i + 1) / 2 * raycastSpacing;
            float height = obstacleRaycasts[i];
            Vector2 pos = new Vector2 (dragon.transform.position.x + raycastXOffset, height);
            Gizmos.DrawRay (pos, Vector2.right * raycastDistance);
        }
    }
}
