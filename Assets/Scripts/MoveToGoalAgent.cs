using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MoveToGoalAgent : Agent {
    [SerializeField] Transform target;
    [SerializeField] float speed;
    [SerializeField] Renderer rend;
    [SerializeField] Material lose, win;

    public override void OnEpisodeBegin() {
        transform.localPosition = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX, 0, moveZ) * speed * 2 * Time.deltaTime;
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        continousActions[0] = Input.GetAxisRaw("Horizontal");
        continousActions[1] = Input.GetAxisRaw("Vertical");
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("wall")) {
            rend.material = lose;
            SetReward(-1);
            EndEpisode();
        }
        if (collider.CompareTag("goal")) {
            rend.material = win;
            SetReward(+1);
            EndEpisode();
        }
    }
}
