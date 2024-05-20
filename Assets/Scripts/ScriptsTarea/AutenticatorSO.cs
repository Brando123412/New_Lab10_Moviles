using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Authenticator", menuName = "ScriptableObject/Authenticator", order = 0)]
public class AutenticatorSO : ScriptableObject
{
    public string _Name;
    public string _correoID;
}
