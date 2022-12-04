using System.ComponentModel.DataAnnotations;

namespace NetNet.Gateway.Entities;

public class Metadata : KeyValueEntity
{
    [Key]
    public int Id { get; set; }
}
