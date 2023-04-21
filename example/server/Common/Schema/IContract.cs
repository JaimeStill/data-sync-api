namespace Common.Schema;
public interface IContract
{
    int Id { get; set; }
    string Name { get; set; }
    string DateCreated { get; set; }
    string LastModified { get; set; }
}