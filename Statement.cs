public class Statement
{
    public int IDSTATEMENT {get; set;}
    public char OPERATION {get; set;}
    public double VALUE {get; set;}
    public int? ID_SOURCE {get; set;}
    public string SOURCE {get; set;}
    public int? ID_DESTINY {get; set;}
    public string DESTINY {get; set;}
    public DateTime DATA_HORA {get; set;}
    public User UserSourceName {get; set;}
    public User UserDestinyName {get; set;}
}
