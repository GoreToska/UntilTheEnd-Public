using UnityEngine;

[CreateAssetMenu(fileName = "Attitude", menuName = "UTE/Attitude")]
public class NPCAttitude : ScriptableObject
{
    [SerializeField] private int _attitudeValue = 5;
    [SerializeField] private int _lowerBound = 3;
    [SerializeField] private int _upperBound = 7;

    public bool Bad()
    {
        if (_attitudeValue <= _lowerBound)
            return true;
        else
            return false;
    }

    public bool Neutral()
    {
        if (_attitudeValue > _lowerBound && _attitudeValue <= _upperBound)
            return true;
        else
            return false;
    }

    public bool Good()
    {
        if (_attitudeValue > _upperBound)
            return true;
        else
            return false;
    }

    public void AddAttitude(double value)
    {
        _attitudeValue += (int)value;

        ValidateAttitude();
    }

    public void SubAttitude(double value)
    {
        _attitudeValue -= (int)value;

        ValidateAttitude();
    }

    private void ValidateAttitude()
    {
        if (_attitudeValue > 10)
            _attitudeValue = 10;
        if (_attitudeValue < 0)
            _attitudeValue = 0;
    }
}
