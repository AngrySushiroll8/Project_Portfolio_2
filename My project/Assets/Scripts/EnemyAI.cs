using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour, IDDamage
{
    [SerializeField] Renderer model;
    [SerializeField] int HP;
    Color colorOrig;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    NavMeshAgent agent;
    Transform player;
    LayerMask Ground, Player;
    Vector3 walkPoint;
    bool walkPointSet;
    int walkRange;
    [SerializeField] int enemyFireRate;

    [SerializeField] int FOV, roamDistance, roamPauseTimer;
    float roamtTimer, angleToPlayer, stoppingDistanceOrigin;
    Vector3 startingPosition;

    [SerializeField] Animator animator;
    [SerializeField] float animateSpeed;
    void Start()
    {
        colorOrig = model.material.color;
        GameManager.instance.UpdateGameGoal(1);
        startingPosition = transform.position;
        stoppingDistanceOrigin = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimations();
        //shootTimer += Time.deltaTime;
        //roamtTimer += Time.deltaTime;
        //if(playerInTrigger&& CanSeePlayer())
        //{
        CheckRoam();
        //}
        //else if(!playerInTrigger) {
        //  CheckRoam();
        //}
        if (agent.remainingDistance < 0.01f) roamtTimer += Time.deltaTime;
    }
    void SetAnimations()
    {
        float agentSpeed = agent.velocity.normalized.magnitude;
        float animatedSpeed = animator.GetFloat("Speed");
        animator.SetFloat("Speed", Mathf.Lerp(animatedSpeed, agentSpeed, Time.deltaTime * animateSpeed));
    }
    void CheckRoam()
    {
        if(roamtTimer >= roamPauseTimer && agent.remainingDistance < 0.01)
        {
            Roam();
        }
    }
    void Roam()
    {
        roamtTimer = 0;
        agent.stoppingDistance = 0;
        Vector3 randPOS = Random.insideUnitSphere * roamDistance;
        randPOS += startingPosition;
        NavMeshHit hit;
        NavMesh.SamplePosition(randPOS, out hit, roamDistance, 1);
        agent.SetDestination(hit.position);
    }
    //bool CanSeePlayer()
    //{
    //    //playerDir = GameManager.instance.player.transform.position - transform.position;
          //RaycastHit hit;
        //      if(Physics.RayCast(Transform.position, playerDirection, out private void OnControllerColliderHit(ControllerColliderHit hit)
        //{
        //    if (hit.collider.CompareTag("Player") && angleToPlayer <= FOV)
        //    {
        //        if(shootTimer )
        //    }
    //}
    //    //angleToPlayer = Vector3.Angle(playerDirection.transform.forward);
    //    //if(angleToPlayer <= FOV)
    //    //{
    //    //    agent.SetDestination(GameManager.instance.player.transform.position);
    //    //    if (shootTimer >= shootRate) Shoot();
    //    //    if (agent.remainingDistance <= agent.stoppingDistance) FaceTarget();
    //    //    return true;
    //    //}
    //    //return false;
    //}
    public void TakeDamage(int amount)
    {
        if (HP > 0)
        {
            HP -= amount;
            StartCoroutine(FlashRed());
        }
        if (HP <= 0)
        {
            GameManager.instance.UpdateGameGoal(-1);
            Destroy(gameObject);
        }
    }
    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }
}
