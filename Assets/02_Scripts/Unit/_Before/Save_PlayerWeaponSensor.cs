namespace DefaultSetting
{
    public class Save_PlayerWeaponSensor : BaseWeaponSensor
    {
        //public float startAttackTime = 0;
        //Save_PlayerController playerController;

        //protected override void Awake()
        //{
        //    base.Awake();
        //    playerController = GetComponentInParent<Save_PlayerController>();
        //}

        //private void OnEnable()
        //{
        //    startAttackTime = Time.time;
        //}

        //protected override void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (Time.time != startAttackTime)
        //        return;

        //    base.OnTriggerEnter2D(collision);

        //    if (checkLayerResult == false)
        //        return;

        //    //switch (collision.gameObject.layer)
        //    //{
        //    //    case (int)Define.Layer.CanGrapplePlatform:
        //    //        playerController.Attack.HitPlatform(collision);
        //    //        break;
        //    //    case (int)Define.Layer.Enemy:
        //    //        //print("몬");
        //    //        playerController.Attack.HitEnemy(collision);
        //    //        break;
        //    //    case (int)Define.Layer.LeverSwitch:
        //    //        playerController.Attack.HitSwitch(collision);
        //    //        break;
        //    //    default:
        //    //        break;
        //    //}
        //}

        ////IEnumerator CoTriggerCheck()
        ////{
        ////    //1. 변수 선언해놓고
        ////    public bool test_isCollision = false;

        ////    //2. Start에서 코루틴 실행시켜놓고
        ////    playerController.StartCoroutine(CoTriggerCheck());

        ////    while (true)
        ////    {
        ////        //3. OnTriggerEnter에 아래 코드 넣고
        ////        test_isCollision = true;

        ////        //4. 여기서 후처리 하면 될 듯?
        ////        if (test_isCollision)
        ////        {
        ////            test_isCollision = false;
        ////            print($"충돌 후처리 타임: {Time.time}");
        ////        }
        ////        yield return new WaitForFixedUpdate();
        ////    }
        ////}

        //protected override void OnHitEffect(Collider2D collision)
        //{
        //    //충돌한 지점에 이펙트 생성
        //}
    }
}