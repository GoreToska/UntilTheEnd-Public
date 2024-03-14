using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "UTE/Skills")]
public class CharacterSkills : ScriptableObject
{
    [SerializeField] private int _law; // �����
    [SerializeField] private int _charter; // �������
    [SerializeField] private int _savvy; // ��������

    public int Law
    { get { return _law; } set { _law = value; } }

    public int Charter
    { get { return _charter; } set { _charter = value; } }

    public int Savvy
    { get { return _savvy; } set { _savvy = value; } }
}
