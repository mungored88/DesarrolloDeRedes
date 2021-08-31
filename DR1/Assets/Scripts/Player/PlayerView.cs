using UnityEngine;

public class PlayerView 
{
    Player _player;
    AnimatorController _animator;
    SoundMananger _soundMananger;

    public PlayerView(Player p, AnimatorController a, SoundMananger s)
    {
        _player = p;
        _animator = a;
        _soundMananger = s;
    }

    public AnimatorController animator { get { return _animator; } }
    public SoundMananger sound{ get { return _soundMananger; } }

}
