using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//These were used when I stored the song data in Xml files
//I left them here if someone wants to switch
//See unused code at the bottom

/*using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;*/

[System.Serializable]
public class Note
{
    public enum ObjectType //游戏里的基础物品类型
    {
        //以下是敌人
        EnemyA0,
        EnemyA1,
        EnemyA2,
        EnemyA3,
        EnemyA4,
        EnemyA5,
        EnemyA6,
        EnemyA7,
        EnemyA8,
        EnemyB1,
        EnemyC0,
        EnemyC1,
        EnemyC2,
        EnemyC3,
        EnemyGhost0,
        EnemyGhost1,
        EnemyGhost2,
        EnemyGhost3,
        EnemyMaoYu,
        EnemyStone0,
        EnemyUFO0,
        EnemyYYY,
        //以下是子弹
        EnmeyBullet,
        //以下是boss
        BossElf01,
        BossElf04,
        BossElf05,
        BossFreya,
        BossKomachi,
        BossUUZ,
        BossYoumu,
        BossYuka,
        //以下是指令
        Command_ShowStageTitle,//显示关卡名称指令
    }
    public ObjectType StageObjectType = ObjectType.EnemyA0;

    public static Keyframe SpeedMinKey = new Keyframe(0,1);
    public static Keyframe SpeedMidKey1 = new Keyframe(2,0);
    public static Keyframe SpeedMidKey2 = new Keyframe(3, 0);
    public static Keyframe SpeedMaxKey = new Keyframe(5, 1);

    public static Keyframe DirXMinKey = new Keyframe(0,0);
    public static Keyframe DirXMaxKey = new Keyframe(5, 0);

    public static Keyframe DirYMinKey = new Keyframe(0, -1);
    public static Keyframe DirYMaxKey = new Keyframe(5, -1);
    
    //public System.Enum Enemy;
	public float Time;
    public Color StageColor = CommandString.EnemyRedColor;//关卡默认红色
    public Vector2 AppearPos = Vector2.zero;//出现时的位置屏幕左上为0,0坐标
    //public float AppearSpeed = 80;//出现时的速度
    public Vector2 AppearSpeed = -Vector2.up;//出现时的方向，默认为向下,数值为1
    public float tweenTime = 1;//速度变换时间
    public int StringIndex;
    public AnimationCurve Speed_Curve = new AnimationCurve(SpeedMinKey, SpeedMidKey1, SpeedMidKey2, SpeedMaxKey);//速度值曲线
    public AnimationCurve DirX_Curve = new AnimationCurve(DirXMinKey, DirXMaxKey); //x速度方向动画曲线
    public AnimationCurve DirY_Curve = new AnimationCurve(DirYMinKey, DirYMaxKey);//y速度方向动画曲线
    public float EnemyCount = 1;//在该节奏点出现的敌人数有多少
    
}

public class SongData : ScriptableObject
{
    //public string Name;//音乐名称
    //public string Band;//乐队名称

	public float BeatsPerMinute;//曲速
    public float AudioStartBeatOffset;//起始偏移
	public AudioClip BackgroundTrack;//音频文件

	[HideInInspector]
	public List<Note> Notes = new List<Note>();

	public SongData()
	{

	}

    //根据时间和类型返回节奏点编号
	public int GetNoteIndex( float time, int stringIndex )
	{
		for( int i = 0; i < Notes.Count; ++i )
		{
			if( Notes[ i ].Time < time )
			{
				continue;
			}

			if( Notes[ i ].Time == time && Notes[ i ].StringIndex == stringIndex )
			{
				return i;
			}

			if( Notes[ i ].Time > time )
			{
				return -1;
			}
		}
		return -1;
	}

    /// <summary>
    /// 根据编号移除某个节奏点
    /// </summary>
    /// <param name="index"></param>
	public void RemoveNote( int index )
	{
		if( index >= 0 && index < Notes.Count )
		{
			Notes.Remove( Notes[ index ] );
		}
	}

    /// <summary>
    /// 根据时间和类型移除某个节奏点
    /// </summary>
    /// <param name="time"></param>
    /// <param name="stringIndex"></param>
	public void RemoveNote( float time, int stringIndex )
	{
		int index = GetNoteIndex( time, stringIndex );

		if( index != -1 )
		{
			RemoveNote( index );
		}
	}

    /// <summary>
    /// 添加节奏点
    /// </summary>
    /// <param name="time"></param>
    /// <param name="stringIndex"></param>
    /// <param name="length"></param>
    /// <returns></returns>
	public int AddNote( float time, int stringIndex, float length = 0f )
	{
		if( time > GetLengthInBeats() )
		{
			return -1;
		}

		Note newNote = new Note();

		newNote.Time = time;
		newNote.StringIndex = stringIndex;
		newNote.EnemyCount = length;

		//Find correct position in the list so that the list remains ordered
		for( var i = 0; i < Notes.Count; ++i )
		{
			if( Notes[ i ].Time > time )
			{
				Notes.Insert( i, newNote );
				return i;
			}
		}

		//If note wasn't inserted in the list, it will be added at the end
		Notes.Add( newNote );
		return Notes.Count - 1;
	}

	public float GetLengthInSeconds()
	{
		if( BackgroundTrack )
		{
			return BackgroundTrack.length;
		}

		return 0;
	}

	public float GetLengthInBeats()
	{
		return GetLengthInSeconds() * BeatsPerMinute / 60;
	}

	/* Unused code since I switched to a custom asset format
	 * But maybe useful if somebody wants to store song data in an XML File
	 * 
	 * public void SortNotesList()
	{
		for( int j = 0; j < Notes.Count; ++j )
		{
			for( int i = 0; i < Notes.Count - 1; ++i )
			{
				if( Notes[ i ].Time > Notes[ i + 1 ].Time )
				{
					Note temp = Notes[ i ];
					Notes[ i ] = Notes[ i + 1 ];
					Notes[ i + 1 ] = temp;
				}
			}
		}
	}

	public void WriteToXml( string filename )
	{
		XmlWriterSettings ws = new XmlWriterSettings();
		ws.NewLineHandling = NewLineHandling.Entitize;
		ws.Encoding = Encoding.UTF8;
		ws.Indent = true;

		XmlSerializer xs = new XmlSerializer( typeof( SongData ) );
		XmlWriter xmlTextWriter = XmlWriter.Create( filename, ws );

		xs.Serialize( xmlTextWriter, this );
		xmlTextWriter.Close();
	}

	protected void LoadFromXml( string xmlData )
	{
		StringReader reader = new StringReader( xmlData );
		
		XmlSerializer xdsg = new XmlSerializer( typeof( SongData ) );

		SongData newSong = (SongData)xdsg.Deserialize( reader );
		reader.Close();

		Name = newSong.Name;
		Band = newSong.Band;

		Bpm = newSong.Bpm;
		StartBeatOffset = newSong.StartBeatOffset;

		Notes = newSong.Notes;
	}*/
}