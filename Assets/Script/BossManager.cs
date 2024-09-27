using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;

    public List<Bullet> _littleBullet;
    public List<Bullet> _bigBullet;
    public List<RetardementBullet> _retardementBulletList;
    public List<FragmentingBullet> _fragmentingBulletList;
    public List<Vector3> _firstBossPositionSecondAttackScheme;

    public int _fragmentingNbrFirstBoss = 4;
    public float _distanceToParkourFirstBoss = 4f;
    public int _duplicationToDOFirstBoss = 4;
    public float _distanceSpanwBossFirstBoss = 0.5f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _littleBullet = new List<Bullet>();
        _bigBullet = new List<Bullet>();
        _retardementBulletList = new List<RetardementBullet>();
        _fragmentingBulletList = new List<FragmentingBullet>();
    }
}
