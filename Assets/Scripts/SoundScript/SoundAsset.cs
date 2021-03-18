using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="SoundSet",menuName ="Sound/SoundSet")]
public class SoundAsset : ScriptableObject
{
    public AudioClip[] clips;
}
