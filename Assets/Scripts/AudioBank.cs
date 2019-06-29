using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBank : MonoBehaviour
{
    public static AudioBank instance;
    public AudioClip backgroundMusic;
    public AudioClip swordPickup;

    public AudioClip demonHurt;
    public AudioClip demonDeath;
    public AudioClip harpyHurt;
    public AudioClip harpyDeath;
    public AudioClip impHurt;
    public AudioClip impDeath;

    public AudioClip levelFinished;

    public Hashtable entitySoundsTable = new Hashtable();

    // elements of the sounds hash table to allow for easy lookup of entity sound clips
    public class EntitySounds
    {
        public EntitySounds(AudioClip s_hurt, AudioClip s_death)
        {
            hurt = s_hurt;
            death = s_death;
        }

        public AudioClip hurt;
        public AudioClip death;
    }

    private void Awake()
    {
        // fill hashtable
        // TODO: Replace with actual sounds
        EntitySounds tempEntry = new EntitySounds(impHurt, demonHurt);
        entitySoundsTable.Add(EnemyStatus.EnemyType.Demon, tempEntry);
        entitySoundsTable.Add(EnemyStatus.EnemyType.Harpy, tempEntry);
        entitySoundsTable.Add(EnemyStatus.EnemyType.Imp, tempEntry);
        entitySoundsTable.Add(EnemyStatus.EnemyType.Boss, new EntitySounds(impHurt, levelFinished));
        /*
        entitySoundsTable.Add(EnemyStatus.EnemyType.Demon, new EntitySounds(demonHurt, demonDeath));
        entitySoundsTable.Add(EnemyStatus.EnemyType.Harpy, new EntitySounds(harpyHurt, harpyDeath));
        entitySoundsTable.Add(EnemyStatus.EnemyType.Imp, new EntitySounds(impHurt, impDeath));
        */
    }
}
