using System.ComponentModel.DataAnnotations;

namespace NetNet.Gateway.Entities;

public class SessionAffinityOptionSetting : KeyValueEntity
{
    [Key]
    public int Id { get; set; }
}
