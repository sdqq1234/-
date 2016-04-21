using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SongData))]
public class SongDataEditor : Editor
{
    private Color[] colors = { Color.red,Color.green,Color.yellow,Color.blue,Color.magenta};
    private static Texture2D EnemyImage;
	//Needed to draw 2D lines and rectangles
	private static Texture2D _coloredLineTexture;
	private static Color _coloredLineColor;

    private GameObject obj_AudioManager;
    //private AudioSource MetronomeSource;
    private SongPlayer SongPlayer;

	//Dimensions of the editor
	//Song View is the one with the black background, where you can add notes etc.
	//ProgressBar, or Progress View is the small preview on the right, where you can navigate through the song
	private Rect SongViewRect;//编辑器主要可视范围
	private float SongViewProgressBarWidth = 20f;//编辑器宽度
	private float SongViewHeight = 400f; //编辑器高度

	//Metronome Vars
	private static bool UseMetronome;
	private float LastMetronomeBeat = Mathf.NegativeInfinity;

	//Helper vars to handle mouse clicks //处理鼠标点击事件用的变量
	private Vector2 MouseUpPosition = Vector2.zero;
	private bool LastClickWasRightMouseButton;

	//Currently selected note index
	private int SelectedNote = -1;

	//Song overview texture (the one on the right which you can use to navigate)
	private Texture2D ProgressViewTexture;
    private Texture2D waveTex;

	//Timer to calculate editor performance
	Timer PerformanceTimer = new Timer();

	[MenuItem( "Assets/Create/Song" )]
	public static void CreateNewSongAsset()
	{
		SongData asset = ScriptableObject.CreateInstance<SongData>();
		AssetDatabase.CreateAsset( asset, "Assets/NewSong.asset" );
		
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
	}

	protected void OnEnable()
	{
		//Setup object references
        obj_AudioManager = GameObject.Find("AudioManager");

        if (obj_AudioManager == null)
        {
            return;
        }

        SongPlayer = obj_AudioManager.GetComponent<SongPlayer>();
        //MetronomeSource = GameObject.Find( "Metronome" ).audio;

		//Prepare playback
		SongPlayer.SetSong( (SongData)target );
		LastMetronomeBeat = -Mathf.Ceil( SongPlayer.Song.AudioStartBeatOffset );

        //waveTex = AudioWaveform(SongPlayer.Song.BackgroundTrack, (int)SongViewRect.width, (int)SongViewRect.height, Color.yellow);
		RedrawProgressViewTexture();
	}

    /// <summary>
    /// 重新绘制整体进度条上的被标记的节奏点
    /// </summary>
	protected void RedrawProgressViewTexture()
	{
		int width = (int)SongViewProgressBarWidth;
		int height = (int)SongViewHeight;

		if( !ProgressViewTexture )
		{
			//Create empty texture if it doesn't exist //如果贴图不存在 则初始化一张色块贴图
			ProgressViewTexture = new Texture2D( width, height );
			ProgressViewTexture.wrapMode = TextureWrapMode.Clamp;
			ProgressViewTexture.hideFlags = HideFlags.HideAndDontSave;
		}

		//Draw Background Color
		Color[] BackgroundColor = new Color[ width * height ];
		for( int i = 0; i < width * height; ++i )
		{
			BackgroundColor[ i ] = new Color( 0.13f, 0.1f, 0.26f );
		}

		ProgressViewTexture.SetPixels( 0, 0, width, height, BackgroundColor );

		//Calculate the scale in which the notes are drawn, so they all fit into the progress view
		float totalBeats = SongPlayer.Song.GetLengthInBeats();
		float heightOfOneBeat = 1f / totalBeats * (float)height;

		//Draw all notes
		for( int i = 0; i < SongPlayer.Song.Notes.Count; ++i )
		{
			//Which string does this note belong to? 判断节奏点属于哪个类型
			int stringIndex = SongPlayer.Song.Notes[ i ].StringIndex;

			//Which color does this string have? //根据类型给予节奏点颜色
            //Color color = GuitarObject.GetComponent<GuitarGameplay>().GetColor(stringIndex);

            Color color = SongPlayer.Song.Notes[i].StageColor;

			//Calculate position of the note //计算每个激活的节奏点位置
			int y = (int)Mathf.Round( ( ( SongPlayer.Song.Notes[ i ].Time + SongPlayer.Song.AudioStartBeatOffset - 1 ) / totalBeats ) * height );
			int x = (int)( width / 2 ) + ( stringIndex - 2 ) * 4;

			//Get the trail length (0 = no trail)//获取节奏点敌人个数
			float EnemyCount = SongPlayer.Song.Notes[ i ].EnemyCount;

			//Draw 3x3 pixel rectangle
			for( int j = -1; j < 2; ++j )
			{
				for( int k = -1; k < 2; ++k )
				{
					ProgressViewTexture.SetPixel( x + j, y + k, color );
				}
			}

			//Draw trail
			if( EnemyCount > 0 )
			{
				for( int lengthY = y; lengthY < y + EnemyCount * heightOfOneBeat; ++lengthY )
				{
					ProgressViewTexture.SetPixel( x, lengthY, color );
				}
			}
		}
			
		ProgressViewTexture.Apply();
	}

	public override void OnInspectorGUI()
	{
		DrawInspector();

		//Check for mouse events
		if( Event.current.isMouse )
		{
			if( Event.current.type == EventType.mouseDown )
			{
				OnMouseDown( Event.current );
			}
			else if( Event.current.type == EventType.mouseUp )
			{
				OnMouseUp( Event.current );
			}
		}

		//Check for key input events
		if( Event.current.isKey )
		{
			if( Event.current.type == EventType.keyDown )
			{
				OnKeyDown( Event.current );
			}
		}

		if( Event.current.type == EventType.ValidateCommand )
		{
			switch( Event.current.commandName )
			{
				case "UndoRedoPerformed":
					RedrawProgressViewTexture();
					break;
			}
		}


		if( GUI.changed )
		{
			SongData targetData = target as SongData;
			if( targetData.BackgroundTrack != null && SongPlayer.Song != targetData )
			{
				SongPlayer.SetSong( targetData );
                //waveTex = AudioWaveform(SongPlayer.Song.BackgroundTrack, (int)SongViewRect.width, (int)SongViewRect.height, Color.yellow);
			}
		}

		UpdateMetronome();
		RepaintGui();
	}

	protected void OnKeyDown( Event e )
	{
		switch( e.keyCode )
		{
			case KeyCode.UpArrow:
				//Up arrow advances the song by one beat
                //if (GuitarObject.audio.time <= SongPlayer.Song.GetLengthInSeconds())
                //{
                //    GuitarObject.audio.time += MyMath.BeatsToSeconds(1f, SongPlayer.Song.BeatsPerMinute);
                //}
				
				e.Use();
				break;
			case KeyCode.DownArrow:
				//Down arrow rewinds the song by one beat
                //if( GuitarObject.audio.time >= MyMath.BeatsToSeconds( 1f, SongPlayer.Song.BeatsPerMinute ) )
                //{
                //    GuitarObject.audio.time -= MyMath.BeatsToSeconds( 1f, SongPlayer.Song.BeatsPerMinute );
                //}
                //else
                //{
                //    GuitarObject.audio.time = 0;
                //}

				e.Use();
				break;
			case KeyCode.RightControl:
				//Right CTRL plays/pauses the song
				OnPlayPauseClicked();
				e.Use();
				break;
			case KeyCode.LeftArrow:
				//Left arrow selects the previous note
				if( SelectedNote != -1 && SelectedNote > 0 )
				{
					SelectedNote--;
					Repaint();
				}
				break;
			case KeyCode.RightArrow:
				//Right arrow selects the next note
				if( SelectedNote != -1 && SelectedNote < SongPlayer.Song.Notes.Count )
				{
					SelectedNote++;
					Repaint();
				}
				break;
			case KeyCode.Delete:
				//DEL removes the currently selected note
				if( SelectedNote != -1 )
				{
                    //Undo.RegisterUndo( SongPlayer.Song, "Remove Note" );
                    Undo.RecordObject(SongPlayer.Song, "Remove Note");
					SongPlayer.Song.RemoveNote( SelectedNote );
					SelectedNote = -1;
					EditorUtility.SetDirty( target );
					RedrawProgressViewTexture();

					Repaint();
				}
				break;
			case KeyCode.Alpha1:
				AddNewNoteAtCurrentTime( 0 );
				break;
			case KeyCode.Alpha2:
				AddNewNoteAtCurrentTime( 1 );
				break;
			case KeyCode.Alpha3:
				AddNewNoteAtCurrentTime( 2 );
				break;
			case KeyCode.Alpha4:
				AddNewNoteAtCurrentTime( 3 );
				break;
			case KeyCode.Alpha5:
				AddNewNoteAtCurrentTime( 4 );
				break;
		}
	}

	void AddNewNoteAtCurrentTime( int stringIndex )
	{
		float currentBeat = Mathf.Round( ( SongPlayer.GetCurrentBeat( true ) + 1 ) * 4 ) / 4;

		Note note = SongPlayer.Song.Notes.Find( item => item.Time == currentBeat && item.StringIndex == stringIndex );

		if( note == null )
		{
			SongPlayer.Song.AddNote( currentBeat, stringIndex );
		}
		else
		{
			Debug.Log( "There is already a note at " + currentBeat + " on string " + stringIndex );
		}
	}

	protected GUIStyle GetWarningBoxStyle()
	{
		GUIStyle box = new GUIStyle( "box" );

		box.normal.textColor = EditorStyles.miniLabel.normal.textColor;
		box.imagePosition = ImagePosition.ImageLeft;
		box.stretchWidth = true;
		box.alignment = TextAnchor.UpperLeft;

		return box;
	}

	protected void WarningBox( string text, string tooltip = "" )
	{
		GUIStyle box = GetWarningBoxStyle();

		Texture2D warningIcon = (Texture2D)Resources.Load( "Warning", typeof( Texture2D ) );
		GUIContent content = new GUIContent( " " + text, warningIcon, tooltip );
		GUILayout.Label( content, box );
	}

	protected void DrawInspector()
	{
        //if( GuitarObject == null )
        //{
        //    WarningBox( "Guitar Object could not be found." );
        //    WarningBox( "Did you load the GuitarUnity scene?" );
        //    return;
        //}

		//Time the performance of the editor window
		PerformanceTimer.Clear();
		PerformanceTimer.StartTimer( "Draw Inspector" );

		GUILayout.Label( "Song Data", EditorStyles.boldLabel );

		DrawDefaultInspector();

		if( SongPlayer.Song.BackgroundTrack == null )
		{
			WarningBox( "Please set a background track!" );
			return;
		}

		if( SongPlayer.Song.BeatsPerMinute == 0 )
		{
			WarningBox( "Please set the beats per minute!" );
		}

		if( SelectedNote >= SongPlayer.Song.Notes.Count )
		{
			SelectedNote = -1;
		}

		if( SelectedNote == -1 )
		{
			//If no note is selected, still draw greyed out inspector elements 
			//so the editor doesn't jump when notes are selected and deselected

			GUI.enabled = false;
			GUILayout.Label( "No Note selected", EditorStyles.boldLabel );
            EditorGUILayout.EnumPopup("EnemyType", Note.ObjectType.EnemyA0);
			EditorGUILayout.FloatField( "Time", 0 );
            EditorGUILayout.ColorField("DeadColor", Color.white);
            EditorGUILayout.Vector2Field("AppearPos", Vector2.zero);
            //EditorGUILayout.FloatField("AppearSpeed", 80);
            EditorGUILayout.Vector2Field("AppearSpeed", Vector2.zero);
            //EditorGUILayout.CurveField("Speed_Curve", new AnimationCurve(new Keyframe(0, 0)));
            //EditorGUILayout.CurveField("SpeedDirX_Curve", new AnimationCurve(new Keyframe(0,0)));
            //EditorGUILayout.CurveField("SpeedDirY_Curve", new AnimationCurve(new Keyframe(0, 0)));
			EditorGUILayout.IntField( "String", 0 );
            EditorGUILayout.FloatField("EnemyCount", 1);

			EditorGUILayout.BeginHorizontal();
				GUILayout.Space( 15 );
				GUILayout.Button( "Remove Note" );
			EditorGUILayout.EndHorizontal();

			GUI.enabled = true;
		}
		else
		{
			//Draw Header and Next/Previous Note Buttons
			EditorGUILayout.BeginHorizontal();

				GUILayout.Label( "Note " + SelectedNote.ToString(), EditorStyles.boldLabel );
				
				if( SelectedNote == 0 )
				{
					GUI.enabled = false;
				}
				if( GUILayout.Button( "<" ) )
				{
					SelectedNote--;
				}
				GUI.enabled = true;

				if( SelectedNote == SongPlayer.Song.Notes.Count - 1 )
				{
					GUI.enabled = false;
				}
				if( GUILayout.Button( ">" ) )
				{
					SelectedNote++;
				}
				GUI.enabled = true;

			EditorGUILayout.EndHorizontal();

			//Draw note data
            //SongPlayer.Song.Notes[SelectedNote].Enemy = (Note.EnemyType)EditorGUILayout.EnumPopup("EnemyType", SongPlayer.Song.Notes[SelectedNote].Enemy) ;
            Note.ObjectType EnemyType = (Note.ObjectType)EditorGUILayout.EnumPopup("EnemyType", SongPlayer.Song.Notes[SelectedNote].StageObjectType);
			float newTime = EditorGUILayout.FloatField( "Time", SongPlayer.Song.Notes[ SelectedNote ].Time );
            Color deadColor = EditorGUILayout.ColorField("DeadColor", SongPlayer.Song.Notes[SelectedNote].StageColor);
            Vector2 appearPos = EditorGUILayout.Vector2Field("AppearPosRatio", SongPlayer.Song.Notes[SelectedNote].AppearPos);
            //float appearSpeed = EditorGUILayout.FloatField("AppearSpeed", SongPlayer.Song.Notes[SelectedNote].AppearSpeed);
            Vector2 appearSpeed = EditorGUILayout.Vector2Field("AppearSpeed", SongPlayer.Song.Notes[SelectedNote].AppearSpeed);
            
            
            //SongPlayer.Song.Notes[SelectedNote].Speed_Curve = EditorGUILayout.CurveField("Speed_Curve", SongPlayer.Song.Notes[SelectedNote].Speed_Curve);


            //AnimationCurve SpeedDirX_Curve = SongPlayer.Song.Notes[SelectedNote].DirX_Curve;
            //Keyframe xKey = new Keyframe(0, appearDir.x);
            //Keyframe yKey = new Keyframe(0, appearDir.y);
            //Keyframe[] newKeys_X = SongPlayer.Song.Notes[SelectedNote].DirX_Curve.keys;
            //Keyframe[] newKeys_Y = SongPlayer.Song.Notes[SelectedNote].DirY_Curve.keys;
            //newKeys_X[0] = xKey;
            //newKeys_Y[0] = yKey;
            //AnimationCurve SpeedDirX_Curve = new AnimationCurve(newKeys_X);
            //AnimationCurve SpeedDirY_Curve = new AnimationCurve(newKeys_Y);
            //SongPlayer.Song.Notes[SelectedNote].DirX_Curve = EditorGUILayout.CurveField("SpeedDirX_Curve", SpeedDirX_Curve);
            //SongPlayer.Song.Notes[SelectedNote].DirY_Curve = EditorGUILayout.CurveField("SpeedDirY_Curve", SpeedDirY_Curve);

			int newStringIndex = EditorGUILayout.IntField( "String", SongPlayer.Song.Notes[ SelectedNote ].StringIndex );
			float newCount = EditorGUILayout.FloatField( "EnemyCount", SongPlayer.Song.Notes[ SelectedNote ].EnemyCount );

			newStringIndex = Mathf.Clamp( newStringIndex, 0, 4 );

			//If note has changed, register undo, commit changes and redraw progress view
			if( newTime != SongPlayer.Song.Notes[ SelectedNote ].Time
				|| newStringIndex != SongPlayer.Song.Notes[ SelectedNote ].StringIndex
				|| newCount != SongPlayer.Song.Notes[ SelectedNote ].EnemyCount
                //|| appearSpeed != SongPlayer.Song.Notes[SelectedNote].AppearSpeed
                || appearSpeed != SongPlayer.Song.Notes[SelectedNote].AppearSpeed
                || appearPos != SongPlayer.Song.Notes[SelectedNote].AppearPos
                || EnemyType != SongPlayer.Song.Notes[SelectedNote].StageObjectType)
			{
                //Undo.RegisterUndo( SongPlayer.Song, "Edit Note" );
                Undo.RecordObject(SongPlayer.Song, "Edit Note");
				SongPlayer.Song.Notes[ SelectedNote ].Time = newTime;
				SongPlayer.Song.Notes[ SelectedNote ].StringIndex = newStringIndex;
				SongPlayer.Song.Notes[ SelectedNote ].EnemyCount = newCount;
                //SongPlayer.Song.Notes[SelectedNote].DeadColor = deadColor;
                SongPlayer.Song.Notes[SelectedNote].AppearPos = appearPos;
                SongPlayer.Song.Notes[SelectedNote].AppearSpeed = appearSpeed;
                //SongPlayer.Song.Notes[SelectedNote].AppearDir = appearDir;

                SongPlayer.Song.Notes[SelectedNote].StageObjectType = EnemyType;


                if (SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyA0
                    || SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyA5
                    || SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyGhost0) {
                        SongPlayer.Song.Notes[SelectedNote].StageColor = CommandString.EnemyRedColor; //默认红色
                }
                else if (SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyA1
                    || SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyA4
                    || SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyC1
                    || SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyGhost1) {
                        SongPlayer.Song.Notes[SelectedNote].StageColor = CommandString.EnemyBlueColor; //默认蓝色
                }
                else if (SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyA2
                    || SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyA6
                    || SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyGhost2)
                {
                    SongPlayer.Song.Notes[SelectedNote].StageColor = CommandString.EnemyGreenColor; //默认绿色
                }
                else if (SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyA3
                    || SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyA7
                    || SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyA8
                    || SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyGhost3)
                {
                    SongPlayer.Song.Notes[SelectedNote].StageColor = CommandString.EnemyYellowColor; //默认黄色
                }
                else if (SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyB1)
                {
                    SongPlayer.Song.Notes[SelectedNote].StageColor = CommandString.EnemyPurpleColor; //默认紫色
                }
                else if (SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyC2)
                {
                    SongPlayer.Song.Notes[SelectedNote].StageColor = CommandString.EnemyOrangeColor; //默认橘黄色
                }
                else if (SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyC3)
                {
                    SongPlayer.Song.Notes[SelectedNote].StageColor = CommandString.EnemyCyanColor; //默认青色
                }
                else if (SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnemyC0)
                {
                    SongPlayer.Song.Notes[SelectedNote].StageColor = CommandString.EnemyPinkColor; //默认粉色
                }
                else if (SongPlayer.Song.Notes[SelectedNote].StageObjectType == Note.ObjectType.EnmeyBullet)
                {
                    SongPlayer.Song.Notes[SelectedNote].StageColor = CommandString.EnemyBulletColor;
                }
				RedrawProgressViewTexture();

				Repaint();
			}

			//Remove Note Button
			//15px Space is added to the front to match the default unity style
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space( 15 );
			if( GUILayout.Button( "Remove Note" ) )
			{
                //Undo.RegisterUndo( SongPlayer.Song, "Remove Note" );
                Undo.RecordObject(SongPlayer.Song, "Remove Note");

				SongPlayer.Song.RemoveNote( SelectedNote );
				SelectedNote = -1;
				RedrawProgressViewTexture();
				EditorUtility.SetDirty( target );

				Repaint();
			}
			EditorGUILayout.EndHorizontal();
		}
			
		GUILayout.Label( "Song", EditorStyles.boldLabel );

		//Draw song player controls
		EditorGUILayout.BeginHorizontal();
			GUILayout.Space( 15 );

			string buttonLabel = "Play Song";
			if( IsPlaying() )
			{
				buttonLabel = "Pause Song";
			}

			if( GUILayout.Button( buttonLabel ) )
			{
				OnPlayPauseClicked();
			}
			if( GUILayout.Button( "Stop Song" ) )
			{
				OnStopClicked();
			}
		EditorGUILayout.EndHorizontal();

		//Add playback speed slider
		EditorGUILayout.BeginHorizontal();
			GUILayout.Space( 15 );
			GUILayout.Label( "Playback Speed", EditorStyles.label );
            //GuitarObject.audio.pitch = GUILayout.HorizontalSlider( GuitarObject.audio.pitch, 0, 1 );
		EditorGUILayout.EndHorizontal();

		//Draw Use Metronome toggle
		UseMetronome = EditorGUILayout.Toggle( "Metronome", UseMetronome );

		//Draw song editor
		SongViewRect = GUILayoutUtility.GetRect( GUILayoutUtility.GetLastRect().width, SongViewHeight ); //设定矩形可视宽高

		PerformanceTimer.StartTimer( "Draw Background" );
		DrawRectangle( 0, SongViewRect.yMin, SongViewRect.width, SongViewRect.height, Color.black );

        PerformanceTimer.StartTimer("Draw Progress View");
        DrawProgressView();

        PerformanceTimer.StartTimer("Draw Music Popu");
        DrawMusicPopu();

        PerformanceTimer.StartTimer("Draw Main View");
        DrawMainView();

		PerformanceTimer.StopTimer();

		DrawAddNotesHint();
		DrawEditorPerformancePanel();
	}

	protected void DrawAddNotesHint()
	{
		WarningBox( "Use number keys 1-5 to add notes while playing" );
	}

	protected void DrawEditorPerformancePanel()
	{
		List<TimerData> Timers = PerformanceTimer.GetTimers();

		GUILayout.Label( "Editor Performance", EditorStyles.boldLabel );

		for( int i = 0; i < Timers.Count; ++i )
		{
			float displayMs = Mathf.Round( Timers[ i ].Time * 10000 ) / 10;
			GUILayout.Label( Timers[ i ].Name + " " + displayMs + "ms" );
		}

		float deltaTime = PerformanceTimer.GetTotalTime();
		float fps = Mathf.Round( 10 / deltaTime ) / 10;
		float msPerFrame = Mathf.Round( deltaTime * 10000 ) / 10;
		GUILayout.Label( "Total " + msPerFrame + "ms/frame (" + fps + "FPS)" );
	}

	protected void UpdateMetronome()
	{
		if( !UseMetronome )
		{
			return;
		}

		if( !IsPlaying() )
		{
			return;
		}

		float currentWholeBeat = Mathf.Floor( SongPlayer.GetCurrentBeat( true ) + 0.05f );
		if( currentWholeBeat > LastMetronomeBeat )
		{
			LastMetronomeBeat = currentWholeBeat;

            //MetronomeSource.Stop();
            //MetronomeSource.time = 0f;

            //MetronomeSource.Play();
            //MetronomeSource.Pause();
            //MetronomeSource.Play();
		}
	}

	protected void RepaintGui()
	{
		if( IsPlaying() )
		{
			Repaint();
		}
	}

	protected Rect GetProgressViewRect()
	{
		return new Rect( SongViewRect.width - SongViewProgressBarWidth, SongViewRect.yMin, SongViewProgressBarWidth, SongViewRect.height );
	}

	protected bool IsPlaying()
	{
        if (obj_AudioManager == null)
        {
        return false;
        }

        return obj_AudioManager.audio.isPlaying;
	}

    //绘制编辑器主窗口
	protected void DrawMainView()
	{
		float totalWidth = SongViewRect.width - SongViewProgressBarWidth;

		if( totalWidth < 0 )
		{
			return;
		}
        
        DrawBeats();
        DrawStrings();
        DrawTimeMarker();
        DrawGridNotesAndHandleMouseClicks();
        DrawNotes();
	}

    //绘制当前时间点的波普
    protected void DrawMusicPopu() { 
        //代码写这里
        //GUI.DrawTexture(SongViewRect,waveTex,ScaleMode.ScaleToFit);
    }

    //绘制时间线
	protected void DrawTimeMarker()
	{
		float heightOfOneBeat = SongViewRect.height / 6f;

		DrawLine( new Vector2( SongViewRect.xMin, SongViewRect.yMax - heightOfOneBeat )
				, new Vector2( SongViewRect.xMax - SongViewProgressBarWidth-15, SongViewRect.yMax - heightOfOneBeat )
				, new Color( 1f, 0f, 0f )
				, 4 );
	}

    //绘制节拍类型轴
	protected void DrawStrings()
	{
		float totalWidth = SongViewRect.width - SongViewProgressBarWidth;
		float stringDistance = totalWidth / 6;

		for( int i = 0; i < 5; ++i )
		{
			float x = stringDistance * ( i + 1 );
			DrawVerticalLine( new Vector2( x, SongViewRect.yMin )
							, new Vector2( x, SongViewRect.yMax )
							, new Color( 0.4f, 0.4f, 0.4f )
							, 3 );
		}
	}

    //IEnumerator GetEnemyTexture(string url)
    //{
    //    WWW www = new WWW(url);
    //    yield return www;
    //    if (www.isDone && www.error == null)
    //    {
    //        EnemyImage = www.texture;
    //        //Debug.Log(EnemyImage.width + "  " + img.height);
    //        //byte[] data = EnemyImage.EncodeToPNG();
    //        //File.WriteAllBytes(UnityEngine.Application.streamingAssetsPath + "/Temp/temp.png", data);
    //    }
    //}  
    //protected void DrawEnemyImage(Note.EnemyType enemy,float x,float y) {
        
    //    string enemyPath = Application.dataPath + "/Sprites/Enemy/";
    //    Debug.Log(enemyPath);
    //    //AssetBundle enemyBundle = new AssetBundle();
    //    switch(enemy){
    //        case Note.EnemyType.EnemyA0:
    //            //StartCoroutine(GetEnemyTexture(enemyPath + "EnemyA1/EnemyA1_0.png"));
    //            //EnemyImage = enemyBundle.Load(enemyPath + "EnemyA1/EnemyA1_0.png") as Texture2D;
    //            break;
    //    }
    //    GUI.DrawTexture(new Rect(x,y,EnemyImage.width/2,EnemyImage.height/2),EnemyImage);
    //}

	protected void DrawNotes()
	{
		//Calculate positioning variables
		float heightOfOneBeat = SongViewRect.height / 6f;
		float totalWidth = SongViewRect.width - SongViewProgressBarWidth;
		float stringDistance = totalWidth / 6;
		
		Note note;

		for( int i = 0; i < SongPlayer.Song.Notes.Count; ++i )
		{
			note = SongPlayer.Song.Notes[ i ];

			if( note.Time > SongPlayer.GetCurrentBeat( true ) + 6.5f )
			{
				//If note is not visible, skip it and draw next note
				continue;
			}

			if( note.Time + note.EnemyCount < SongPlayer.GetCurrentBeat( true ) - 0.5f )
			{
				//If note is not visible, skip it and draw next note
				continue;
			}

			//How far has the note progressed
			float progressOnNeck = 1 - ( note.Time - SongPlayer.GetCurrentBeat( true ) ) / 6f;

			//Get note color
            //Color color = GuitarObject.GetComponent<GuitarGameplay>().GetColor( note.StringIndex );

            Color color = SongPlayer.Song.Notes[i].StageColor;

			//Get note position
			float y = SongViewRect.yMin + progressOnNeck * SongViewRect.height;
			float x = SongViewRect.xMin + ( note.StringIndex + 1 ) * stringDistance;
            //DrawEnemyImage(note.Enemy,x,y);
			//If note is selected, draw a white rectangle around it
			if( SelectedNote == i )
			{
				DrawRectangle( x - 9, y - 9, 17, 17, new Color( 1f, 1f, 1f ), SongViewRect );
			}

			//Draw note rectangle
			DrawRectangle( x - 7, y - 7, 13, 13, color, SongViewRect );

			//Draw trail
            //if( note.EnemyCount > 0 )
            //{
            //    float trailYTop = y - note.EnemyCount * heightOfOneBeat;
            //    float trailYBot = y;

            //    DrawVerticalLine( new Vector2( x, trailYBot ), new Vector2( x, trailYTop ), color, 7, SongViewRect );
            //}
		}
	}

	//This function is a little bit iffy, 
	//It not only draws the grey circles which you can click //绘制的灰色圆圈你不仅可以点击，
	//but it also handles the mouse clicks which add/remove notes//还可以在点击的时候添加和删除要用的节奏点
	protected void DrawGridNotesAndHandleMouseClicks()
	{
		//Grid notes are only drawn when the song is paused
        //if (IsPlaying())
        //{
        //    return;
        //}

		float heightOfOneBeat = SongViewRect.height / 6f;
		float totalWidth = SongViewRect.width - SongViewProgressBarWidth;
		float stringDistance = totalWidth / 6;
		float numNotesPerBeat = 4f;//每个节拍间隔之间有4个节奏点

		//Calculate the offset (from 0 to 1) how far the current beat has progressed
		float beatOffset = SongPlayer.GetCurrentBeat( true );
		beatOffset -= (int)beatOffset;

		//Get the texture of the grey circles
		Texture2D GridNoteTexture = (Texture2D)UnityEngine.Resources.Load( "GridNote", typeof( Texture2D ) ); //读取节奏点贴图

		//Draw on each of the five strings
		for( int i = 0; i < 5; ++i )
		{
			float x = stringDistance * ( i + 1 );//每条节奏类型轴的x间距

			for( int j = 0; j < 7 * numNotesPerBeat; ++j )
			{
				float y = SongViewRect.yMax - ( j / numNotesPerBeat - beatOffset ) * heightOfOneBeat;

				//Calculate beat value of this grid position
				float beat = (float)j / numNotesPerBeat + Mathf.Ceil( SongPlayer.GetCurrentBeat( true ) );

				Rect rect = new Rect( x - 7, y - 7, 13, 13 );

				if( beat > SongPlayer.Song.GetLengthInBeats() )
				{
					//Dont draw grid notes beyond song length 音乐长度以外则不画
					continue;
				}

				if( rect.yMin < SongViewRect.yMin && rect.yMax < SongViewRect.yMin )
				{
					//Dont draw grid notes that are not visible in the current frame //y轴最小值外的不画
					continue;
				}

				if( rect.yMin > SongViewRect.yMax && rect.yMax > SongViewRect.yMax )
				{
                    //Dont draw grid notes that are not visible in the current frame//y轴最大值以外的不画
					continue;
				}
				
				//Clip the draw rectangle to the song view
				rect.yMin = Mathf.Clamp( rect.yMin, SongViewRect.yMin, SongViewRect.yMax );
				rect.yMax = Mathf.Clamp( rect.yMax, SongViewRect.yMin, SongViewRect.yMax );

                GUI.DrawTexture(rect, GridNoteTexture, ScaleMode.ScaleAndCrop, true);

				//Correct mouse offset
				y -= heightOfOneBeat;

				//Check if current grid note contains the mouse position //检查鼠标位置是否在该节奏点内
                //MouseUpPosition is set to Vector2( -1337, -1337 ) if no click occured this frame //如果没点击节奏点的话设置默认鼠标位置Vector2( -1337, -1337 )
				if( rect.Contains( MouseUpPosition ) )
				{
					//Correct beat offset in positive space
					if( SongPlayer.GetCurrentBeat( true ) > 0 )
					{
						beat -= 1;
					}

					//Check if a note is already present 以下为关键的检查节奏点是否被激活操作
					SelectedNote = SongPlayer.Song.GetNoteIndex( beat, i );//获得当前选中的节奏点编号
					if( SelectedNote == -1 )
					{
						//If note wasn't present, add the note on left mouse button click 如果该节奏点为未激活状态，则遇到左键事件的时候激活它
						if( LastClickWasRightMouseButton == false )
						{
                            //Undo.RegisterUndo( SongPlayer.Song, "Add Note" );
                            Undo.RecordObject(SongPlayer.Song, "Add Note"); //给SongPlayer中的SongData中的AddNote方法添加可撤销事件
							SelectedNote = SongPlayer.Song.AddNote( beat, i );//添加选中的节奏点
							EditorUtility.SetDirty( target );//保存修改的ScriptObject对象数据
							RedrawProgressViewTexture(); //重绘贴图
						}
					}
					else
					{
						//If note is present, remove the note on right mouse button click 如果是激活状态的节奏点，则遇到鼠标右键事件的时候取消激活
						if( LastClickWasRightMouseButton )
						{
                            //Undo.RegisterUndo( SongPlayer.Song, "Remove Note" );
                            Undo.RecordObject(SongPlayer.Song, "Remove Note");
							SongPlayer.Song.RemoveNote( SelectedNote );
							SelectedNote = -1; //将未选中的节奏点标注为-1
							EditorUtility.SetDirty( target );
							RedrawProgressViewTexture();
						}
					}

					Repaint();
				}
			}
		}

		//Reset mouse values
		MouseUpPosition = new Vector2( -1337, -1337 );
		LastClickWasRightMouseButton = false;
	}

    /// <summary>
    /// 用横线绘制节拍间隔
    /// </summary>
	protected void DrawBeats()
	{
		float heightOfOneBeat = SongViewRect.height / 6f;//每个间隔直接的高度为整个节拍范围的1/6

		//Calculate the offset (from 0 to 1) how far the current beat has progressed
		float beatOffset = SongPlayer.GetCurrentBeat( true );//每个节拍间隔的高度由歌曲的时间长度计算而来
		beatOffset -= (int)beatOffset;

		for( int i = 0; i < 7; ++i )
		{
			float y = SongViewRect.yMax - ( i - beatOffset ) * heightOfOneBeat;
            DrawLine(new Vector2(SongViewRect.xMin, y)
                    , new Vector2(SongViewRect.xMax - SongViewProgressBarWidth - 15, y)
                    , new Color(0.1f, 0.1f, 0.1f)
                    , 2, SongViewRect);
		}
	}

	protected void DrawProgressView()
	{
		GUI.DrawTexture( GetProgressViewRect(), ProgressViewTexture );
		DrawProgressViewTimeMarker();
	}

	protected void DrawProgressViewBackground()
	{
		Rect rect  = GetProgressViewRect();
		DrawRectangle( rect.xMin, rect.yMin, rect.width, rect.height, new Color( 0.13f, 0.1f, 0.26f ) );
	}

	protected void DrawProgressViewTimeMarker()
	{
		Rect rect  = GetProgressViewRect();

		float previewProgress = 0f;
        if (obj_AudioManager && obj_AudioManager.audio.clip)
        {
            previewProgress = obj_AudioManager.audio.time / obj_AudioManager.audio.clip.length;
        }

		float previewProgressTop = rect.yMin + rect.height * ( 1 - previewProgress );
		DrawLine( new Vector2( rect.xMin, previewProgressTop ), new Vector2( rect.xMax + rect.width, previewProgressTop ), Color.red, 2 );
	}

	protected void OnMouseDown( Event e )
	{
		if( GetProgressViewRect().Contains( e.mousePosition ) )
		{
			OnProgressViewClicked( e.mousePosition );
		}
	}

	protected void OnMouseUp( Event e )
	{
		if( SongViewRect.Contains( e.mousePosition ) && !GetProgressViewRect().Contains( e.mousePosition ) )
		{
			OnSongViewMouseUp( e.mousePosition );

			if( e.button == 1 )
			{
				LastClickWasRightMouseButton = true;
			}
		}
	}

	protected void OnSongViewMouseUp( Vector2 mousePosition )
	{
		MouseUpPosition = mousePosition;

		Repaint();
	}

	protected void OnProgressViewClicked( Vector2 mousePosition )
	{
		float progress = 1 - (float)( mousePosition.y - SongViewRect.yMin ) / SongViewRect.height;

        obj_AudioManager.audio.time = obj_AudioManager.audio.clip.length * progress;
	}

	protected void OnPlayPauseClicked()
	{
		EditorGUIUtility.keyboardControl = 0;

		if( IsPlaying() )
		{
            obj_AudioManager.audio.Pause();
			EditorUtility.SetDirty( target );
		}
		else
		{
            obj_AudioManager.audio.Play();
            obj_AudioManager.audio.Pause();
            obj_AudioManager.audio.Play();
		}
	}

	protected void OnStopClicked()
	{
        if (!obj_AudioManager)
        {
            return;
        }

        obj_AudioManager.audio.Stop();
        obj_AudioManager.audio.time = 0f;
		LastMetronomeBeat = -Mathf.Ceil( SongPlayer.Song.AudioStartBeatOffset );
		EditorUtility.SetDirty( target );
	}

	//2D Draw Functions
	//Found on the unity forums: http://forum.unity3d.com/threads/17066-How-to-draw-a-GUI-2D-quot-line-quot/page2
	//Added clipping rectangle
	public static void DrawLine( Vector2 lineStart, Vector2 lineEnd, Color color, int thickness, Rect clip )
	{
		if( ( lineStart.y < clip.yMin && lineEnd.y < clip.yMin )
		 || ( lineStart.y > clip.yMax && lineEnd.y > clip.yMax )
		 || ( lineStart.x < clip.xMin && lineEnd.x < clip.xMin )
		 || ( lineStart.x > clip.xMax && lineEnd.x > clip.xMax ) )
		{
			return;
		}

		lineStart.x = Mathf.Clamp( lineStart.x, clip.xMin, clip.xMax );
		lineStart.y = Mathf.Clamp( lineStart.y, clip.yMin, clip.yMax );

		lineEnd.x = Mathf.Clamp( lineEnd.x, clip.xMin, clip.xMax );
		lineEnd.y = Mathf.Clamp( lineEnd.y, clip.yMin, clip.yMax );

		DrawLine( lineStart, lineEnd, color, thickness );
	}

	public static void DrawLine( Vector2 lineStart, Vector2 lineEnd, Color color, int thickness )
	{
        //if( lineStart.x == lineStart.y )
        //{
        //    DrawVerticalLine( lineStart, lineEnd, color, thickness );
        //}

		if( !_coloredLineTexture )
		{
			_coloredLineTexture = new Texture2D( 1, 1 );
			_coloredLineTexture.wrapMode = TextureWrapMode.Repeat;
			_coloredLineTexture.hideFlags = HideFlags.HideAndDontSave;
		}

		if( _coloredLineColor != color )
		{
			_coloredLineColor = color;
			_coloredLineTexture.SetPixel( 0, 0, _coloredLineColor );
			_coloredLineTexture.Apply();
		}
		DrawLineStretched( lineStart, lineEnd, _coloredLineTexture, thickness );
	}

	public static void DrawVerticalLine( Vector2 lineStart, Vector2 lineEnd, Color color, int thickness, Rect clip )
	{
		if( ( lineStart.y < clip.yMin && lineEnd.y < clip.yMin )
		 || ( lineStart.y > clip.yMax && lineEnd.y > clip.yMax )
		 || ( lineStart.x < clip.xMin && lineEnd.x < clip.xMin )
		 || ( lineStart.x > clip.xMax && lineEnd.x > clip.xMax ) )
		{
			return;
		}

		lineStart.x = Mathf.Clamp( lineStart.x, clip.xMin, clip.xMax );
		lineStart.y = Mathf.Clamp( lineStart.y, clip.yMin, clip.yMax );

		lineEnd.x = Mathf.Clamp( lineEnd.x, clip.xMin, clip.xMax );
		lineEnd.y = Mathf.Clamp( lineEnd.y, clip.yMin, clip.yMax );

		DrawVerticalLine( lineStart, lineEnd, color, thickness );
	}

    /// <summary>
    /// 绘制竖线
    /// </summary>
    /// <param name="lineStart"></param>
    /// <param name="lineEnd"></param>
    /// <param name="color"></param>
    /// <param name="thickness">粗细</param>
	public static void DrawVerticalLine( Vector2 lineStart, Vector2 lineEnd, Color color, int thickness )
	{
        //if( lineStart.x != lineEnd.x )
        //{
        //    DrawLine( lineStart, lineEnd, color, thickness );
        //    return;
        //}

		float x = lineStart.x;
		float xOffset = (float)thickness;
		float y = lineStart.y + ( lineEnd.y - lineStart.y ) / 2;
		int newThickness = (int)( Mathf.Abs( Mathf.Floor( lineStart.y - lineEnd.y ) ) );

		DrawLine( new Vector2( x - xOffset / 2, y ), new Vector2( x + xOffset / 2, y ), color, newThickness );
	}

	public static void DrawLineStretched( Vector2 lineStart, Vector2 lineEnd, Texture2D texture, int thickness )
	{
		Vector2 lineVector = lineEnd - lineStart;

		if( lineVector.x == 0 )
		{
			return;
		}

		float angle = Mathf.Rad2Deg * Mathf.Atan( lineVector.y / lineVector.x );

		if( lineVector.x < 0 )
		{
			angle += 180;
		}

		if( thickness < 1 )
		{
			thickness = 1;
		}

		// The center of the line will always be at the center
		// regardless of the thickness.
		int thicknessOffset = (int)Mathf.Ceil( thickness / 2 );

		GUIUtility.RotateAroundPivot( angle, lineStart );

		GUI.DrawTexture( new Rect( lineStart.x,
								 lineStart.y - thicknessOffset,
								 lineVector.magnitude,
								 thickness ),
						texture );

		GUIUtility.RotateAroundPivot( -angle, lineStart );
	}

	private void DrawRectangle( float left, float top, float width, float height, Color color )
	{
		DrawRectangle( new Rect( left, top, width, height ), color );
	}

	private void DrawRectangle( float left, float top, float width, float height, Color color, Rect clip )
	{
		DrawRectangle( new Rect( left, top, width, height ), color, clip );
	}

	private void DrawRectangle( Rect rect, Color color, Rect clip )
	{
		DrawVerticalLine( new Vector2( rect.xMin + rect.width / 2, rect.yMin ), new Vector2( rect.xMin + rect.width / 2, rect.yMax ), color, (int)rect.width, clip );
	}

	private void DrawRectangle( Rect rect, Color color )
	{
		DrawRectangle( rect, color, rect );
	}

    private Texture2D AudioWaveform(AudioClip aud, int width, int height, Color color)
    {
        int step = Mathf.CeilToInt((aud.samples * aud.channels) / width);
        float[] samples = new float[aud.samples * aud.channels];
        //workaround to prevent the error in the function getData
        //when Audio Importer loadType is "compressed in memory"

        string path = AssetDatabase.GetAssetPath(aud);

        AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;

        AudioImporterLoadType audioLoadTypeBackup = audioImporter.loadType;

        audioImporter.loadType = AudioImporterLoadType.StreamFromDisc;

        AssetDatabase.ImportAsset(path);



        //getData after the loadType changed

        aud.GetData(samples, 0);



        //restore the loadType (end of workaround)

        audioImporter.loadType = audioLoadTypeBackup;

        AssetDatabase.ImportAsset(path);



        Texture2D img = new Texture2D(width, height, TextureFormat.RGBA32, false);



        Color[] xy = new Color[width * height];

        for (int x = 0; x < width * height; x++)
        {
            xy[x] = new Color(0, 0, 0, 0);
        }
        img.SetPixels(xy);
        int i = 0;

        while (i < width)
        {
            int barHeight = Mathf.CeilToInt(Mathf.Clamp(Mathf.Abs(samples[i * step]) * height, 0, height));
            int add = samples[i * step] > 0 ? 1 : -1;
            for (int j = 0; j < barHeight; j++)
            {
                img.SetPixel(i, Mathf.FloorToInt(height / 2) - (Mathf.FloorToInt(barHeight / 2) * add) + (j * add), color);
            }
            ++i;
        }
        img.Apply();
        return img;
    }
}