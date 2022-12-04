﻿using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace NetNet.Gateway.Entities;

public class ActiveHealthCheckOptions : Entity<long>
{
    /// <summary>
    /// Whether active health checks are enabled.
    /// </summary>
    public bool? Enabled { get; set; }

    /// <summary>
    /// Health probe interval.
    /// </summary>
    public string Interval { get; set; }

    /// <summary>
    /// Health probe timeout, after which a destination is considered unhealthy.
    /// </summary>
    public string Timeout { get; set; }

    /// <summary>
    /// Active health check policy.
    /// </summary>
    public string Policy { get; set; }

    /// <summary>
    /// HTTP health check endpoint path.
    /// </summary>
    public string Path { get; set; }
    public int HealthCheckOptionsId { get; set; }

    public virtual HealthCheckOptions HealthCheckOptions { get; set; }
}
