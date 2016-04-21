using UnityEngine;
using System.Collections;

public class SongPlayer : MonoBehaviour
{
	public SongData Song;

	protected float SmoothAudioTime = 0f;
	protected bool AudioStopEventFired = false;
	protected bool WasPlaying = false;
	protected bool IsSongPlaying = false;
    public AudioSource PlayerAudio;

	void Update()
	{
		if( IsPlaying() )
		{
			AudioStopEventFired = false;
			WasPlaying = true;
			UpdateSmoothAudioTime();
		}
	}

	protected void OnSongStopped()
	{
        if (!PlayerAudio.clip)
		{
			return;
		}


		
		//I want to check if the song has finished playing automatically.
		//Sometimes this is triggered when the song is at the end, 
		//and sometimes it has already been reset to the beginning of the song.
        if (PlayerAudio.time == PlayerAudio.clip.length
         || (WasPlaying && PlayerAudio.time == 0))
		{
			IsSongPlaying = false;
            //GetComponent<GuitarGameplay>().OnSongFinished();
		}
	}

	protected void UpdateSmoothAudioTime()
	{
		//Smooth PlayerAudio time is used because the PlayerAudio.time has smaller discreet steps and therefore the notes wont move
		//as smoothly. This uses Time.deltaTime to progress the PlayerAudio time
		SmoothAudioTime += Time.deltaTime;

		if( SmoothAudioTime >= PlayerAudio.clip.length )
		{
			SmoothAudioTime = PlayerAudio.clip.length;
			OnSongStopped();
		}

		//Sometimes the PlayerAudio jumps or lags, this checks if the smooth PlayerAudio time is off and corrects it
		//making the notes jump or lag along with the PlayerAudio track
		if( IsSmoothAudioTimeOff() )
		{
			CorrectSmoothAudioTime();
		}
	}

	protected bool IsSmoothAudioTimeOff()
	{
		//Negative SmoothAudioTime means the songs playback is delayed
		if( SmoothAudioTime < 0 )
		{
			return false;
		}

		//Dont check this at the end of the song
		if( SmoothAudioTime > PlayerAudio.clip.length - 3f )
		{
			return false;
		}

		//Check if my smooth time and the actual PlayerAudio time are of by 0.1
		return Mathf.Abs( SmoothAudioTime - PlayerAudio.time ) > 0.1f;
	}

	protected void CorrectSmoothAudioTime()
	{
		SmoothAudioTime = PlayerAudio.time;
	}

	public void Play()
	{
		IsSongPlaying = true;

		if( SmoothAudioTime < 0 )
		{
			StartCoroutine( PlayDelayed( Mathf.Abs( SmoothAudioTime ) ) );
		}
		else
		{
			PlayerAudio.Play();
			SmoothAudioTime = PlayerAudio.time;
		}
	}

	protected IEnumerator PlayDelayed( float delay )
	{
		yield return new WaitForSeconds( delay );

		PlayerAudio.Play();
	}

	public void Pause()
	{
		IsSongPlaying = false;
		PlayerAudio.Pause();
	}

	public void Stop()
	{
		PlayerAudio.Stop();
		WasPlaying = false;
		IsSongPlaying = false;
	}

	public bool IsPlaying()
	{
		return IsSongPlaying;
	}

	public void SetSong( SongData song )
	{
		Song = song;
		PlayerAudio.time = 0;
		PlayerAudio.clip = Song.BackgroundTrack;
		PlayerAudio.pitch = 1;

		SmoothAudioTime = MyMath.BeatsToSeconds( -Song.AudioStartBeatOffset, Song.BeatsPerMinute );
	}

	public float GetCurrentBeat( bool songDataEditor = false )
	{
		if( songDataEditor )
		{
			SmoothAudioTime = PlayerAudio.time;
		}

		return MyMath.SecondsToBeats( SmoothAudioTime, Song.BeatsPerMinute ) - Song.AudioStartBeatOffset;
	}
}