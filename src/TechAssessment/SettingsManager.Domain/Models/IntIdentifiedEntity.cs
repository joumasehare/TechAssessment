﻿namespace SettingsManager.Domain.Models;

public abstract class IntIdentifiedEntity : IEntity<int>
{
    public int Id { get; set; }
}