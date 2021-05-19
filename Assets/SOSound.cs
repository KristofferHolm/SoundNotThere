using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SOSound", menuName = "ScriptableObjects/SOSound", order = 1)]
public class SOSound : ScriptableObject
{
    [SerializeField]
    public List<AudioID> SoundList = new List<AudioID>();
    //[SerializeField]
    //public SoundBoard SoundBoardToGetListFrom;
    //private void OnValidate()
    //{
    //    if (SoundList.Count == 0)
    //    {
    //        if(SoundBoardToGetListFrom != null )
    //            SoundBoardToGetListFrom = SoundBoard.Instance;
    //        foreach (var item in SoundBoardToGetListFrom.soundText)
    //        {
    //            var audioID = new AudioID();
    //            System.Enum.TryParse(item, out SoundBoard.Sound sEnum);
    //            audioID.Sound = sEnum;
    //            audioID.Length = SoundBoardToGetListFrom.GetsSoundLength(sEnum);
    //            SoundList.Add(audioID);
    //        }

    //    }
            
    //}
    [System.Serializable]
    public class AudioID
    {
        public SoundBoard.Sound Sound;
        public AudioClip Clip;
        public float Length;
    }
}

