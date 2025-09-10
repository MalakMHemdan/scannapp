using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<int>
{
   

    public string Firstname { get; set; }
    public string Lastname { get; set; }
}

public class Admain : User
{
    public int BranchID { get; set; }
    
}

public class SuperAdmain : User
{
    public int BranchID { get; set; }
    
}
