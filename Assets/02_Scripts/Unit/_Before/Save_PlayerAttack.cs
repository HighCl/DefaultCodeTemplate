using UnityEngine;

namespace DefaultSetting
{
    public class Save_PlayerAttack : MonoBehaviour
    {
        //[HideInInspector]
        //Save_PlayerController controller;
        //Rigidbody2D rig;
        //WaitForFixedUpdate cashingWaitForFixedUpdate = new WaitForFixedUpdate();

        //public GameObject weapon;
        //public Animator weaponAnim;
        //public GameObject weaponPivot;
        //public Coroutine coAttack;

        //public bool showDamageText = false;

        //public bool CanAttack
        //{
        //    get
        //    {
        //        //if (!managers.input.waitattackdown)
        //        //    return false;

        //        if (!controller.Stat.HasAttack)
        //            return false;

        //        if (coAttack != null)
        //            return false;

        //        if (controller.Move.isDashing)
        //            return false;

        //        return true;
        //    }
        //}
        //public bool IsAttacking { get; protected set; } = false;
        //public bool cancelAttacking = false;

        //public float weaponDamage { get { return controller.Stat.damage; } }
        //[Range(0.2f, 2f)]
        //public float attackTime = 0.2f; //공격 지속 시간 / 무기가 나오고 몇초동안 남아있을 것인가? / 애니메이션 때문에 0.2f 이상으로 해야 함.
        //public float weaponDelay = 0.5f;

        //public float unitReboundPow = 70; //공격 대상에게 주는 반동량
        //public Vector2 hitUnitPow = new Vector2(50, 110);
        //public float hitPlatformReboundPow = 50; // 벽 공격 시 반동량

        //[Header("가속 데미지 공식")]
        //public float minAccelerationRatio = 1; //최소 가속 데미지 배율
        //public float maxAccelerationRatio = 5; //최대 가속 데미지 배율

        //public float minAccelerationValue = 40; //최소 가속량
        //public float maxAccelerationValue = 50; //최대 가속량

        //private void Reset()
        //{
        //    weapon = GetComponentInChildren<BaseWeaponSensor>().gameObject;
        //    weaponPivot = transform.Find("WeaponPivot").gameObject;
        //    weaponAnim = weaponPivot.transform.Find("WeaponSprite").GetComponent<Animator>();
        //    print(weaponAnim);
        //}


        //public void MyAwake()
        //{
        //    controller = gameObject.GetComponent<Save_PlayerController>();
        //    rig = GetComponent<Rigidbody2D>();

        //    weapon.SetActive(false);
        //}

        //public void MyFixedUpdate()
        //{
        //    if (controller.State.IsInabilityActTime)
        //        return;

        //    CheckAttack();
        //}

        //public void CheckAttack()
        //{
        //    if (!CanAttack)
        //        return;

        //    coAttack = StartCoroutine(CoAttack());

        //    IEnumerator CoAttack()
        //    {
        //        //<사전 조건>
        //        //Managers.Input.WaitAttackDown = false;
        //        controller.Anim.anim_TriggerAttack = true;
        //        SetLookDir();
        //        IsAttacking = true;
        //        cancelAttacking = false;

        //        //날리는 애니메이션
        //        //공격
        //        //Managers.Sound.Play(Managers.Data.MstMaster.PlayerSound.AttackClip);
        //        SetWeaponPos(); //무기 위치 지정

        //        ShowWeapon();
        //        weaponAnim.SetTrigger("OnAttack");

        //        float currentTime = 0;
        //        while (currentTime / attackTime < 1)
        //        {
        //            if (controller.Move.isDashing || cancelAttacking)
        //                break;

        //            currentTime += Time.fixedDeltaTime;
        //            yield return cashingWaitForFixedUpdate;
        //        }
        //        IsAttacking = false;
        //        HideWeapon();

        //        currentTime = 0;
        //        //공격 딜레이
        //        while (currentTime / weaponDelay < 1)
        //        {
        //            if (controller.Move.isDashing)
        //                break;

        //            currentTime += Time.fixedDeltaTime;
        //            yield return cashingWaitForFixedUpdate;
        //        }

        //        coAttack = null;
        //    }


        //    void SetLookDir()
        //    {
        //        //Vector2 mouse = Camera.main.ScreenToWorldPoint(Managers.Input.MousePos);
        //        //float angle = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg;
        //        //controller.Move.LookDir = angle > 90 && angle < 180 || angle > -180 && angle < -90 ? Vector3.left : Vector3.right;
        //    }

        //    void SetWeaponPos()
        //    {
        //        //Vector2 mouse = Camera.main.ScreenToWorldPoint(Managers.Input.MousePos);
        //        //float angle = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg;
        //        //weaponPivot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //    }

        //    void ShowWeapon()
        //    {
        //        weapon.SetActive(true);
        //    }
        //    void HideWeapon()
        //    {
        //        weapon.SetActive(false);
        //    }
        //}

        //public void HitEnemy(Collider2D collision)
        //{
        //    //EnemyController hitController = collision.GetComponentInParent<EnemyController>();
        //    //if (hitController == null)
        //    //{
        //    //    Debug.LogError("적 컨트롤러 없음");
        //    //    return;
        //    //}

        //    //if (hitController.state.IsInvincibility)
        //    //    return;

        //    //controller.Move.immediateReadyDash = true;
        //    //controller.Move.currentJumpCount = 1;

        //    //float finalDamage = weaponDamage * CalcAccelerationDmgRatio(hitController);
        //    //hitController.OnHitUnit(finalDamage);

        //    ////반동
        //    //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Managers.Input.MousePos);
        //    //Vector2 forceDir = ((Vector2)transform.position - mousePos).normalized;

        //    ////플레이어 반동
        //    //Rebound(hitUnitPow / controller.Stat.weight, forceDir);

        //    ////적 반동
        //    //collision.GetComponentInParent<Rigidbody2D>().AddForce(-forceDir * (unitReboundPow / hitController.stat.weight));

        //    ////데미지 표시 여부에 따라 표시한다.
        //    //if (showDamageText)
        //    //{
        //    //    GameObject damageText = Managers.Resource.Instantiate("Unit/DamageText", collision.transform.position + new Vector3(0, collision.bounds.extents.y, 0), collision.transform.rotation);
        //    //    damageText.GetComponent<TextMesh>().text = $"{finalDamage}";
        //    //}
        //}

        //public void HitPlatform(Collider2D collision)
        //{
        //    //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Managers.Input.MousePos);
        //    //Vector2 forceDir = ((Vector2)transform.position - mousePos).normalized;

        //    ////강력 튕기기 오브젝트 공격 시
        //    //if (collision.name == Extension.GetEnum2Str(Define.ObjectName.ReboundObject))
        //    //{
        //    //    //TODO : 임시값
        //    //    //이거는 추후 오브젝트의 필드로 수정해야 유지보수에서 용이할 것 같다.
        //    //    controller.Move.immediateReadyDash = true;
        //    //    controller.Move.currentJumpCount = 1;
        //    //    controller.Move.cancelJumping = true;
        //    //    Rebound(hitPlatformReboundPow / controller.Stat.weight * 10, forceDir);
        //    //}
        //    //else
        //    //{
        //    //    Rebound(hitPlatformReboundPow / controller.Stat.weight, forceDir);
        //    //}
        //}

        //public void HitSwitch(Collider2D collision)
        //{
        //    //collision.GetComponent<SwitchController>().OnActiveSwitch();
        //}

        ////반동 함수
        //public void Rebound(float reboundPower, Vector2 forceDir)
        //{
        //    float weaponRotZ = weaponPivot.transform.rotation.eulerAngles.z;
        //    Vector2 vec = forceDir * reboundPower;

        //    //방향에 따른 addforce 값 수정
        //    switch ((int)(weaponRotZ + 45) / 90)
        //    {
        //        //오, 왼쪽
        //        case 0:
        //        case 2:
        //        case 4:
        //            rig.SetVelocityX(0);
        //            rig.AddForce(vec);
        //            break;
        //        //상하단
        //        case 1:
        //        case 3:
        //            rig.SetVelocityY(0);
        //            rig.AddForce(vec);
        //            break;
        //        default:
        //            Debug.LogError("잘못된 값");
        //            break;
        //    }
        //}

        ////반동 함수
        //public void Rebound(Vector2 reboundPower, Vector2 forceDir)
        //{
        //    float weaponRotZ = weaponPivot.transform.rotation.eulerAngles.z;

        //    //방향에 따른 addforce 값 수정
        //    switch ((int)(weaponRotZ + 45) / 90)
        //    {
        //        //오, 왼쪽
        //        case 0:
        //        case 2:
        //        case 4:
        //            rig.SetVelocityX(0);
        //            rig.AddForce(forceDir * reboundPower.x);
        //            break;
        //        //상하단
        //        case 1:
        //        case 3:
        //            rig.SetVelocityY(0);
        //            rig.AddForce(forceDir * reboundPower.y);
        //            break;
        //        default:
        //            Debug.LogError("잘못된 값");
        //            break;
        //    }
        //}


        ////가속 데미지 비율 계산
        ////TODO : 타겟의 이동방향이 교차된다면 타겟의 이동속도값도 추가로 계산할 것
        ////public float CalcAccelerationDmgRatio(EnemyController eController)
        ////{
        ////    //TODO : 데미지 계산 공식 추후 수정
        ////    //플레이어 + 적 속도로 계산
        ////    Rigidbody2D enemyRig = eController.GetComponent<Rigidbody2D>();
        ////    float calcVelocity = enemyRig.velocity.magnitude + rig.velocity.magnitude;
        ////    float ratio = (calcVelocity - minAccelerationValue) / (maxAccelerationValue - minAccelerationValue);
        ////    return Mathf.Lerp(minAccelerationRatio, maxAccelerationRatio, ratio);

        ////    //기존 : Player 이속만 영향
        ////    //float ratio = (rig.velocity.magnitude - minAccelerationValue) / (maxAccelerationValue - minAccelerationValue);
        ////    //return Mathf.Lerp(minAccelerationRatio, maxAccelerationRatio, ratio);
        ////}
    }
}