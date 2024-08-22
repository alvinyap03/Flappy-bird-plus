using SQLite4Unity3d;

namespace MyGameNamespace
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int SinglePlayerHighScore { get; set; }
        public int MultiplayerScore { get; set; }
    }
}
