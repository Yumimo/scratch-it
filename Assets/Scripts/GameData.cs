using System;

[Serializable]
public class GameData
{
    public InGame game;
    public ExistingData existingPlay;
        
    [Serializable]
    public class ExistingData
    {
        public string email;
        public Reward result;
        public string clainCode;
        public bool isClaimed;
    }

    [Serializable]
    public class InGame
    {
        public string code;
        public string name;
        public string type;
        public Reward[] rewards;
        public Meta meta;

    }

    [Serializable]
    public class Meta
    {
        public string logo;
    }

    [Serializable]
    public class Reward
    {
        public string photo;
        public string name;
        public bool isWin;
    }
    
}