namespace ModularPatternTraining.Modules.Authentication.Models
{
    public class TokenBlackListModel
    {
        public int Id { get; set; }
        public string jti { get; set; }
        public DateTime ExpiryDate { get; set; }    
    }
}
