using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;

    public List<LittleBullet> _littleBullet;
    public List<BigBullet> _bigBullet;
    public List<RetardementBullet> _retardementBullet;
    public List<FragmentingBullet> _fragmentingBulletList;
    public List<Vector3> _firstBossPositionSecondAttackScheme;

    public int _fragmentingNbrFirstBoss = 4;
    public float _distanceToParkourFirstBoss = 4f;
    public int _duplicationToDOFirstBoss = 4;
    public float _distanceSpanwBossFirstBoss = 0.5f;

    void Start()
    {
        instance = this;

        _littleBullet = new List<LittleBullet>();
        _bigBullet = new List<BigBullet>();
        _retardementBullet = new List<RetardementBullet>();
        _fragmentingBulletList = new List<FragmentingBullet>();
    }
}
