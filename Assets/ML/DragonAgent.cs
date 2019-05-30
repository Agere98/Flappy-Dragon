using UnityEngine;
using MLAgents;

namespace Demo {

    public class DragonAgent : Agent {

        [SerializeField] GameController controller = null;
        [SerializeField] Dragon dragon = null;
        [SerializeField] float raycastDistance = 20f;
        [SerializeField] float raycastXOffset = -2f;
        [SerializeField] float[] obstacleRaycasts = new float[] { -2.5f, -1.5f, -.5f, 1f, 2.3f, 3.6f };
        [SerializeField] float[] coinRaycasts = new float[] { -.5f, .5f, 1.5f };

        Transform agentTransform;
        Transform tr;
        Rigidbody2D rb;
        Flapper flapper;
        int score;

        private void Awake()
        {
            agentTransform = transform;
            tr = dragon.transform;
            rb = dragon.GetComponent<Rigidbody2D> ();
            flapper = dragon.GetComponent<Flapper> ();
        }

        public override void AgentAction(float[] vectorAction, string textAction)
        {
            base.AgentAction (vectorAction, textAction);
            // To Flap, or not to Flap
            if ((int)vectorAction[0] > 0) {
                flapper.Flap ();
            }
            // Reward for coins collected
            if (dragon.score > score) {
                AddReward ((dragon.score - score) * 1f);
                score = dragon.score;
            }
            // Existential bonus
            AddReward (.001f);
        }

        public override void AgentReset()
        {
#if UNITY_EDITOR
            Debug.Log ($"Score: {score}\t Reward: {GetCumulativeReward ()}");
#endif
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
            // Normalized y position of the agent
            AddVectorObs (Mathf.Clamp01 ((tr.localPosition.y + 4f) / 8f));
            // Normalized y velocity of the agent
            AddVectorObs (Mathf.Clamp01 ((rb.velocity.y + 20f) / 28f));
            // Normalized distances to the nearest obstacles
            for (int i = 0; i < obstacleRaycasts.Length; i++) {
                Vector2 pos = new Vector2 (tr.position.x + raycastXOffset, agentTransform.position.y + obstacleRaycasts[i]);
                RaycastHit2D hit = Physics2D.Raycast (pos, Vector2.right, raycastDistance, LayerMask.GetMask ("Obstacle"));
                if (hit.collider) {
                    AddVectorObs (hit.distance / (2 * raycastDistance));
                    Debug.DrawLine (pos, hit.point, Color.red);
                }
                // No obstacle detected
                else AddVectorObs (1f);
            }
            // Normalized position of the closest coin
            float closestCoinDistance = 2 * raycastDistance;
            float closestCoinHeight = 0f;
            for (int i = 0; i < coinRaycasts.Length; i++) {
                Vector2 pos = new Vector2 (tr.position.x + raycastXOffset, agentTransform.position.y + coinRaycasts[i]);
                RaycastHit2D hit = Physics2D.Raycast (pos, Vector2.right, raycastDistance, LayerMask.GetMask ("Coin"));
                if (hit.collider) {
                    Debug.DrawLine (pos, hit.point, Color.green);
                }
                if (hit.collider && hit.distance < closestCoinDistance) {
                    if (hit.distance < 1f) {
                        // Coin missed
                        continue;
                    }
                    closestCoinDistance = hit.distance;
                    closestCoinHeight = coinRaycasts[i];
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
                Vector2 pos = new Vector2 (dragon.transform.position.x + raycastXOffset, transform.position.y + obstacleRaycasts[i]);
                Gizmos.DrawRay (pos, Vector2.right * raycastDistance);
            }
            Gizmos.color = Color.yellow;
            for (int i = 0; i < coinRaycasts.Length; i++) {
                Vector2 pos = new Vector2 (dragon.transform.position.x + raycastXOffset, transform.position.y + coinRaycasts[i]);
                Gizmos.DrawRay (pos, Vector2.right * raycastDistance);
            }
        }
    }
}