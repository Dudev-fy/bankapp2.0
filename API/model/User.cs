public class User
{   
    public int IDUSER {get; set;}
    public string NAME {get; set;}
    public int ID_ACCOUNT {get; set;}
    public ICollection<Statement> STATEMENTS {get; set;}
    public ICollection<Statement> STATEMENTD {get; set;}

}