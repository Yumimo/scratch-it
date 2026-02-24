using System;

[Serializable]
public class UserData
{
    public bool alreadyPlayed;
    public GameData.Reward reward;
    public string clainCode;
    public class EmailRequest
    {
        public string email;
    }
}