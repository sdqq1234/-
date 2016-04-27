using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    public float minAudioPlayRate = 0.06f;//射击音效最快的播放间隔
    private float nextAudioPlay = 0;//下次播放时间

    private static SongPlayer m_SongPlayer;//关卡数据播放器
    public AudioSource BgmAudioPlayer;//背景音乐播放器
    public SongPlayer StageSongPlayer { //对外关卡数据
        get {
            return m_SongPlayer;
        }
    }

    //每个节拍时间长度
    public static float EveryBeatsLenght {
        get {
            return m_SongPlayer.Song.GetLengthInSeconds()/1000;
        }
    }
    private AudioSource PlaneDeadPlayer;//飞机死亡音乐播放器
    private AudioSource EnemyBulletPlayer;//敌人子弹声音播放器
    private AudioSource ItemGetPlayer;//获取道具声音播放器
    //private AudioSource HitSoundPlayer;//

    private static List<AudioClip> PlaneDeadSoundList = new List<AudioClip>();
    private static List<AudioClip> EnemyBulletSoundList = new List<AudioClip>();
    private static List<AudioClip> ItemGetSoundList = new List<AudioClip>();
	// Use this for initialization
    void Awake() {
        InitAllAudioPlayer();
        m_SongPlayer = gameObject.GetComponent<SongPlayer>();
        m_SongPlayer.PlayerAudio = BgmAudioPlayer;
    }

	void Start () {
        //Init();
	}

    void Init() {
        InitAllAudioPlayer();
    }

    public void SetSongPlayer(SongData songData) {
        m_SongPlayer.SetSong(songData);
        m_SongPlayer.Play();
    }

    void InitAllAudioPlayer() {
        BgmAudioPlayer = gameObject.GetComponent<AudioSource>();
        BgmAudioPlayer.loop = true;
        BgmAudioPlayer.panLevel = 0;

        PlaneDeadPlayer = gameObject.AddComponent<AudioSource>();
        PlaneDeadPlayer.panLevel = 0;

        EnemyBulletPlayer = gameObject.AddComponent<AudioSource>();
        EnemyBulletPlayer.volume = 0.3f;
        EnemyBulletPlayer.panLevel = 0;
        //EnemyBulletPlayer.ignoreListenerVolume = true;

        ItemGetPlayer = gameObject.AddComponent<AudioSource>();
        ItemGetPlayer.panLevel = 0;
    }

    //public static float GetBeatsLenght() { 
    //    return
    //}

    //添加死亡声音
    public static void AddDeadSound(AudioClip sound) {
        PlaneDeadSoundList.Add(sound);
    }

    public static void AddBulletSound(AudioClip sound) {
        EnemyBulletSoundList.Add(sound);
    }

    public static void AddItemGetSound(AudioClip sound) {
        ItemGetSoundList.Add(sound);
    }

    string EnemyBulletAudioName;//当前播放的敌人子弹声音
    string GetItemAudioName;//当前播放的得到道具的声音
    void UpdateAudioList() {
        while (PlaneDeadSoundList.Count > 0)
        {
            PlaneDeadPlayer.PlayOneShot(PlaneDeadSoundList[PlaneDeadSoundList.Count - 1]);
            PlaneDeadSoundList.Remove(PlaneDeadSoundList[PlaneDeadSoundList.Count - 1]);
        }
        while (ItemGetSoundList.Count > 0)
        {
            if (GetItemAudioName == null)
            {
                ItemGetPlayer.PlayOneShot(ItemGetSoundList[ItemGetSoundList.Count - 1]);
                GetItemAudioName = ItemGetSoundList[ItemGetSoundList.Count - 1].name;
            }
            else {
                if (GetItemAudioName != ItemGetSoundList[ItemGetSoundList.Count - 1].name) {
                    ItemGetPlayer.PlayOneShot(ItemGetSoundList[ItemGetSoundList.Count - 1]);
                }
            }
            ItemGetSoundList.Remove(ItemGetSoundList[ItemGetSoundList.Count - 1]);
        }

        while (EnemyBulletSoundList.Count > 0)
        {
            if (EnemyBulletAudioName == null)
            {
                EnemyBulletPlayer.PlayOneShot(EnemyBulletSoundList[EnemyBulletSoundList.Count - 1]);
                EnemyBulletAudioName = EnemyBulletSoundList[EnemyBulletSoundList.Count - 1].name;
            }
            else
            {
                if (EnemyBulletAudioName != EnemyBulletSoundList[EnemyBulletSoundList.Count - 1].name)
                {
                    EnemyBulletPlayer.PlayOneShot(EnemyBulletSoundList[EnemyBulletSoundList.Count - 1]);
                }
            }
            EnemyBulletSoundList.Remove(EnemyBulletSoundList[EnemyBulletSoundList.Count - 1]);
        }   
    }

	// Update is called once per frame
	void Update () {
        if (Time.time > nextAudioPlay)
        {
            nextAudioPlay = Time.time + minAudioPlayRate;
            UpdateAudioList();
            EnemyBulletAudioName = null;
            GetItemAudioName = null;
        }
        
	}
}
